using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WhosThatPokemonAPI.Validators;

namespace WhosThatPokemonAPI.Models
{
    [Table("User")]
    public class User
    {
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("pseudo")]
        [Required]
        public string Pseudo { get; set; }

        [Column("level")]
        [Required]
        public int Level { get; set; }

        [Column("xp")]
        [Required]
        public int Xp { get; set; }

        [Column("password")]
        [PasswordValidator]
        public string Password { get; set; }

        [Column("isAdmin")]
        public bool IsAdmin { get; set; }


        // Relation ManyToMany entre User et Pokemon
        public List<UserPokemon> Pokemons { get; set; }

        public override string ToString()
        {
            return Pseudo;
        }
    }
}
