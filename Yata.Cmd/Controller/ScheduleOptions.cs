using CommandLine;

namespace Yata.Cmd.Controller
{
    [Verb("schedule", HelpText = "Assign an unscheduled task a date")]
    class ScheduleOptions
    {
        [Option(Group = "schedule", Required = true, HelpText = "The id number of the task.")]
        public string? Id { get; set; }

        [Option(Group = "due", Required = true, HelpText = "The due date of the task. In the format YYYY-MM-DD.")]
        public DateTime? Due { get; set; }
    }
}