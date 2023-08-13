using Microsoft.AspNetCore.Mvc;
using WhosThatPokemonAPI.Repositories;
using Type = WhosThatPokemonAPI.Models.Type;

namespace WhosThatPokemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly IRepository<Type> _typeRepo;

        public TypeController(IRepository<Type> typeRepo)
        {
            _typeRepo = typeRepo;
        }

        [HttpGet("/types")]
        public async Task<IActionResult> GetAllTypes()
        {
            List<Type> types = (await _typeRepo.GetAll()).ToList();
            if (types.Count < 1) return NotFound("Aucun type dans la BDD !");
            return Ok(types);
        }

        [HttpPost("/type")]
        public async Task<IActionResult> AddType([FromBody] Type type)
        {
            if (await _typeRepo.Get(t => t.Name == type.Name) != null) return BadRequest("Un type existe déjà avec ce nom !");
            if (await _typeRepo.Add(type)) return Ok("Type ajouté avec succès !");
            return BadRequest("Erreur lors de l'ajout du type...");
        }

        [HttpPut("/type/{id}")]
        public async Task<IActionResult> UpdateType([FromBody] Type type, int id)
        {
            Type typeFromDb = await _typeRepo.GetById(id);
            if (typeFromDb == null) return NotFound("Le type demandé n'a pas été trouvé...");

            type.Id = id;
            if (await _typeRepo.Update(type)) return Ok("Type modifié avec succès !");

            return BadRequest("Erreur lors de la modification du type...");
        }

        [HttpDelete("/type/{id}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            Type typeFromDb = await _typeRepo.GetById(id);
            if (typeFromDb == null) return NotFound("Le type demandé n'a pas été trouvé...");

            if (await _typeRepo.Delete(id)) return Ok("Type modifié avec succès !");

            return BadRequest("Erreur lors de la modification du Type...");
        }
    }
}
