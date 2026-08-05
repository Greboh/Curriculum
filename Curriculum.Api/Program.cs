using Curriculum.Api.Extensions;
using Curriculum.Infrastructure.Configurations;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

builder.AddServiceDefaults();

services.ConfigureApi(builder.Configuration);

var app = builder.Build();

app.ConfigureApi(app.Configuration);

app.Run();