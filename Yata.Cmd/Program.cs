using YataModel;
using YataModel.Storage;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using Yata.Cmd;

public class Program
{

    private static string _banner = @"
░░    ░░  ░░░░░  ░░░░░░░░  ░░░░░    
 ▒▒  ▒▒  ▒▒   ▒▒    ▒▒    ▒▒   ▒▒ 
  ▒▒▒▒   ▒▒▒▒▒▒▒    ▒▒    ▒▒▒▒▒▒▒ 
   ▓▓    ▓▓   ▓▓    ▓▓    ▓▓   ▓▓ 
   ██    ██   ██    ██    ██   ██ v0.0.1";

    private static IStorage _store = new FileStoreImpl();

    //test with  dotnet run add --task "This is a test" --due 2021-01-01

    [Verb("add", HelpText = "Add a new task to the list.")]
    class AddOptions
    {
        //options for adding a task
        [Option(Group = "add", Required = true, HelpText = "The description of the task.")]
        public string? Task { get; set; }

        [Option(Group = "add", HelpText = "The due date of the task. In the format YYYY-MM-DD.")]
        public DateTime? Due { get; set; }
    }

    [Verb("due", HelpText = "See the list of tasks that are due to be completed now (or earlier).")]
    class DueOptions
    {
    }

    [Verb("tidy", HelpText = "Assign unscheduled tasks a date or stick them in the tickler file.")]
    class TidyOptions
    {
    }

    [Verb("tickle", HelpText = "Review the tickler file.")]
    class TickleOptions
    {
    }


    private static void DisplayHelp(ParserResult<object> result, IEnumerable<Error> errs)
    {
        var helpText = HelpText.AutoBuild(result, h =>
        {
            h.AdditionalNewLineAfterOption = false;
            h.Heading = _banner;
            h.Copyright = "Freeware (c) 2022 Alex Connell"; //TODO: update this
            return HelpText.DefaultParsingErrorsHandler(result, h);
        }, e => e);

        PrettyPrinter.Info(helpText);
        PrettyPrinter.Info("Example: yata add --task \"Washing up.\"");
    }


    private static int RunDueAndReturnExitCode(DueOptions opts)
    {
        PrettyPrinter.Info("Outstanding tasks due now (or earlier):");

        //get the list of tasks due now
        List<YTask> dueTasks = _store.DueJobs();
        //sort it by due date
        dueTasks.Sort((x, y) => x.Due.CompareTo(y.Due));
        //print it out
        foreach (YTask task in dueTasks)
        {
            PrettyPrinter.PrintTask(task);
        }

        return 0;
    }


    private static int RunAddAndReturnExitCode(AddOptions opts)
    {
        //Throw exception if opts.Task is null
        if (opts.Task == null)
        {
            PrettyPrinter.Error("No task description specified!");
            return 1;
        }

        YTask task = new YTask(opts.Task);
        PrettyPrinter.Info("Adding a new task to the list:" + task);

        if (opts.Due.HasValue)
        {
            task.Due = opts.Due.Value;
        }
        else
        {
            task.Due = DateTime.Now;
        }

        //add the task to the list
        _store.AddTask(task);
        //save the list
        _store.SaveTodos();

        return 0;
    }


    private static void RunTickleAndReturnExitCode(TickleOptions options)
    {
        //show list of tasks in the state Tickler
        List<YTask> tickler = _store.GetAllInState(TaskState.Tickler);
        //display the list                
        PrettyPrinter.Info("Tasks in the tickler file:");
        foreach (YTask task in tickler)
        {
            PrettyPrinter.PrintTask(task);
        }
    }


    private static void RunTidyReturnExitCode(TidyOptions options)
    {
        //Show list of tasks in the state Unscheduled
        List<YTask> unscheduled = _store.GetAllInState(TaskState.Unsorted);
        //display the list

        PrettyPrinter.Info("Unscheduled tasks:");
        foreach (YTask task in unscheduled)
        {
            PrettyPrinter.PrintTask(task);
        }
        PrettyPrinter.Break();

        //ask the user if they want to assign a date to each task or move it to the tickler file.
        PrettyPrinter.Info("Do you want to assign a date to each task or move it to the tickler file? (a/t)");
        string? answer = Console.ReadLine();
        if (answer == "a")
        {
            schedule(unscheduled);
        }
        else if (answer == "t")
        {
            putInTickler(unscheduled);
        }
        else
        {
            PrettyPrinter.Error("Invalid answer. Please try again.");
        }

        //Save or abort?
        //loop until valid input
        bool valid = true;
        do
        {
            valid = true;
            PrettyPrinter.Info("Do you want to save your changes? (y/n)");
            string? answer3 = Console.ReadLine();
            if (answer3 == "y")
            {
                _store.SaveTodos();
            }
            else if (answer3 == "n")
            {
                PrettyPrinter.Error("Aborting...");
            }
            else
            {
                PrettyPrinter.Error("Invalid answer. Please try again.");
                valid = false;
            }
        }
        while (!valid);

        PrettyPrinter.Info("Saving changes...");
        _store.SaveTodos();
        PrettyPrinter.Info("Done.");
    }

    private static void putInTickler(List<YTask> unscheduled)
    {
        //move each task to the tickler file
        foreach (YTask task in unscheduled)
        {
            bool validInput = true;
            do
            {
                validInput = true;
                PrettyPrinter.PrintTask(task);
                PrettyPrinter.Info("Enter s to skip, q to quit, or else t to assign to the tickler file.");
                string? answer2 = Console.ReadLine();
                switch (answer2)
                {
                    case "s":
                        continue; //skip this task (go to next iteration of the loop
                    case "q":
                        break;
                    case "t":
                        task.State = TaskState.Tickler;
                        break;
                    default:
                        //invalid input
                        PrettyPrinter.Error("Invalid input. Please try again.");
                        validInput = false;
                        break;
                }
            } while (!validInput);
        }
    }

    private static void schedule(List<YTask> unscheduled)
    {
        //assign a date to each task
        foreach (YTask task in unscheduled)
        {
            PrettyPrinter.PrintTask(task);
            PrettyPrinter.Info("Press s to skip or else enter a date (in the format YYYY-MM-DD) to assign to this task.");

            Boolean validInput = false;
            do
            {
                validInput = true;
                string? date = Console.ReadLine();

                switch (date)
                {
                    case null:
                        PrettyPrinter.Error("No date specified!");
                        validInput = false;
                        break;
                    case "s":
                        continue;
                    default:
                        task.Due = DateTime.Parse(date); //TODO: check for valid date.
                        task.State = TaskState.Scheduled;
                        break;
                }
            } while (!validInput);

        }
    }


    //Q: How do I run this to add a task?
    //A: dotnet run add --task "This is a test" --due 2021-01-01    
    private static void Main(string[] args)
    {
        _store.LoadTodos();

        var parser = new CommandLine.Parser(with => with.HelpWriter = null);
        var parserResult = parser.ParseArguments<AddOptions, DueOptions, TidyOptions, TickleOptions>(args);
        parserResult
            .WithParsed<AddOptions>(options => RunAddAndReturnExitCode(options))
            .WithParsed<DueOptions>(options => RunDueAndReturnExitCode(options))
            .WithParsed<TidyOptions>(options => RunTidyReturnExitCode(options))
            .WithParsed<TickleOptions>(options => RunTickleAndReturnExitCode(options))
            .WithNotParsed(errs => DisplayHelp(parserResult, errs));
    }
}