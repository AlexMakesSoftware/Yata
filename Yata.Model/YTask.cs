using System.Text.Json.Serialization;

namespace YataModel;
public class YTask
{
    //default constructor for serialisation.
    public YTask()
    {
        this.Description = "";
        this.State = TaskState.Unsorted;
        this.Due = DateTime.Today;
    }

    public YTask(string description)
    {
        this.Description = description;
        this.State = TaskState.Unsorted;
        this.Due = DateTime.Today;
    }

    public YTask(string description, DateTime due)
    {
        this.Description = description;
        this.State = TaskState.Scheduled;
        this.Due = due;
    }    

    public string Description { get; set; }

    //Q:How do I make JSON serialise this as a date and not a datetime?
    //TODO: Use the [JsonConverter] attribute ?
    
     public DateTime Due { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] //Make this field blank in the json when it's serialised if it's null
    public DateTime? Completed { get; set; }

    public TaskState State { get; set; }

    //Override the ToString method to print out the task in a nice format
    public override string ToString()
    {
        string output = "";
        output += this.Description;
        output += " (";
        output += this.State.ToString();
        output += ") ";
        output += this.Due.ToString("yyyy-MM-dd");
        if (this.Completed.HasValue)
        {
            output += " Completed: ";
            output += this.Completed.Value.ToString("yyyy-MM-dd");
        }
        return output;
    }
}
