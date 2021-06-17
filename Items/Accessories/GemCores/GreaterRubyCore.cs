﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SagesMania.Items.Accessories.GemCores
{
	public class GreaterRubyCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("10% increased damage\n" +
				"Attacks inflict 'On Fire'");
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.value = Item.sellPrice(silver: 15);
			item.rare = ItemRarityID.White;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.allDamage += .1f;
			player.GetModPlayer<SMPlayer>().greaterRubyCore = true;
		}
	}
}