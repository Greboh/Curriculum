using Curriculum.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureApi();

var services = builder.Services;
services.ConfigureApi(builder.Configuration);

var app = builder.Build();
app.ConfigureApi(app.Configuration);

app.Run();