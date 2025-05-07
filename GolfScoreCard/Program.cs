using GolfScoreCard.APISTUFF;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<FetchFromAPI>();

var app = builder.Build();



if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.MapGet("/api/golfcourses", async (string query, FetchFromAPI fetcher) =>
{
    if (string.IsNullOrWhiteSpace(query))
        return Results.BadRequest("Missing query");

    var json = await fetcher.SearchCoursesAsync(query);
    return Results.Content(json, "application/json");
});

app.Run();