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

        public void AddTask(YTask task);

        public List<YTask> GetAllInState(TaskState state);
        
    }
}