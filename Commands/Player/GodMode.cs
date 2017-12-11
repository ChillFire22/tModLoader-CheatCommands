﻿using Terraria.ModLoader;

namespace CheatCommands.Commands.Player {
    class GodMode : CheatCommand {
        public static bool Enabled { get; set; }

        public override string CommandName => "God Mode";
        public override string Command => "god";
        public override string Description => "Enable/disable god mode.";
        public override int MinimumArguments => 0;
        public override CommandType Type => CommandType.Chat;

        public override CommandReply Action(CommandCaller caller, string[] args) {
            CheatCommandsPlayer player = caller.Player.GetModPlayer<CheatCommandsPlayer>();
            Enabled = !Enabled;

            if(Enabled) {
                player.RefillLife();
                player.RefillMana(true);
                player.RemoveDebuffs();
            }

            return new CommandReply("God mode " + (Enabled ? "enabled" : "disabled") + "!");
        }
    }
}