using System.ComponentModel.DataAnnotations;

namespace WhosThatPokemonAPI.DTOs
{
    public class LoginRequestDTO
    {
        [Required]
        public string Pseudo { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
