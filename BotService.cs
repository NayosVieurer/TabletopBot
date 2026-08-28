
using Discord;
using Discord.Interactions;
using Discord.Rest;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TabletobClubBot.Models.Config;
using DiscordConfig = TabletobClubBot.Models.Config.DiscordConfig;

namespace TabletobClubBot
{
    public class BotService(DiscordSocketClient client, IDbContextFactory<TCDBContext> dbContextFactory, InteractionService interactionService, IServiceProvider services, IConfiguration configuration, ILogger<BotService> logger) : BackgroundService
    {
        public static BotService Instance { get; private set; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (Instance != this)
                Instance = this;

            client.Log += LogDiscordMessage;
            interactionService.Log += LogDiscordMessage;

            // Événement déclenché lorsque le bot est connecté et prêt
            client.Ready += OnBotReady;
            // Événement déclenché à chaque fois qu'un utilisateur utilise une commande Slash
            client.InteractionCreated += OnInteractionCreated;

            DiscordConfig config = configuration.GetSection("Discord").Get<DiscordConfig>();

            if (string.IsNullOrEmpty(config.Token)) return;

            await client.LoginAsync(TokenType.Bot, config.Token);
            await client.StartAsync();

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task OnBotReady()
        {
            //⚠ Trying to retrieve the Guild ID from the settings file. Make sure it is valid
            var discordSettings = configuration.GetSection("Discord").Get<DiscordConfig>();

            if(discordSettings?.IsValid() != true)
            {
                logger.LogError("DiscordSettings.json is not correctly set");
                return;
            }

            // Load all the command modules classes 
            await interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), services);

            //Forcing the guildID to avoid running the same bot for multiple servers
            await interactionService.RegisterCommandsToGuildAsync(discordSettings.GuildId); //  Register the bot to the guild ID

            logger.LogInformation("Commandes Slash enregistrées avec succès.");

            await Init();
        }

        public async Task Init()
        {

            using var dbContext = await dbContextFactory.CreateDbContextAsync();

            var hasConfigs = await dbContext.hqConfigs.AnyAsync();

            if (!hasConfigs)
            {
                var jsonConfigs = configuration.GetSection("HQConfigs").Get<List<HQConfig>>();

                await dbContext.hqConfigs.AddRangeAsync(jsonConfigs);
                await dbContext.SaveChangesAsync();
            }

            var configs = dbContext.hqConfigs.Include(hq => hq.Channels);

            foreach (var config in configs)
            {
                await InitHQ(config);
            }
        }

        private async Task OnInteractionCreated(SocketInteraction interaction)
        {
            var context = new SocketInteractionContext(client, interaction);
            await interactionService.ExecuteCommandAsync(context, services);
        }

        private Task LogDiscordMessage(LogMessage message)
        {
            logger.LogInformation("Discord: {Message}", message.Message);
            return Task.CompletedTask;
        }

        private async Task InitHQ(HQConfig config)
        {
            var guild = client.Guilds.FirstOrDefault();

            ICategoryChannel category = guild.CategoryChannels.FirstOrDefault(c => c.Name == config.CategoryName);

            if(category == null)
            {
                category = await guild.CreateCategoryChannelAsync(config.CategoryName);

                if (!config.IsCommon)
                {                
                    var everyoneRole = guild.EveryoneRole;
                    var accessRole = guild.GetRole(config.AccessRoleId);

                    var denyEveryonePerm = new OverwritePermissions(viewChannel: PermValue.Deny);
                    var privatePerm = new OverwritePermissions(viewChannel: PermValue.Allow);

                    await category.AddPermissionOverwriteAsync(everyoneRole, denyEveryonePerm);
                    await category.AddPermissionOverwriteAsync(accessRole, privatePerm);
                }
            }

            foreach(var chan in config.Channels)
            {
                var channel = guild.TextChannels.FirstOrDefault(c => c.CategoryId == category.Id && c.Name.Equals(chan.Name, StringComparison.OrdinalIgnoreCase)) as ITextChannel;

                if(channel == null)
                {
                    TextChannelProperties props = new();

                    props.CategoryId = category.Id;

                    channel = await guild.CreateTextChannelAsync(chan.Name, props =>
                    {
                        props.CategoryId = category.Id;
                    });
                }

                if(chan.BotOnly)
                {
                //    var messages =  channel.GetMessagesAsync();

                //    var count = await messages.CountAsync();

                //    var message = await messages.FirstOrDefaultAsync();


                //    if (message.Count == 0)
                //    {
                //        var button = new ButtonBuilder()
                //            .WithLabel("Créer un JDR")
                //            .WithCustomId("btn_open_modal") // ID unique pour intercepter le clic
                //            .WithStyle(ButtonStyle.Primary) // Couleur bleue
                //            .WithEmote(new Emoji("➕"));

                //        var components = new ComponentBuilder().WithButton(button).Build();

                //        await channel.SendMessageAsync("Cliquez sur le bouton ci-dessous pour ajouter un jeu :", components: components);

                   // }
                }
            }
        }
    }
}
