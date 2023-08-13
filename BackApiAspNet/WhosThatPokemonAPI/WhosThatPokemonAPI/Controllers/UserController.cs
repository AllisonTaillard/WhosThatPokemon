using Microsoft.AspNetCore.Mvc;
using WhosThatPokemonAPI.Models;
using WhosThatPokemonAPI.Repositories;

namespace WhosThatPokemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PokemonRepository _pokeRepo;
        private readonly IRepository<User> _userRepo;

        public UserController(PokemonRepository pokeRepo, IRepository<User> userRepo)
        {
            _pokeRepo = pokeRepo;
            _userRepo = userRepo;
        }

        [HttpGet("/users")]
        public async Task<IActionResult> GetAllUsers()
        {
            List<User> users = (await _userRepo.GetAll()).ToList();
            if (users.Count < 1) return NotFound("Aucun user dans la BDD !");
            return Ok(users);
        }

        [HttpPost("/user")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (await _userRepo.Get(u => u.Pseudo == user.Pseudo) != null) return BadRequest("Un utilisateur existe déjà avec ce pseudo !");
            if (await _userRepo.Add(user)) return Ok("User ajouté avec succès !");
            return BadRequest("Erreur lors de l'ajout du user...");
        }

        [HttpPut("/user/{id}")]
        public async Task<IActionResult> UpdateUser([FromBody] User user, int id)
        {
            User userFromDb = await _userRepo.GetById(id);
            if (userFromDb == null) return NotFound("L'utilisateur demandé n'a pas été trouvé...");

            user.Id = id;
            if (await _userRepo.Update(user)) return Ok("Utilisateur modifié avec succès !");

            return BadRequest("Erreur lors de la modification du user...");
        }

        [HttpDelete("/user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            User userFromDb = await _userRepo.GetById(id);
            if (userFromDb == null) return NotFound("L'utilisateur demandé n'a pas été trouvé...");

            if (await _userRepo.Delete(id)) return Ok("Utilisateur modifié avec succès !");

            return BadRequest("Erreur lors de la modification de l'utilisateur...");
        }
    }
}
