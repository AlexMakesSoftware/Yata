using CommandLine;

namespace Yata.Cmd.Controller
{
    [Verb("later", HelpText = "Consign the task to the tickler file.")]
    class RemindOptions
    {
        [Option(Group = "later", Required = true, HelpText = "The id number of the task.")]
        public string? Id { get; set; }
    }
}