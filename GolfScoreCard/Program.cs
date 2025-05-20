using GolfScoreCard;
using GolfScoreCard.APISTUFF;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1) Register your controllers so /api/CourseDataOutput and friends will work
builder.Services.AddControllers();

// 2) Razor Pages + HttpClient + DbContext
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<FetchFromAPI>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var connectionString = "Server=tcp:parpalserver.database.windows.net,1433;Initial Catalog=parpaldb;Persist Security Info=False;User ID=jsanderswp;Password=vckzKL#k;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ─── Note: no UseAuthorization() here ───

// 3) Map attribute-based API controllers
app.MapControllers();

// 4) Map your Razor Pages
app.MapRazorPages();

// 5) Keep your minimal /api/golfcourses endpoint too
app.MapGet("/api/golfcourses", async (string query, FetchFromAPI fetcher) =>
{
    if (string.IsNullOrWhiteSpace(query))
        return Results.BadRequest("Missing query");

    var json = await fetcher.SearchCoursesAsync(query);
    return Results.Content(json, "application/json");
});

app.Run();