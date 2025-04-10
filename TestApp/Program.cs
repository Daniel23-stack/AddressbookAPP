var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/test", () => "This is a test page");
app.MapGet("/register", () => "This would be a registration page");

app.Run();
