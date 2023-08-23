using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WhosThatPokemonAPI.DTOs;
using WhosThatPokemonAPI.Helpers;
using WhosThatPokemonAPI.Models;
using WhosThatPokemonAPI.Repositories;
using WhosThatUserAPI.Repositories;

namespace WhosThatPokemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PokemonRepository _pokeRepo;
        private readonly UserRepository _userRepo;
        private Encryption _encryption;
        private readonly AppSettings _appSettings;

        public UserController(PokemonRepository pokeRepo, UserRepository userRepo, Encryption encryption, IOptions<AppSettings> appSettings)
        {
            _pokeRepo = pokeRepo;
            _userRepo = userRepo;
            _encryption = encryption;
            _appSettings = appSettings.Value;
        }

        // Autorisé pour tout le monde
        [HttpGet("/users")]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> users = (await _userRepo.GetAll()).ToList();
            if (users.Count < 1) return NotFound("Aucun user dans la BDD !");
            return Ok(users);
        }

        // Autorisé pour tout le monde
        [HttpGet("/user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            User user = await _userRepo.GetById(id);
            if (user == null) return NotFound("Aucun utilisateur trouvé avec cet id...");
            return Ok(user);
        }

        // Autorisé pour tout le monde
        [HttpPost("/user")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (await _userRepo.Get(u => u.Pseudo == user.Pseudo) != null) return BadRequest("Un utilisateur existe déjà avec ce pseudo !");

            // Création d'un nouvel User pour éviter les problèmes dans le cas où un id est indiqué dans le json
            User userToAdd = new User(user.Pseudo, _encryption.EncryptPassword(user.Password));
            
            if (await _userRepo.Add(userToAdd)) return Ok("User ajouté avec succès !");
            return BadRequest("Erreur lors de l'ajout du user...");
        }

        // Autorisé pour tout le monde
        [HttpPut("/user/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] User user, int id)
        {
            User userFromDb = await _userRepo.GetById(id);
            if (userFromDb == null) return NotFound("L'utilisateur demandé n'a pas été trouvé...");

            user.Id = id;
            if (await _userRepo.Update(user)) return Ok("Utilisateur modifié avec succès !");

            return BadRequest("Erreur lors de la modification du user...");
        }

        // Autorisé pour tout le monde
        [HttpDelete("/user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User userFromDb = await _userRepo.GetById(id);
            if (userFromDb == null) return NotFound("L'utilisateur demandé n'a pas été trouvé...");

            if (await _userRepo.Delete(id)) return Ok("Utilisateur supprimé avec succès !");

            return BadRequest("Erreur lors de la suppression de l'utilisateur...");
        }

        // Autorisé pour tout le monde
        [HttpPost("/user/add-pokemon/{userId}/{pokemonId}")] // bool isWin: ajouter un [FromBody ?]
        public async Task<IActionResult> AddPokemonToUser(int userId, int pokemonId, bool isWin)
        {
            // Checker si l'utilisateur avec l'id rentré existe dans la BDD
            User userFromDb = await _userRepo.GetById(userId);
            if (userFromDb == null) return NotFound("L'utilisateur demandé n'a pas été trouvé...");

            // Checker si le pokémon avec l'id rentré existe dans la BDD
            Pokemon pokemonFromDb = await _pokeRepo.GetById(pokemonId);
            if (pokemonFromDb == null) return NotFound("Le pokémon demandé n'a pas été trouvé...");

            // Checker si le pokémon ne figure pas déjà dans la liste des pokémons de l'utilisateur
            UserPokemon pokemonFromUser = userFromDb.Pokemons.FirstOrDefault(p => p.PokemonId == pokemonId);
            if (pokemonFromUser != null) return BadRequest($"{userFromDb.Pseudo} possède déjà le pokémon {pokemonFromDb.Name}");

            // Si les verifs précédentes sont passées, on ajoute le pokémon à l'utilisateur
            if (await _userRepo.AddPokemonToUser(userFromDb, new UserPokemon(pokemonFromDb, userFromDb, isWin)))
                return Ok($"Le pokémon {pokemonFromDb.Name} a été ajouté à l'utilisateur {userFromDb.Pseudo} avec succès !");

            return BadRequest("Erreur lors de l'ajout du pokémon...");
        }

        // Autorisé pour tout le monde
        [HttpDelete("/user/remove-pokemon/{userId}/{pokemonId}")]
        public async Task<IActionResult> RemovePokemonFromUser(int userId, int pokemonId)
        {
            // Checker si l'utilisateur avec l'id rentré existe dans la BDD
            User userFromDb = await _userRepo.GetById(userId);
            if (userFromDb == null) return NotFound("L'utilisateur demandé n'a pas été trouvé...");

            // Erreur si l'utilisateur ne possède même pas le pokémon demandé
            UserPokemon pokemonFromUser = userFromDb.Pokemons.FirstOrDefault(p => p.PokemonId == pokemonId);
            if (pokemonFromUser == null) return NotFound($"{userFromDb.Pseudo} ne possède par encore le pokémon avec l'id {pokemonId}");

            // Si les verifs précédentes sont passées, on retire le pokémon de l'utilisateur
            if (await _userRepo.RemovePokemonFromUser(userFromDb, pokemonFromUser)) return Ok($"Pokémon retiré de {userFromDb.Pseudo} avec succès !");

            return BadRequest("Erreur lors de la suppression du pokémon...");
        }

        // Autorisé pour tout le monde
        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest) // loginRequest = Pseudo + Password
        {
            loginRequest.Password = _encryption.EncryptPassword(loginRequest.Password);

            // Checker si l'utilisteur est dans la BDD
            User user = await _userRepo.Get(u => u.Pseudo == loginRequest.Pseudo && u.Password == loginRequest.Password);
            if (user == null) return BadRequest("Mauvais identifiants");

            // Rôle
            string userRole = user.IsAdmin ? "Admin" : "User";
            List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Role, userRole),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                };

            // SigningCredentials
            SigningCredentials signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.SecretKey)),
                    SecurityAlgorithms.HmacSha256);

            // JwtSecurityToken
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _appSettings.ValidIssuer,
                audience: _appSettings.ValidAudience,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddDays(7)
                );

            // token sous forme de string
            string token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(new
            {
                Message = "Connexion réussie",
                Token = token,
                User = user
            });
        }
    }
}