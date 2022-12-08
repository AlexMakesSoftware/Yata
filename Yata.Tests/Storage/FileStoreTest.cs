namespace YataTests;

using System.Reflection;
using YataModel;
using YataModel.Storage;
using Xunit;

public class FileStoreTest
{
    IStorage? _store;

    [Fact]
    public void TestTaskStartsAsUnsorted()
    {
        YTask t = new YTask("Write Yata - Yet Another Todo App.");        
        Assert.Equal(TaskState.Unsorted, t.State);
    }

    [Fact]
    public void TestOverdueJobs()
    {
        _store = new FileStoreImpl(Path.GetFullPath("resources/oneOverDuetest.json"), new DateTime(2022, 12, 6) );
        _store.LoadTodos();
        List<YTask> overdueJobs = _store.DueJobs();
        Assert.Equal(2, overdueJobs.Count);
        //Doubt order is guaranteed, so check both start with "Overdue thing".
        Assert.StartsWith( "Overdue thing", overdueJobs[0].Description );
        Assert.StartsWith( "Overdue thing", overdueJobs[1].Description );
    }

    [Fact]
    public void TestJobsDueToday()
    {
        _store = new FileStoreImpl(Path.GetFullPath("resources/oneDueTodaytest.json"), new DateTime(2022, 12, 6));
        _store.LoadTodos();
        List<YTask> dueTodayJobs = _store.JobsDueToday();

        Assert.Single(dueTodayJobs);
        Assert.Equal("Write Yata - Yet Another Todo App.", dueTodayJobs[0].Description);

    }

}