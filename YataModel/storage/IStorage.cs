using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YataModel.storage
{
    public interface IStorage
    {
        public void LoadTodos();

        public void SaveTodos();

        public List<YTask> OverdueJobs();

        public List<YTask> JobsDueToday();

        public int CountByState(TaskState state);

        public void AddTask(YTask task);

        public void SaveTask(YTask task);

        public List<YTask> GetAllInState(TaskState state);
        
    }
}