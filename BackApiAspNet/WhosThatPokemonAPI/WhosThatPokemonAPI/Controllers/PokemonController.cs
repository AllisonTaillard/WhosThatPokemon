using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhosThatPokemonAPI.Models;
using WhosThatPokemonAPI.Repositories;
using WhosThatPokemonAPI.Helpers;
using Type = WhosThatPokemonAPI.Models.Type;

namespace WhosThatPokemonAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly PokemonRepository _pokeRepo;
        private readonly IRepository<Type> _typeRepo;

        public PokemonController(PokemonRepository pokeRepo, IRepository<Type> typeRepo)
        {
            _pokeRepo = pokeRepo;
            _typeRepo = typeRepo;
        }

        // Autorisé pour tout le monde
        [HttpGet("/pokemons")]
        public async Task<IActionResult> GetAllPokemons()
        {
            List<Pokemon> pokemons = (await _pokeRepo.GetAll()).ToList();
            if (pokemons.Count < 1) return NotFound("Aucun pokémon dans la BDD !");
            return Ok(pokemons);
        }

        // Autorisé pour tout le monde
        [HttpGet("/pokemon/{id}")]
        public async Task<IActionResult> GetPokemonById(int id)
        {
            Pokemon pokemon = await _pokeRepo.GetById(id);
            if (pokemon == null) return NotFound("Aucun pokémon trouvé avec cet id...");
            return Ok(pokemon);
        }

        // Autorisé seulement pour le rôle Admin
        [Authorize(Roles = Constants.RoleAdmin)]
        [Authorize(Policy = Constants.PolicyAdmin)]
        [HttpPost("/pokemon")]
        public async Task<IActionResult> AddPokemon([FromBody] Pokemon pokemon)
        {
            if (await _pokeRepo.Get(p => p.Name == pokemon.Name) != null) return BadRequest("Un pokémon existe déjà avec ce nom !");

            // Création d'un nouveau Pokémon pour éviter les problèmes dans le cas où un id est indiqué dans le json
            Pokemon pokemonToAdd = new Pokemon(pokemon.Name, pokemon.Picture);

            if (await _pokeRepo.Add(pokemonToAdd)) return Ok("Pokémon ajouté avec succès !");
            return BadRequest("Erreur lors de l'ajout du Pokémon...");
        }

        // Autorisé seulement pour le rôle Admin
        [Authorize(Roles = Constants.RoleAdmin)]
        [Authorize(Policy = Constants.PolicyAdmin)]
        [HttpPut("/pokemon/{id}")]
        public async Task<IActionResult> UpdatePokemon([FromBody] Pokemon pokemon, int id)
        {
            Pokemon pokemonFromDb = await _pokeRepo.GetById(id);
            if (pokemonFromDb == null) return NotFound("Le pokémon demandé n'a pas été trouvé...");

            pokemon.Id = id;
            if (await _pokeRepo.Update(pokemon)) return Ok("Pokémon modifié avec succès !");

            return BadRequest("Erreur lors de la modification du Pokémon...");
        }

        // Autorisé seulement pour le rôle Admin
        [Authorize(Roles = Constants.RoleAdmin)]
        [Authorize(Policy = Constants.PolicyAdmin)]
        [HttpDelete("/pokemon/{id}")]
        public async Task<IActionResult> DeletePokemon(int id)
        {
            Pokemon pokemonFromDb = await _pokeRepo.GetById(id);
            if (pokemonFromDb == null) return NotFound("Le pokémon demandé n'a pas été trouvé...");

            if (await _pokeRepo.Delete(id)) return Ok("Pokémon supprimé avec succès !");

            return BadRequest("Erreur lors de la suppression du Pokémon...");
        }

        // Autorisé seulement pour le rôle Admin
        [Authorize(Roles = Constants.RoleAdmin)]
        [Authorize(Policy = Constants.PolicyAdmin)]
        [HttpPost("/pokemon/add-existing-type/{pokemonId}/{typeId}")]
        public async Task<IActionResult> AddExistingTypeToPokemon(int pokemonId, int typeId)
        {
            // Checker si le pokémon avec l'id rentré existe dans la BDD
            Pokemon pokemonFromDb = await _pokeRepo.GetById(pokemonId);
            if (pokemonFromDb == null) return NotFound("Le pokémon demandé n'a pas été trouvé...");

            // Checker si le type avec l'id rentré existe dans la BDD
            Type typeFromDb = await _typeRepo.GetById(typeId);
            if (typeFromDb == null) return NotFound("Le type demandé n'a pas été trouvé...");

            // Checker si le type ne figure pas déjà dans la liste des types du Pokémon
            for (int i = 0; i < pokemonFromDb.Types.Count; i++)
            {
                if (pokemonFromDb.Types[i].Name == typeFromDb.Name) return BadRequest($"{pokemonFromDb.Name} possède déjà le type {typeFromDb.Name}");
                // Checker si il reste une place pour un type (que le Pokémon n'a pas déjà 2 types)
                if (i == 1) return BadRequest(new
                {
                    Message = $"{pokemonFromDb.Name} possède déjà 2 types. Un Pokémon ne peut pas avoir 3 types.",
                    Pokemon = pokemonFromDb
                });
            }

            // Si les verifs précédentes sont passées, on ajoute le type au Pokémon
            if (await _pokeRepo.AddTypeToPokemon(pokemonFromDb, typeFromDb)) return Ok(new
            {
                Message = $"Le type {typeFromDb.Name} a été ajouté au Pokémon {pokemonFromDb.Name} avec succès !",
                Pokemon = pokemonFromDb,
            });

            return BadRequest("Erreur lors de l'ajout du type...");
        }

        // Autorisé seulement pour le rôle Admin
        [Authorize(Roles = Constants.RoleAdmin)]
        [Authorize(Policy = Constants.PolicyAdmin)]
        [HttpDelete("/pokemon/remove-type/{pokemonId}/{typeId}")]
        public async Task<IActionResult> RemoveTypeFromPokemon(int pokemonId, int typeId)
        {
            // Checker si le pokémon avec l'id rentré existe dans la BDD
            Pokemon pokemonFromDb = await _pokeRepo.GetById(pokemonId);
            if (pokemonFromDb == null) return NotFound("Le pokémon demandé n'a pas été trouvé...");

            // Erreur si le pokémon ne possède même pas le type demandé
            Type type = pokemonFromDb.Types.FirstOrDefault(t => t.Id == typeId);
            if (type == null) return NotFound($"{pokemonFromDb.Name} ne possède pas encore le type {type.Name}");

            // Si les verifs précédentes sont passées, on supprime le type du Pokémon
            if (await _pokeRepo.RemoveTypeFromPokemon(pokemonFromDb, type)) return Ok($"Type supprimé de {pokemonFromDb.Name} avec succès !");

            return BadRequest("Erreur lors de la suppression du type...");
        }
    }
}