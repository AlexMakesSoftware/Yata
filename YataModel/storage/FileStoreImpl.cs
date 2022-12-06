using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YataModel.storage
{
    public class FileStoreImpl : IStorage
    {
        string? _envFile;
        private List<YTask> _tasks = new List<YTask>();
        private DateTime? _fixDateForToday = null;


        //constructor
        public FileStoreImpl()
        {
            _envFile = Environment.GetEnvironmentVariable("YATA_FILE");
            if (_envFile == null)
            {
                _envFile = "Jobs.json";
            }            
        }

        //constructor for testing that takes a file path
        public FileStoreImpl(string filePath, DateTime fixDateForToday)
        {
            _envFile = filePath;
            _fixDateForToday = fixDateForToday;
        }

        public void LoadTodos()
        {
            if(_envFile==null) throw new Exception("No file specified");
            string contents = System.IO.File.ReadAllText(_envFile);
            List<YTask>? tasks = System.Text.Json.JsonSerializer.Deserialize<List<YTask>>(contents);
            if(tasks==null) tasks = new List<YTask>();
            _tasks = tasks;
        }

        public void SaveTodos()
        {
            if(_envFile==null) throw new Exception("No file specified");

            string contents = System.Text.Json.JsonSerializer.Serialize(_tasks);

            System.IO.File.WriteAllText(_envFile, contents);
        }      

        public List<YTask> OverdueJobs()
        {
            //get all the jobs that are overdue in a list
            List<YTask> overdueJobs = new List<YTask>();
            foreach (YTask task in _tasks)
            {
                if (task.State == TaskState.Scheduled && task.Due.Date < TodaysDate())
                {
                    overdueJobs.Add(task);
                }
            }
            return overdueJobs;
        }

        public List<YTask> JobsDueToday()
        {
            //get all the jobs scheduled for today in a list
            List<YTask> todaysJobs = new List<YTask>();
            foreach (YTask task in _tasks)
            {
                if (task.State == TaskState.Scheduled && task.Due.Date == TodaysDate())
                {
                    todaysJobs.Add(task);
                }
            }
            return todaysJobs;
        }

        private DateTime TodaysDate()
        {
            if(_fixDateForToday==null)
            {
                return DateTime.Today;
            } else {
                return _fixDateForToday.Value;
            }
        }

        public int CountByState(TaskState state)
        {
            int count = 0;
            foreach (YTask task in _tasks)
            {
                if (task.State == state)
                {
                    count++;
                }
            }

            return count;
        }

        public void AddTask(YTask task)
        {
            _tasks.Add(task);
        }

        public void SaveTask(YTask task)
        {
            //With an in-memory store, this is a no-op.
        }

        public List<YTask> GetAllInState(TaskState state)
        {
            //get all the jobs in a given state
            List<YTask> jobs = new List<YTask>();
            foreach (YTask task in _tasks)
            {
                if (task.State == state)
                {
                    jobs.Add(task);
                }
            }
            return jobs;
        }
    }    
}