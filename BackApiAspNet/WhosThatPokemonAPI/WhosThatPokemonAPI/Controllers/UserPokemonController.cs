using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WhosThatPokemonAPI.Models;
using WhosThatPokemonAPI.Repositories;

namespace WhosThatPokemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserPokemonController : ControllerBase
    {
        private readonly IRepository<UserPokemon> _userPokeRepo;

        public UserPokemonController(IRepository<UserPokemon> userPokeRepo)
        {
            _userPokeRepo = userPokeRepo;
        }

        [HttpGet("/userPokemons")]
        public async Task<IActionResult> GetAllUserPokemons()
        {
            List<UserPokemon> userPokemons = (await _userPokeRepo.GetAll()).ToList();
            if (userPokemons.Count < 1) return NotFound("Aucun userPokemon dans la BDD !");
            return Ok(userPokemons);
        }

        [HttpPost("/userPokemon")]
        public async Task<IActionResult> AddUserPokemon([FromBody] UserPokemon userPokemon)
        {
            if (await _userPokeRepo.Get(u => u.Id == userPokemon.Id) != null) return BadRequest("Un userPokemon existe déjà avec ce nom !");
            if (await _userPokeRepo.Add(userPokemon)) return Ok("UserPokemon ajouté avec succès !");
            return BadRequest("Erreur lors de l'ajout du userPokemon...");
        }

        [HttpPut("/userPokemon/{id}")]
        public async Task<IActionResult> UpdateUserPokemon([FromBody] UserPokemon userPokemon, int id)
        {
            UserPokemon userPokemonFromDb = await _userPokeRepo.GetById(id);
            if (userPokemonFromDb == null) return NotFound("Le userPokemon demandé n'a pas été trouvé...");

            userPokemon.Id = id;
            if (await _userPokeRepo.Update(userPokemon)) return Ok("UserPokemon modifié avec succès !");

            return BadRequest("Erreur lors de la modification du userPokemon...");
        }

        [HttpDelete("/userPokemon/{id}")]
        public async Task<IActionResult> DeleteUserPokemon(int id)
        {
            UserPokemon userPokemonFromDb = await _userPokeRepo.GetById(id);
            if (userPokemonFromDb == null) return NotFound("Le userPokemon demandé n'a pas été trouvé...");

            if (await _userPokeRepo.Delete(id)) return Ok("UserPokemon modifié avec succès !");

            return BadRequest("Erreur lors de la modification du UserPokemon...");
        }
    }
}
