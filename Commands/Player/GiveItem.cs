﻿using Terraria;
using Terraria.ModLoader;

namespace CheatCommands.Commands.Player {
    class GiveItem : CheatCommand {
        public override string CommandName => "Give Item";
        public override string Command => "give";
        public override string Description => "Give yourself an item.";
        public override string Usage => base.Usage + " <type/name> [amount]";
        public override int MinimumArguments => 1;
        public override CommandType Type => CommandType.Chat;

        public override CommandReply Action(CommandCaller caller, string[] args) {
            int itemType = 0;
            int maxStack = 99;
            int amount = 1;

            if(!int.TryParse(args[0], out itemType)) {
                itemType = CommandUtils.GetItemType(args[0]);
            }

            if(itemType == 0 || itemType >= ItemLoader.ItemCount) {
                throw new UsageException("Unknown item type: " + itemType);
            }
            else {
                Item item = new Item();
                item.SetDefaults(itemType);
                maxStack = item.maxStack;
            }

            if(args.Length > 1) {
                if(!int.TryParse(args[1], out amount)) {
                    amount = 1;
                }
            }

            int adjustedAmount = amount;

            while(adjustedAmount > 0) {
                int spawnAmount = (adjustedAmount > maxStack ? maxStack : adjustedAmount);

                caller.Player.QuickSpawnItem(itemType, spawnAmount);
                adjustedAmount -= maxStack;
            }
            
            return new CommandReply("Gave you " + amount + " of item type " + itemType + "!");
        }
    }
}
