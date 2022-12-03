using System;
using System.Collections.Generic;

using System.Text.Json;

namespace YataModel
{
    public class TaskListLoader
    {
         public static List<YTask> LoadTasks(string path) {
            List<YTask> taskList = new List<YTask>();


            return taskList;
        }

        public static void SaveTasks(List<YTask> taskList) {
            string jsonString = JsonSerializer.Serialize(taskList);
            Console.WriteLine(jsonString);
            File.WriteAllText("test.json", jsonString);
        }
    }
}