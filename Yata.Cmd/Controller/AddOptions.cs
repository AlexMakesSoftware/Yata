using CommandLine;

namespace Yata.Cmd.Controller
{
    [Verb("add", HelpText = "Add a new task to the list.")]
    class AddOptions
    {
        //options for adding a task
        [Option(Group = "add", Required = true, HelpText = "The description of the task.")]
        public string? Task { get; set; }

        [Option(Group = "add", HelpText = "The due date of the task. In the format YYYY-MM-DD.")]
        public DateTime? Due { get; set; }
    }
}