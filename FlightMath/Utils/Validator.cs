using System.Text.RegularExpressions;

namespace FlightMath.Utils
{
    public class Validator
    {
        private readonly Regex _iataCode = new Regex(@"[A-Z]{3}", RegexOptions.Compiled);

        public bool ValidateIATACode(string IataCode)
        {
            return _iataCode.Matches(IataCode).Count == 1;
        }
    }
}