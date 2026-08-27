using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using TabletobClubBot;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("discordsettings.json", optional:false, reloadOnChange:true)
    .AddJsonFile("discordsettings.Development.json", optional:true, reloadOnChange:true);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//require an hybrid registration. The Factory will be used by the bot service, whilst the DBContext itself will be used by ASP 
builder.Services.AddDbContextFactory<TCDBContext>(options =>
options.UseNpgsql(builder.Configuration.GetSection("ConnectionString").Get<string>())).
AddScoped(sp => sp.GetRequiredService<IDbContextFactory<TCDBContext>>().CreateDbContext());

builder.Services.AddSingleton<DiscordSocketClient>();
builder.Services.AddSingleton<InteractionService>(x => new(x.GetRequiredService<DiscordSocketClient>())); 

builder.Services.AddHostedService<BotService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
