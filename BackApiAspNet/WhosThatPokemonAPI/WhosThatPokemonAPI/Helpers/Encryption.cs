using System.Text;

namespace WhosThatPokemonAPI.Helpers
{
    public class Encryption
    {
        private readonly string _securityKey = "oui";
        public string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return "";

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password + _securityKey));
        }
    }
}
