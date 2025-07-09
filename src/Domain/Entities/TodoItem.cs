using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Todo_App.Domain.Entities;

public class TodoItem : BaseAuditableEntity
{
    public int ListId { get; set; }

    public string? Title { get; set; }

    public string? Note { get; set; }
    
    [NotMapped]
    public List<string>? Tags { get; set; }

    public string Tag
    {
        get => JsonSerializer.Serialize(Tags);
        set => Tags = string.IsNullOrWhiteSpace(value)
            ? new List<string>()
            : JsonSerializer.Deserialize<List<string>>(value)!;
    }

    public PriorityLevel Priority { get; set; }

    public DateTime? Reminder { get; set; }

    private bool _done;
    public bool Done
    {
        get => _done;
        set
        {
            if (value == true && _done == false)
            {
                AddDomainEvent(new TodoItemCompletedEvent(this));
            }

            _done = value;
        }
    }

    public TodoList List { get; set; } = null!;
}
