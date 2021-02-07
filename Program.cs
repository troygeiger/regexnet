using System;
using CommandLine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;

namespace regexy
{
    class Program
    {
        static void Main(string[] args)
        {
            Parsed<ClOptions> cl = Parser.Default.ParseArguments<ClOptions>(args) as Parsed<ClOptions>;
            if (cl == null) return;
            if (Console.IsInputRedirected)
            {
                cl.Value.Input = Console.In.ReadToEnd();
            }
            Regex regex = new Regex(cl.Value.Expression, BuildRegexOptions(cl.Value));
            var match = regex.Match(cl.Value.Input);
            if (!match.Success) return;
            if (cl.Value.ReturnGroups.Count() == 0)
            {
                Console.Write(match.Value);
                return;
            }
            if (cl.Value.ReturnAsSingleLine)
            {
                foreach (int g in cl.Value.ReturnGroups)
                {
                    Console.Write(match.Groups[g].Value);
                }
            }
            else
            {
                foreach (int g in cl.Value.ReturnGroups)
                {
                    Console.WriteLine(match.Groups[g].Value);
                }
            }
            if (!cl.Value.NoEndLineBreak)
                Console.WriteLine(); 
        }

        static RegexOptions BuildRegexOptions(ClOptions args)
        {
            RegexOptions options = RegexOptions.None;
            options |= args.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
            options |= args.MultiLine ? RegexOptions.Multiline : RegexOptions.Singleline;
            return options;
        }
    }

    class ClOptions
    {
        [Option('s', "string", HelpText = "The input string to process. (This can also be piped in. Ex. echo \"Hello\" | ./regexy -e '\\w')")]
        public string Input { get; set; }

        [Option('e', "expresion", HelpText = "The regular expression query.", Required = true)]
        public string Expression { get; set; }

        [Option('g', Separator = ',', HelpText = "Specify the group to return in a comma separated list. Ex. -g 1,2", SetName = "group")]
        public IEnumerable<int> ReturnGroups { get; set; }

        [Option('l', "group-inline", HelpText = "Return group results as single line.", SetName = "group")]
        public bool ReturnAsSingleLine { get; set; }

        [Option('i', "ignore", HelpText = "Ignore string case.")]
        public bool IgnoreCase { get; set; }

        [Option('m', "multiline", HelpText = "Regex is multi-lined.")]
        public bool MultiLine { get; set; }

        [Option('n', "no-line-break", HelpText = "Don't output a line break at end of output.")]
        public bool NoEndLineBreak { get; set; }
    }
}
