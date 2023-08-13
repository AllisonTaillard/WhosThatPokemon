using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WhosThatPokemonAPI.Models
{
    [Table("Type")]
    public class Type
    {
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        // Relation ManyToMany entre Type et Pokemon
        [JsonIgnore]
        public List<Pokemon> Pokemons { get; set; }

        public Type()
        {

        }

        public Type(string name)
        {
            Name = name;
        }

        public Type(int id, string name) : this(name)
        {
            Id = id;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
