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
            Parsed<ClOptions> parsed = Parser.Default.ParseArguments<ClOptions>(args) as Parsed<ClOptions>;
            if (parsed == null) return;
            ClOptions cl = parsed.Value;
            if (Console.IsInputRedirected)
            {
                cl.Input = Console.In.ReadToEnd();
            }

            Regex regex = new Regex(cl.Expression, BuildRegexOptions(cl));
            var match = regex.Match(cl.Input);
            while (match.Success)
            {
                if (cl.ReturnGroups.Count() == 0)
                {
                    WriteOutput(match.Value, cl.MultiLine);
                }
                else
                {
                    foreach (int g in cl.ReturnGroups)
                    {
                        WriteOutput(match.Groups[g].Value, cl.MultiLine);
                    }
                }
                if (!cl.MatchAll) break;
                match = match.NextMatch();
            }
            if (!cl.NoEndLineBreak)
            {
                Console.WriteLine();
            }
        }

        static RegexOptions BuildRegexOptions(ClOptions args)
        {
            RegexOptions options = RegexOptions.None;
            options |= args.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
            options |= args.MultiLine ? RegexOptions.Multiline : RegexOptions.Singleline;
            return options;
        }

        static void WriteOutput(string value, bool writeLine)
        {
            if (writeLine)
            {
                Console.WriteLine(value);
            }
            else
            {
                Console.Write(value);
            }
        }
    }

    class ClOptions
    {
        [Option('s', "string", HelpText = "The input string to process. (This can also be piped in. Ex. echo \"Hello\" | ./regexy -e '\\w')")]
        public string Input { get; set; }

        [Option('e', "expresion", HelpText = "The regular expression query.", Required = true)]
        public string Expression { get; set; }

        [Option('a', "match-all", HelpText = "If set, matching will continue until no more matches are found.")]
        public bool MatchAll { get; set; }

        [Option('g', Separator = ',', HelpText = "Specify the group to return in a comma separated list. Ex. -g 1,2", SetName = "group")]
        public IEnumerable<int> ReturnGroups { get; set; }

        [Option("ml", HelpText = "Return group results as multi-line; otherwise single-lines are returned.", SetName = "group")]
        public bool OutputMultiLine { get; set; }

        [Option('i', "ignore", HelpText = "Ignore string case.")]
        public bool IgnoreCase { get; set; }

        [Option('m', "multiline", HelpText = "Regex is multi-lined.")]
        public bool MultiLine { get; set; }

        [Option('n', "no-line-break", HelpText = "Don't output a line break at end of output.")]
        public bool NoEndLineBreak { get; set; }
    }
}
