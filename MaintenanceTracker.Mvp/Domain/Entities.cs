namespace MaintenanceTracker.Mvp.Domain;


public enum AssetStatus { Active, Inactive, Retired }
public enum WorkOrderStatus { New, InProgress, QA, Closed }
public enum Priority { Low, Medium, High }


public sealed class Asset
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string AssetType { get; set; } = string.Empty;
    public AssetStatus Status { get; set; } = AssetStatus.Active;
    public string Location { get; set; } = string.Empty;


    public List<WorkOrder> WorkOrders { get; set; } = new();
}


public sealed class WorkOrder
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AssetId { get; set; }
    public Asset? Asset { get; set; }


    public string Title { get; set; } = string.Empty;
    public Priority Priority { get; set; } = Priority.Medium;
    public WorkOrderStatus Status { get; set; } = WorkOrderStatus.New;
    public DateOnly? ScheduledDate { get; set; }
    public DateOnly? CompletedDate { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}


public sealed record CreateWorkOrderDto(Guid AssetId, string Title, Priority Priority, DateOnly? ScheduledDate);
public sealed record UpdateStatusDto(WorkOrderStatus NewStatus);
