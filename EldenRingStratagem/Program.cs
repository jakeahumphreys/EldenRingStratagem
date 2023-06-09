﻿using EldenRingStratagem.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IBossSearchService, BossSearchSearchService>();

var app = builder.Build();

app.MapControllers();
app.Run();




