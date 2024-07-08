using CM.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureDefaults();

var app = builder.Build();

await app.ConfigureDefaults();

app.UseAuthorization();

app.MapControllers();

app.Run();
