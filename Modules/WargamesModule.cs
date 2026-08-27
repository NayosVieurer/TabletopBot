using Discord.Interactions;

namespace TabletobClubBot.Modules
{
    public class WargamesModule(ILogger<WargamesModule> logger) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("create-base", "Create the base HQ for Wargames related stuff")]
        public async Task CreateBase()
        {
            await DeferAsync(true);

            var test = Context.Guild.Channels.FirstOrDefault(c => c.Name == "Wargames HQ");

            var category = await Context.Guild.CreateCategoryChannelAsync("Wargames HQ");
        }
    }
}
