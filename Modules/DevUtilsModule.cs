using Discord;
using Discord.Interactions;
using TabletobClubBot.Models.Config;

namespace TabletobClubBot.Modules
{
    [RequireUserPermission(GuildPermission.Administrator)]
    public class DevUtilsModule(IConfiguration configuration, ILogger<DevUtilsModule> logger) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("reset", "Clear all created channels")]
        public async Task ResetServer()
        {
            await DeferAsync();



            List<HQConfig> configs = configuration.GetSection("HQConfigs").Get<List<HQConfig>>();

            foreach (var conf in configs)
            {
                var category = Context.Guild.CategoryChannels.FirstOrDefault(c => c.Name == conf.CategoryName);

                foreach (var chan in conf.Channels)
                {
                    var channel = category.Channels.FirstOrDefault(ch => ch.Name == chan.Name.ToLower());

                    if (channel == null)
                        continue;

                    channel.DeleteAsync();
                }

                await category?.DeleteAsync();
            }

           var message = await FollowupAsync("Reset done");

            await Task.Delay(1000);

            await message.DeleteAsync();
        }

        [SlashCommand("clear-stray", "Clear orphans created channels")]
        public async Task ClearStray()
        {
            await DeferAsync();

            List<HQConfig> configs = configuration.GetSection("HQConfigs").Get<List<HQConfig>>();

            foreach (var conf in configs)
            {
                var category = Context.Guild.CategoryChannels.FirstOrDefault(c => c.Name == conf.CategoryName);

                foreach (var chan in Context.Guild.Channels)
                {
                    if (conf.Channels.Any(c => c.Name.Equals(chan.Name, StringComparison.OrdinalIgnoreCase)))
                        chan.DeleteAsync();
                }
            }

            var message = await FollowupAsync("Stray cleared");

            await Task.Delay(1000);

            await message.DeleteAsync();
        }


        [SlashCommand("init", "Re initialise the server HQs")]
        public async Task InitDiscord()
        {
            await DeferAsync();
            await BotService.Instance.Init();

            var message = await FollowupAsync("Re initiliased");

            await Task.Delay(1000);

            await message.DeleteAsync();
        }
    }
}
