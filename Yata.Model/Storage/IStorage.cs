using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YataModel.Storage
{
    public interface IStorage
    {
        public void LoadTodos();

        public void SaveTodos();

        public List<YTask> DueJobs();

        public List<YTask> JobsDueToday();

        public int CountByState(TaskState state);

        public int AddTask(string Description);

        public void SetTaskDue(int id, DateTime due);

        public void SetTaskState(int id, TaskState state);

        public List<YTask> GetAllInState(TaskState state);
        
        public YTask? GetTask(int id);
    }
}