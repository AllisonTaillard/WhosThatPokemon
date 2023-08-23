using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using WhosThatPokemonAPI.Repositories;
using WhosThatPokemonAPI.Helpers;
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

        // Autorisé pour tout le monde
        [HttpGet("/types")]
        public async Task<IActionResult> GetAllTypes()
        {
            List<Type> types = (await _typeRepo.GetAll()).ToList();
            if (types.Count < 1) return NotFound("Aucun type dans la BDD !");
            return Ok(types);
        }

        // Autorisé pour tout le monde
        [HttpGet("/type/{id}")]
        public async Task<IActionResult> GetTypeById(int id)
        {
            Type type = await _typeRepo.GetById(id);
            if (type == null) return NotFound("Aucun type trouvé avec cet id...");
            return Ok(type);
        }

        // Autorisé seulement pour le rôle Admin
        [Authorize(Roles = Constants.RoleAdmin)]
        [Authorize(Policy = Constants.PolicyAdmin)]
        [HttpPost("/type")]
        public async Task<IActionResult> AddType([FromBody] Type type)
        {
            if (await _typeRepo.Get(t => t.Name == type.Name) != null) return BadRequest("Un type existe déjà avec ce nom !");

            // Création d'un nouvel User pour éviter les problèmes dans le cas où un id est indiqué dans le json
            Type typeToAdd = new Type(type.Name);

            if (await _typeRepo.Add(typeToAdd)) return Ok("Type ajouté avec succès !");
            return BadRequest("Erreur lors de l'ajout du type...");
        }

        // Autorisé seulement pour le rôle Admin
        [Authorize(Roles = Constants.RoleAdmin)]
        [Authorize(Policy = Constants.PolicyAdmin)]
        [HttpPut("/type/{id}")]
        public async Task<IActionResult> UpdateType([FromBody] Type type, int id)
        {
            Type typeFromDb = await _typeRepo.GetById(id);
            if (typeFromDb == null) return NotFound("Le type demandé n'a pas été trouvé...");

            type.Id = id;
            if (await _typeRepo.Update(type)) return Ok("Type modifié avec succès !");

            return BadRequest("Erreur lors de la modification du type...");
        }

        // Autorisé seulement pour le rôle Admin
        [Authorize(Roles = Constants.RoleAdmin)]
        [Authorize(Policy = Constants.PolicyAdmin)]
        [HttpDelete("/type/{id}")]
        public async Task<IActionResult> DeleteType(int id)
        {
            Type typeFromDb = await _typeRepo.GetById(id);
            if (typeFromDb == null) return NotFound("Le type demandé n'a pas été trouvé...");

            if (await _typeRepo.Delete(id)) return Ok("Type supprimé avec succès !");

            return BadRequest("Erreur lors de la suppression du Type...");
        }
    }
}
