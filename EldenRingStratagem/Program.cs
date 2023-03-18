using EldenRingStratagem.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IBossService, BossService>();

var app = builder.Build();

// app.UseHttpsRedirection();
app.MapControllers();
app.Run();




