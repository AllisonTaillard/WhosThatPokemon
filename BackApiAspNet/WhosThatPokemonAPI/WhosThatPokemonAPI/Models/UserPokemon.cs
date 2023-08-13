using System.ComponentModel.DataAnnotations.Schema;

namespace WhosThatPokemonAPI.Models
{
    [Table("UserPokemon")]
    public class UserPokemon // table intermediaire
    {
        public int Id { get; set; }
        public int PokemonId { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(PokemonId))]
        public Pokemon Pokemon { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        // Pour déterminer si le user a deviné le pokémon en question ou non
        public bool IsWin { get; set; }
    }
}
