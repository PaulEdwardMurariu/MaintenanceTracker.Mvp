using MaintenanceTracker.Mvp;
using MaintenanceTracker.Mvp.Components;
using MaintenanceTracker.Mvp.Data;
using MaintenanceTracker.Mvp.Domain;
using Microsoft.EntityFrameworkCore;

// Random comment

var builder = WebApplication.CreateBuilder(args);

// Blazor Web App (.NET 8) hosting model
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// SQLite DB
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=app.db"));

var app = builder.Build();

// Ensure DB + seed demo data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    if (!db.Assets.Any())
    {
        var a1 = new Asset { Name = "Generator A", SerialNumber = "GEN-001", AssetType = "Power", Status = AssetStatus.Active, Location = "Bay 1" };
        var a2 = new Asset { Name = "Vehicle 12", SerialNumber = "VEH-012", AssetType = "Vehicle", Status = AssetStatus.Active, Location = "Lot" };
        db.Assets.AddRange(a1, a2);
        db.WorkOrders.AddRange(
            new WorkOrder { Asset = a1, Title = "Oil change", Priority = Priority.Medium, Status = WorkOrderStatus.New, ScheduledDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2)) },
            new WorkOrder { Asset = a2, Title = "Brake inspection", Priority = Priority.High, Status = WorkOrderStatus.InProgress, ScheduledDate = DateOnly.FromDateTime(DateTime.Today) }
        );
        db.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// ----- Minimal API endpoints -----
var api = app.MapGroup("/api/workorders");

api.MapGet("/", async (AppDbContext db) =>
{
    var items = await db.WorkOrders
        .Include(w => w.Asset)
        .OrderByDescending(w => w.CreatedUtc)
        .Select(w => new
        {
            w.Id,
            w.Title,
            w.Status,
            w.Priority,
            w.ScheduledDate,
            AssetName = w.Asset!.Name
        })
        .ToListAsync();
    return Results.Ok(items);
});

api.MapPost("/", async (CreateWorkOrderDto dto, AppDbContext db) =>
{
    var asset = await db.Assets.FindAsync(dto.AssetId);
    if (asset is null) return Results.NotFound(new { message = "Asset not found" });

    var wo = new WorkOrder
    {
        AssetId = asset.Id,
        Title = dto.Title,
        Priority = dto.Priority,
        Status = WorkOrderStatus.New,
        ScheduledDate = dto.ScheduledDate
    };
    db.WorkOrders.Add(wo);
    await db.SaveChangesAsync();
    return Results.Created($"/api/workorders/{wo.Id}", new { wo.Id });
});

api.MapPut("/{id:guid}/status", async (Guid id, UpdateStatusDto dto, AppDbContext db) =>
{
    var wo = await db.WorkOrders.FindAsync(id);
    if (wo is null) return Results.NotFound();

    bool valid = (wo.Status, dto.NewStatus) switch
    {
        (WorkOrderStatus.New, WorkOrderStatus.InProgress) => true,
        (WorkOrderStatus.InProgress, WorkOrderStatus.QA) => true,
        (WorkOrderStatus.QA, WorkOrderStatus.Closed) => true,
        _ => false
    };
    if (!valid) return Results.BadRequest(new { message = "Invalid status transition" });

    wo.Status = dto.NewStatus;
    if (wo.Status == WorkOrderStatus.Closed)
        wo.CompletedDate = DateOnly.FromDateTime(DateTime.UtcNow);

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Map the Blazor app (replaces MapBlazorHub/MapFallbackToPage)
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode();

app.Run();

/*using MaintenanceTracker.Mvp.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();*/
