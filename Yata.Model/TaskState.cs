using System.Text.Json.Serialization;

namespace YataModel
{
     [JsonConverter(typeof(JsonStringEnumConverter))]
     public enum TaskState
    {
        Unsorted,
        Scheduled,
        InProgress,
        Tickler,
        Completed
    }
}