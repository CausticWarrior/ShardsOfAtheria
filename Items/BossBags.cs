﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SagesMania.Items.Weapons;
using SagesMania.Items.Accessories;

namespace SagesMania.Items
{
    public class BossBags : GlobalItem
	{
		public override void OpenVanillaBag(string context, Player player, int arg)
		{
			if (context == "bossBag" && arg == ItemID.EyeOfCthulhuBossBag)
			{
				player.QuickSpawnItem(ModContent.ItemType<Cataracnia>());
				player.QuickSpawnItem(ModContent.ItemType<EyeOfTheAllSeer>());
			}
			if (context == "bossBag" && arg == ItemID.EaterOfWorldsBossBag)
			{
				player.QuickSpawnItem(ModContent.ItemType<OversizedWormsTooth>());
			}
			if (context == "bossBag" && arg == ItemID.BrainOfCthulhuBossBag)
			{
				player.QuickSpawnItem(ModContent.ItemType<TomeOfOmniscience>());
			}
			if (context == "bossBag" && arg == ItemID.QueenBeeBossBag)
			{
				if (Main.rand.NextFloat() < 0.5f)
				{
					player.QuickSpawnItem(ModContent.ItemType<HecateII>());
					player.QuickSpawnItem(ModContent.ItemType<DemonClaw>());
				}
				else
				{
					player.QuickSpawnItem(ModContent.ItemType<LCAR9>());
					player.QuickSpawnItem(ModContent.ItemType<HiddenBlade>());
				}
				if (Main.rand.NextFloat() < .1f)
                {
					if (Main.rand.NextFloat() < .5f)
						player.QuickSpawnItem(ModContent.ItemType<MarkOfAnastasia>());
					else player.QuickSpawnItem(ModContent.ItemType<ShadowBrand>());
				}
			}
		}
	}
}