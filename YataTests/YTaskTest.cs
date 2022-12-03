namespace YataTests;
using YataModel;

public class YTaskTest
{
    [Fact]
    public void TestTaskStartsAsUnsorted()
    {
        YTask t = new YTask("Write Yata - Yet Another Todo App.");        
        Assert.Equal(TaskState.Unsorted, t.State);
    }

    [Fact]
    public void TestLoadTasks() {
        List<YTask> loaded = TaskListLoader.LoadTasks(".\resources\testJobs.json");
        Assert.Equal(3,loaded.Count);
    }

    [Fact]
    public void TestSaveTasks() {
        List<YTask> todo = new List<YTask>();
        todo.Add(new YTask("one"));
        todo.Add(new YTask("two"));
        todo.Add(new YTask("three"));
        TaskListLoader.SaveTasks(todo);
    }
}