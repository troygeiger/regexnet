using System;
using CommandLine;
using System.Text.RegularExpressions;
using System.Linq;

namespace regexnet
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
}
