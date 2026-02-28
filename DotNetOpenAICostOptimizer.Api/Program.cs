using DotNetOpenAICostOptimizer.Models;
using DotNetOpenAICostOptimizer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using OpenAI.Chat;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<OpenAiSettings>(builder.Configuration.GetSection("OpenAiSettings"));
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// Mock Service Implemented
builder.Services.AddScoped<IOpenAiService>(provider => 
{
    var realService = new OpenAiMockService();
    var cache = provider.GetRequiredService<IDistributedCache>();
    return new CachedOpenAiService(realService, cache);
});

// Real Service Implemented
// builder.Services.AddSingleton<ChatClient>(provider =>
// {
//     var settings = provider.GetRequiredService<IOptions<OpenAiSettings>>();
//     return new ChatClient(model: settings.Value.Model, apiKey: settings.Value.ApiKey);
// });
//
// builder.Services.AddScoped<IOpenAiService>(provider => 
// {
//     var openaiClient = provider.GetRequiredService<ChatClient>();
//     var realService = new OpenAiService(openaiClient);
//     var cache = provider.GetRequiredService<IDistributedCache>();
//     return new CachedOpenAiService(realService, cache);
// });


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/api/ai/ask",
        async ([FromBody] PromptRequest request, IOpenAiService aiService, CancellationToken cancellationToken) =>
        {
            if (request is null || string.IsNullOrEmpty(request.Prompt))
                return Results.BadRequest();

            try
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var response = await aiService.AskAsync(request.Prompt, cancellationToken);
                watch.Stop();

                return Results.Ok(new
                {
                    Result = response,
                    ElapsedTime = watch.ElapsedMilliseconds
                });
            }
            catch (OperationCanceledException)
            {
                return Results.StatusCode(499); // Client Closed Request
            }
            catch (Exception)
            {
                return Results.Problem("The AI service is currently unavailable. Please try again later.");
            }
        })
    .WithName("AskAi")
    .WithSummary("OpenAI'dan yanıt alır (Redis Cache destekli)")
    .WithDescription("Eğer prompt daha önce sorulmuşsa Redis üzerinden milisaniyeler içinde yanıt döner.")
    .Produces(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status400BadRequest);
    
app.Run();