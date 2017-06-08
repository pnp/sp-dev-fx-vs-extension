using System.Text.RegularExpressions;

namespace Framework.VSIX
{
    internal static class ParameterHelper
    {
        public static string AddOrUpdateCommandParameter(string commandString, string parameter, string value, bool deleteIfEmpty = false)
        {
            if (deleteIfEmpty && string.IsNullOrWhiteSpace(value))
            {
                commandString = Regex.Replace(commandString, $@"\s*{parameter}\s*""[^""]*""", string.Empty);
            }
            else
            {
                commandString = !commandString.Contains(parameter)
                    ? $@"{commandString} {parameter} ""{value}"""
                    : Regex.Replace(commandString, $@"\s*{parameter}\s*""[^""]*""", $@" {parameter} ""{value}""");
            }
            return commandString;
        }
    }
}