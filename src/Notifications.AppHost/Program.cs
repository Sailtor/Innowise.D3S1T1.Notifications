using Notifications.Application;
using Notifications.Infrastructure;
using Notifications.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPresentation(builder.Configuration);

var app = builder.Build();

app.Run();
