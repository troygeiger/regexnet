using CommandLine;
using System.Collections.Generic;

namespace regexnet
{
    class ClOptions
    {
        [Option('s', "string", HelpText = "The input string to process. (This can also be piped in. Ex. echo \"Hello\" | ./regexnet -e '\\w')")]
        public string Input { get; set; }

        [Option('e', "expression", HelpText = "The regular expression query.", Required = true)]
        public string Expression { get; set; }

        [Option('a', "match-all", HelpText = "If set, matching will continue until no more matches are found.")]
        public bool MatchAll { get; set; }

        [Option('g', Separator = ',', HelpText = "Specify the group(s) to return in a comma separated list. Ex. -g 1,2", SetName = "group")]
        public IEnumerable<int> ReturnGroups { get; set; }

        [Option("ml", HelpText = "Return group results as multi-line; otherwise single-lines are returned.", SetName = "group")]
        public bool OutputMultiLine { get; set; }

        [Option('i', "ignore", HelpText = "Ignore Case option is applied to the expression.")]
        public bool IgnoreCase { get; set; }

        [Option('m', "multiline", HelpText = "Multi-Line option is applied to the expression.")]
        public bool MultiLine { get; set; }

        [Option('n', "no-line-break", HelpText = "Don't output a line break at end of output.")]
        public bool NoEndLineBreak { get; set; }
    }
}
