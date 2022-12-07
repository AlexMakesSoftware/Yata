using YataModel;
using YataModel.Storage;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

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
    class AddOptions {
        //options for adding a task
        [Option(Group = "add", Required = true, HelpText = "The description of the task.")]
        public string? Task { get; set; }

        [Option(Group = "add", HelpText = "The due date of the task. In the format YYYY-MM-DD.")]
        public DateTime? Due { get; set; }
    }

    //Q:What's the dotnet command to run this with arguments for main?
    //A: dotnet run -- add -d "Do the dishes" -D 2021-01-01


    [Verb("due", HelpText = "See the list of tasks that are due to be completed now (or earlier).")]
    class DueOptions {

    }

    private static void Main(string[] args)
    {
        //Q;What's the best way to handle command-line switches?
        //A: Use a library like CommandLineParser
        
        _store.LoadTodos();

  var parser = new CommandLine.Parser(with => with.HelpWriter = null);
  var parserResult = parser.ParseArguments<AddOptions,DueOptions>(args);
  parserResult
    .WithParsed<AddOptions>(options => RunAddAndReturnExitCode(options))
    .WithParsed<DueOptions>(options => RunDueAndReturnExitCode(options))
    .WithNotParsed(errs => DisplayHelp(parserResult, errs));



		// return CommandLine.Parser.Default.ParseArguments<AddOptions,DueOptions>(args)
	    //     .MapResult(
        //         (AddOptions opts) => RunAddAndReturnExitCode(opts),
        //         (DueOptions opts) => RunDueAndReturnExitCode(opts),                
        //         errs => 1);
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
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(helpText);        
        Console.ResetColor();
        Console.WriteLine("Example: yata add --task \"Washing up.\"");
    }

    private static int RunDueAndReturnExitCode(DueOptions opts)
    {
        Console.WriteLine("Outstanding tasks due now (or earlier):");
        
        //get the list of tasks due now
        List<YTask> dueTasks = _store.DueJobs();
        //sort it by due date
        dueTasks.Sort((x,y) => x.Due.CompareTo(y.Due));
        //print it out
        foreach (YTask task in dueTasks)
        {
            Console.WriteLine(task.ToString());
        }        

        return 0;
    }

    //Q: How do I run this to add a task?
    //A: dotnet run add --task "This is a test" --due 2021-01-01

    private static int RunAddAndReturnExitCode(AddOptions opts)
    {
        //Throw exception if opts.Task is null
        if (opts.Task == null)
        {
            throw new System.ArgumentNullException("No task description speicified!");
        }

        Console.WriteLine("Adding a new task to the list:"+opts);
        
        //Add a new task to the list
        YTask task = new YTask(opts.Task);        
        
        //if opts.Due is set, split it into a date and time
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
}