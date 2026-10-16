using Microsoft.Extensions.Options;
using ActivityRegistration.Api.Services;
using ActivityRegistration.Api.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ActivityService>();

builder.Services.AddProblemDetails();

builder.Services.AddOptions<ApplicationOptions>()
    .Bind(builder.Configuration.GetSection("Application"))
    .Validate(
        options => !string.IsNullOrWhiteSpace(options.DisplayName),
        "Application:DisplayName 不可空白")
    .ValidateOnStart();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Use(async (context, next) =>
{
    var traceId = System.Diagnostics.Activity.Current?.Id
        ?? context.TraceIdentifier;

    app.Logger.LogInformation(
        "開始請求 {Method} {Path} TraceId={TraceId}",
        context.Request.Method,
        context.Request.Path,
        traceId);

    await next(context);

    app.Logger.LogInformation(
        "結束請求 {StatusCode} TraceId={TraceId}",
        context.Response.StatusCode,
        traceId);
});

var applicationOptions = app.Services
    .GetRequiredService<IOptions<ApplicationOptions>>()
    .Value;

app.Logger.LogInformation(
    "目前環境 {Environment}，系統名稱 {DisplayName}",
    app.Environment.EnvironmentName,
    applicationOptions.DisplayName
);

app.UseExceptionHandler();

app.UseAuthorization();

app.MapControllers();

app.Run();
