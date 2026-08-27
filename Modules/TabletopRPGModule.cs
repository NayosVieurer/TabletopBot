using Discord;
using Discord.Interactions;

namespace TabletobClubBot.Modules
{
    public class TabletopRPGModule(ILogger<TabletopRPGModule> logger) : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("create-party", "Create a new party for a tabletop RPG")]
        public async Task CreateParty()
        {
            await DeferAsync(true);

            var rpgMenu = new SelectMenuBuilder()
                .WithCustomId("rpg_menu")
                .WithPlaceholder("Select a tabletop RPG")
                .AddOption("Dungeons & Dragons", "dnd")
                .AddOption("Pathfinder", "pathfinder")
                .AddOption("Call of Cthulhu", "call_of_cthulhu")
                .AddOption("Add RPG","new");

            var components = new ComponentBuilder().WithSelectMenu(rpgMenu).Build();

            await FollowupAsync("Please select a tabletop RPG to create a party for:", components: components, ephemeral: true);
        }

        [ComponentInteraction("rpg_menu")]
        public async Task HandleRPGSelection(string rpg)
        {
            if(rpg == "new")
            {
                await RespondWithModalAsync<CustomRpgModal>("custom_rpg");
                return;
            }
            await DeferAsync(true);

            await FollowupAsync($"You have selected {rpg}. Now, please provide the party name and description.", ephemeral: true);

        }

        // L'ID doit correspondre à celui fourni dans RespondWithModalAsyncs
        [ModalInteraction("custom_rpg")]
        public async Task HandleModalResponseAsync(CustomRpgModal modal)
        {
            // Le framework remplit automatiquement la propriété RpgName avec la saisie de l'utilisateur
            string newRpgName = modal.RpgName;

            // Ici, vous pouvez ajouter ce JDR à votre base de données si nécessaire

            await RespondAsync($"✨ Nouveau JDR créé et sélectionné : **{newRpgName}** !", ephemeral: true);
        }
    }

    public class CustomRpgModal : IModal
    {
        public string Title => "Create new Party";

        [InputLabel("Nom du jeu de rôle")]
        [ModalTextInput("rpg_name_input", TextInputStyle.Short, placeholder: "Ex: Cyberpunk RED, Starfinder...", minLength: 2, maxLength: 50)]
        public string RpgName { get; set; }
    }
}
