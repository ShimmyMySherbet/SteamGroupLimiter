using System.Linq;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;

namespace SteamGroupLimiter
{
    public class SteamGroupLimiter : RocketPlugin
    {
        public override void LoadPlugin()
        {
            base.LoadPlugin();
            PlayerQuests.onGroupChanged += PlayerQuests_onGroupChanged;
            U.Events.OnPlayerConnected += Events_OnPlayerConnected;
        }

        public byte GetMemberCount(CSteamID ID)
        {
            if (ID == CSteamID.Nil) return 0;
            return (byte)Provider.clients.Where(x => x.player.quests.isMemberOfGroup(ID)).Count();
        }

        private void Events_OnPlayerConnected(UnturnedPlayer player)
        {
            if (Provider.modeConfigData.Gameplay.Max_Group_Members != 0 && GetMemberCount(player.Player.quests.groupID) > Provider.modeConfigData.Gameplay.Max_Group_Members)
            {
                player.Player.quests.leaveGroup(true);
                UnturnedChat.Say(player, $"Your group is past the max members of {Provider.modeConfigData.Gameplay.Max_Group_Members}!");
            }
        }

        private void PlayerQuests_onGroupChanged(PlayerQuests sender, Steamworks.CSteamID oldGroupID, EPlayerGroupRank oldGroupRank, Steamworks.CSteamID newGroupID, EPlayerGroupRank newGroupRank)
        {
            if (Provider.modeConfigData.Gameplay.Max_Group_Members != 0 && GetMemberCount(sender.player.quests.groupID) > Provider.modeConfigData.Gameplay.Max_Group_Members)
            {
                sender.player.quests.leaveGroup(true);
                UnturnedChat.Say(UnturnedPlayer.FromPlayer(sender.player), $"Your group is past the max members of {Provider.modeConfigData.Gameplay.Max_Group_Members}!");
            }
        }
    }
}