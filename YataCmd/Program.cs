using YataModel;
using YataModel.storage;
using System.Collections.Generic;

public class Program
{
    private static void Main(string[] args)
    {
        //print to console in color
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Welcome to Yet Another TODO App!");
        Console.ForegroundColor = ConsoleColor.White;

        IStorage store = new FileStoreImpl();
        store.LoadTodos();

        //sort tasks by due date
        //

        List<YTask> todaysJobs = store.JobsDueToday();

        //print out today's jobs
        Console.WriteLine("Today's Jobs:");
        //if todaysJobs is empty, print out "No jobs scheduled for today"
        if (todaysJobs.Count == 0)
        {
            Console.WriteLine("No jobs scheduled for today");
        }
        else 
        {
            //printout todaysjobs list
            foreach (YTask task in todaysJobs)
            {
                Console.WriteLine(task.Description);
            }
        }

        //make this pink
        Console.ForegroundColor = ConsoleColor.Magenta;
        //print list of jobs that are overdue
        Console.WriteLine("Overdue Jobs:");
        //if overdueJobs is empty, print out "No jobs overdue"
        if (store.OverdueJobs().Count == 0)
        {
            Console.WriteLine("No jobs overdue");
        }
        else
        {
            //printout overdueJobs list
            foreach (YTask task in store.OverdueJobs())
            {
                Console.WriteLine(task.Description);
            }
        }


        //stats
        int totalUnsorted = store.CountByState(TaskState.Unsorted);
        int tickler = store.CountByState(TaskState.Tickler);
        int completed = store.CountByState(TaskState.Completed);

        //print out stats
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Stats:");
        Console.WriteLine($"Total Unsorted: {totalUnsorted}");
        Console.WriteLine($"Total Tickler: {tickler}");
        Console.WriteLine($"Total Completed: {completed}");


    }

   
}