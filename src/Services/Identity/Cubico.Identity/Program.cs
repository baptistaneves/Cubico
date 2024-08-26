var builder = WebApplication.CreateBuilder(args);

var jwtSettings = new JwtSettings();

builder.Configuration.Bind(nameof(JwtSettings), jwtSettings);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(nameof(JwtSettings)));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
