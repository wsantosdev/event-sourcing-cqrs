using WSantosDev.EventSourcing.WebApi;
using WSantosDev.EventSourcing.WebApi.Accounts;
using WSantosDev.EventSourcing.WebApi.Exchange;
using WSantosDev.EventSourcing.WebApi.Orders;
using WSantosDev.EventSourcing.WebApi.Positions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMessageBus();
builder.Services.AddEventStore(builder.Configuration);
builder.Services.AddAccountsModule(builder.Configuration);
builder.Services.AddPositionsModule(builder.Configuration);
builder.Services.AddOrdersModule(builder.Configuration);
builder.Services.AddExchangeModule(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddHostedService<DefaultHostedService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => options.DefaultModelsExpandDepth(-1));
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseMessageBus();

app.Run();

public partial class Program { }