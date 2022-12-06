namespace YataModel;
public class YTask
{
    public YTask(string description)
    {
        this.Description = description;
        this.State = TaskState.Unsorted;
    }

    public string Description { get; set; }

    public DateTime Due { get; set; }

    public DateTime? Completed { get; set; }

    public TaskState State { get; set; }

    
}
