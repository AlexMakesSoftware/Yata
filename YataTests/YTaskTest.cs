namespace YataTests;
using YataModel;

public class YTaskTest
{
    [Fact]
    public void TestTaskStartsAsUnsorted()
    {
        YTask t = new YTask("Write Yata - Yet Another Todo App.");        
        Assert.Equal(t.State, TaskState.Unsorted);
    }
}