using CommandLine;

namespace Yata.Cmd.Controller
{
    [Verb("done", HelpText = "Mark a task as complete.")]
    class CompleteOptions
    {
        [Option(Group = "done", Required = true, HelpText = "The id number of the task.")]
        public string? Id { get; set; }
    }
}