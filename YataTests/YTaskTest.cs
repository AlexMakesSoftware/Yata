namespace YataTests;

using System.Reflection;
using YataModel;
using YataModel.storage;
using Xunit;

public class YTaskTest
{
    IStorage? _store;

    [Fact]
    public void TestTaskStartsAsUnsorted()
    {
        YTask t = new YTask("Write Yata - Yet Another Todo App.");        
        Assert.Equal(TaskState.Unsorted, t.State);
    }
    
        
    // public YTaskTest() {
    //     _store = new FileStoreImpl(Path.GetFullPath("resources/testJobs.json"), new DateTime(2021, 10, 1) );
    //     _store.LoadTodos();
    // }


    [Fact]
    public void TestOverdueJobs()
    {
        _store = new FileStoreImpl(Path.GetFullPath("resources/oneOverDuetest.json"), new DateTime(2022, 12, 6) );
        _store.LoadTodos();
        List<YTask> overdueJobs = _store.OverdueJobs();
        Assert.Equal(2, overdueJobs.Count);
        //Doubt order is guaranteed, so check both start with "Overdue thing".
        Assert.True( overdueJobs[0].Description.StartsWith("Overdue thing") );
        Assert.True( overdueJobs[1].Description.StartsWith("Overdue thing") );
    }

    [Fact]
    public void TestJobsDueToday()
    {
        _store = new FileStoreImpl(Path.GetFullPath("resources/oneDueTodaytest.json"), new DateTime(2022, 12, 6));
        _store.LoadTodos();
        List<YTask> dueTodayJobs = _store.JobsDueToday();
        Assert.Equal(1, dueTodayJobs.Count);
        Assert.Equal("Write Yata - Yet Another Todo App.", dueTodayJobs[0].Description);

    }

    

    

}