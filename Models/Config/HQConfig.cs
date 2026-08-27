using Discord;

namespace TabletobClubBot.Models.Config
{

    public class HQConfig
    {
        public int ID { get; set; }
        public string CategoryName { get; set; }

        public List<HQChannel> Channels { get; set; } = new List<HQChannel>();

        public ulong AccessRoleId { get; set; }

        public bool IsCommon { get; set; }
    }

    public class HQChannel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public ChannelType ChannelType { get; set; }

        public bool BotOnly { get; set; }
    }
}
