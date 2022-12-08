using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using YataModel;
using YataModel.Storage;

namespace Yata.Cmd.Controller
{

    public class CmdController
    {
            private static string _banner = @"
░░    ░░  ░░░░░  ░░░░░░░░  ░░░░░    
 ▒▒  ▒▒  ▒▒   ▒▒    ▒▒    ▒▒   ▒▒ 
  ▒▒▒▒   ▒▒▒▒▒▒▒    ▒▒    ▒▒▒▒▒▒▒ 
   ▓▓    ▓▓   ▓▓    ▓▓    ▓▓   ▓▓ 
   ██    ██   ██    ██    ██   ██ v0.0.1";



        private IStorage _store;

        public CmdController(IStorage store)
        {
            this._store = store;
            _store.LoadTodos();
        }

        internal void DisplayHelp(ParserResult<object> result, IEnumerable<Error> errs)
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


        internal int RunDueAndReturnExitCode(DueOptions opts)
        {
            PrettyPrinter.Info("Outstanding tasks due now (or earlier):");

            //get the list of tasks due now
            List<YTask> dueTasks = _store.DueJobs();

            //print it out
            foreach (YTask task in dueTasks)
            {
                PrettyPrinter.PrintTask(task);
            }

            return 0;
        }


        internal int RunAddAndReturnExitCode(AddOptions opts)
        {
            //Throw exception if opts.Task is null
            if (opts.Task == null)
            {
                PrettyPrinter.Error("No task description specified!");
                return 1;
            }

            int id = _store.AddTask(opts.Task);
            PrettyPrinter.Info("Adding a new task to the list with id:" + id);

            if (opts.Due.HasValue)
            {
                _store.SetTaskDue(id, opts.Due.Value);
            }
            else
            {
                _store.SetTaskDue(id, DateTime.Now);
            }
            _store.SaveTodos();

            return 0;
        }


        internal void RunTickleAndReturnExitCode(TickleOptions options)
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


        internal void RunRemindAndReturnExitCode(RemindOptions options)
        {
            if (options.Id == null)
            {
                PrettyPrinter.Error("No task id specified!");
                return;
            }
            int id = int.Parse(options.Id);

            if (_store.GetTask(id) == null)
            {
                PrettyPrinter.Error("No task with id " + id + " found!");
                return;
            }

            //set the state to Tickler
            _store.SetTaskState(id, TaskState.Tickler);
            _store.SaveTodos();
            PrettyPrinter.Info("Task " + id + " added to Tickler.");
        }


        internal void RunCompleteAndReturnExitCode(CompleteOptions options)
        {
            if (options.Id == null)
            {
                PrettyPrinter.Error("No task id specified!");
                return;
            }
            int id = int.Parse(options.Id);

            if (_store.GetTask(id) == null)
            {
                PrettyPrinter.Error("No task with id " + id + " found!");
                return;
            }

            _store.SetTaskState(id, TaskState.Completed);
            _store.SaveTodos();
            PrettyPrinter.Info("Task " + id + " completed.");
        }


        internal void RunShowTicklerAndReturnExitCode(TickleOptions options)
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


        internal void RunScheduleReturnExitCode(ScheduleOptions options)
        {
            if (options.Due == null)
            {
                PrettyPrinter.Error("Bad parameters!");
                return;
            }

            if (options.Id == null)
            {
                PrettyPrinter.Error("No task id specified!");
                return;
            }

            int id = int.Parse(options.Id);

            //get the task
            YTask? task = _store.GetTask(id);
            if (task == null)
            {
                PrettyPrinter.Error("No task with id " + id + " found!");
                return;
            }

            //set the due date
            DateTime due = options.Due.Value;  

            _store.SetTaskDue(id, due);
            _store.SaveTodos();
            PrettyPrinter.Info("Task " + id + " scheduled for " + due);
        }
    }
}