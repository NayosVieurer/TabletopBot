namespace TabletobClubBot.Models.Config
{
    public class DiscordConfig
    {
        public string Token { get; set; }

        public ulong GuildId { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(Token) && GuildId != 0;
        }
    }
}
