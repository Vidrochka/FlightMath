using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FlightMath.Utils
{
    public class ParametersParser
    {
        private readonly Regex _floatElementsValue = new Regex(@"[0-9]+\,[0-9]{1}\,|[0-9]+\,[0-9]{1}$|[0-9]+", RegexOptions.Compiled);
        private readonly Regex _namderElementsValue = new Regex(@"[0-9]+", RegexOptions.Compiled);
        private readonly Regex _stringElementsValue = new Regex(@"[a-zA-Z]+", RegexOptions.Compiled);
        private readonly Regex _stringAndNumElementsValue = new Regex(@"[0-9a-zA-Z]+", RegexOptions.Compiled);
        private readonly Regex _datesElementsValue = new Regex(@"[0-9]{2}-[0-9]{2}-[0-9]{2}", RegexOptions.Compiled);

        public IEnumerable<string> ParseFloatElements(string row)
            => ParseElements(_floatElementsValue, row);

        public IEnumerable<string> ParseNumberElements(string row) 
            => ParseElements(_namderElementsValue, row);

        public IEnumerable<string> ParseStringElements(string row)
            => ParseElements(_stringElementsValue, row);

        public IEnumerable<string> ParseStringAndNumElements(string row)
            => ParseElements(_stringAndNumElementsValue, row);

        public IEnumerable<string> ParseDateElements(string row)
            => ParseElements(_datesElementsValue, row);

        private IEnumerable<string> ParseElements(Regex regex, string row) 
            => regex.Matches(row).Select(match => match.Value);
    }
}