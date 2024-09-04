using OracaoApp_API.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.AddCustomServices();

var app = builder.Build();

app.UseCustomServices();

app.Run();
