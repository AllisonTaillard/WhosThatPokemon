using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WhosThatPokemonAPI.Models
{
    [Table("Pokemon")]
    public class Pokemon
    {
        [Column("id")]
        [Required]
        public int Id { get; set; }

        [Column("name")]
        [Required]
        public string Name { get; set; }

        [Column("picture")]
        public string Picture { get; set; }

        // Relation ManyToMany entre User et Pokemon
        [JsonIgnore]
        public List<UserPokemon> Users { get; set; }

        // Relation ManyToMany entre Type et Pokemon
        [Required]
        public List<Type> Types { get; set; }

        public Pokemon()
        {
            Users = new List<UserPokemon>();
            Types = new List<Type>();
        }

        public Pokemon(string name, string picture) : this()
        {
            Name = name;
            Picture = picture;
        }

        public Pokemon(string name, List<Type> types, string picture, List<UserPokemon> users) : this(name, picture)
        {
            Types = types;
            Users = users;
        }

        public Pokemon(int id, string name, List<Type> types, string picture, List<UserPokemon> users) : this(name, types, picture, users)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"Numéro du Pokédex : {Id}, Nom du Pokémon : {Name}";
        }
    }
}
