using Discord.Interactions;

namespace TabletobClubBot.Modules
{
    public class PingModule(ILogger<PingModule> logger) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Just replies Pong")]
        public async Task Ping()
        {
            await RespondAsync("Envoi en cours...", ephemeral: true);

            await Context.Channel.SendMessageAsync("pong");
        }
    }
}
