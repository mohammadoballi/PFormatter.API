using PFormatter.API.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

var app = builder.Build();

app.MapPost("/ai/ask", async (AiRequest request, IConfiguration config, HttpClient http) =>
{
    if (string.IsNullOrWhiteSpace(request.Prompt))
        return Results.BadRequest(new { error = "Prompt is required" });

    var n8nUrl = config["N8N:Url"];
    if (string.IsNullOrWhiteSpace(n8nUrl))
        return Results.Problem("N8N URL is missing");

    var response = await http.PostAsJsonAsync(n8nUrl, request);

    if (!response.IsSuccessStatusCode)
        return Results.Problem("N8N request failed");

    var replyText = await response.Content.ReadAsStringAsync();
    var raw = await response.Content.ReadFromJsonAsync<GeminiResponse>();

    //var replyText = raw?.Replay.Message ?? "No response";

    return Results.Ok(new
    {
        reply = raw.Replay
    });
});

app.Run();

