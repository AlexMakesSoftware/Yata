using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YataModel.Storage
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
            
            //check file exists
            if (!System.IO.File.Exists(_envFile))
            {
                _tasks = new List<YTask>();
                return;
            }

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

        public List<YTask> DueJobs()
        {
            //get all the jobs that are overdue in a list
            List<YTask> overdueJobs = new List<YTask>();
            foreach (YTask task in _tasks)
            {
                if (task.State == TaskState.Scheduled && task.Due.Date <= TodaysDate())
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

            todaysJobs.Sort((x, y) => x.Due.CompareTo(y.Due));

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

        public int AddTask(string description)
        {    
            YTask task = new YTask(description);            
            _tasks.Add(task);
            return task.Id;
        }
        
        public void SetTaskDue(int id, DateTime due)
        {
            foreach (YTask task in _tasks)
            {
                if (task.Id == id)
                {
                    task.Due = due;
                    task.State = TaskState.Scheduled;
                    return;
                }
            }
            throw new Exception("Task with id "+id+" not found");
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

        public void SetTaskState(int id, TaskState state)
        {
            foreach (YTask task in _tasks)
            {
                if (task.Id == id)
                {
                    task.State = state;
                    return;
                }
            }
            throw new Exception("Task with id "+id+" not found");
        }

        public YTask? GetTask(int id)
        {
            foreach (YTask task in _tasks)
            {
                if (task.Id == id)
                {
                    return task;
                }
            }
            return null;
        }
    }    
}