using YataModel;
using YataModel.Storage;
using CommandLine;
using CommandLine.Text;
using Yata.Cmd;
using Yata.Cmd.Controller;

public partial class Program
{


    //Q: How do I run this to add a task?
    //A: dotnet run add --task "This is a test" --due 2021-01-01    
    private static void Main(string[] args)
    {        
        CmdController controller = new CmdController(new FileStoreImpl());

        var parser = new CommandLine.Parser(with => with.HelpWriter = null);
        var parserResult = parser.ParseArguments<AddOptions, DueOptions, ScheduleOptions, RemindOptions, CompleteOptions, TickleOptions>(args);
        parserResult
            .WithParsed<AddOptions>(options => controller.RunAddAndReturnExitCode(options))
            .WithParsed<DueOptions>(options => controller.RunDueAndReturnExitCode(options))
            .WithParsed<ScheduleOptions>(options => controller.RunScheduleReturnExitCode(options))
            .WithParsed<RemindOptions>(options => controller.RunRemindAndReturnExitCode(options))
            .WithParsed<CompleteOptions>(options => controller.RunCompleteAndReturnExitCode(options))
            .WithParsed<TickleOptions>(options => controller.RunShowTicklerAndReturnExitCode(options))
            .WithNotParsed(errs => controller.DisplayHelp(parserResult, errs));
    }    
}