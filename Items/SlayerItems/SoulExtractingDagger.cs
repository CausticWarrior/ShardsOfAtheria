﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ShardsOfAtheria.Projectiles.Tools;
using System.Collections.Generic;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using ShardsOfAtheria.Players;
using ShardsOfAtheria.Items.SlayerItems.SoulCrystals;
using ShardsOfAtheria.Globals;
using static ShardsOfAtheria.Items.SlayerItems.Entry;
using System.Configuration;

namespace ShardsOfAtheria.Items.SlayerItems
{
    public class SoulExtractingDagger : ModItem
    {
        public int selectedSoul = 0;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Right click to cycle between absorbed Soul Crystals\n" +
                "Use to extract the currently selected Soul Crystal");

            SoAGlobalItem.SlayerItem.Add(Type);
        }

        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 40;
            Item.rare = ItemRarityID.Yellow;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.shoot = ModContent.ProjectileType<ExtractingSoul>();
            Item.shootSpeed = 16f;
            Item.noUseGraphic = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddRecipeGroup(ShardsRecipes.EvilBar, 15)
                .AddIngredient(ItemID.Stinger, 5)
                .AddTile(TileID.DemonAltar)
                .Register();
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            SlayerPlayer slayer = Main.LocalPlayer.GetModPlayer<SlayerPlayer>();
            if (slayer.soulCrystals.Count == 0)
                CombatText.NewText(player.getRect(), Color.Blue, "You have no Soul Crystals to extract");
            else SelectSoul(slayer);
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            SlayerPlayer slayer = Main.LocalPlayer.GetModPlayer<SlayerPlayer>();
            var selected = new TooltipLine(Mod, "SelectedSoul", "Selected: None");

            // Selected Soul Crystal
            for (int i = 0; i < entries.Count; i++)
            {
                PageEntry entry = entries[selectedSoul];
                if (slayer.soulCrystals.Contains(entry.crystalItem))
                {
                    selected = new TooltipLine(Mod, "SelectedSoul", "Selected: " + entry.entryName)
                    {
                        OverrideColor = Color.Blue
                    };
                }
            }
            tooltips.Add(selected);

            //Available Souls
            for (int i = 0; i < entries.Count; i++)
            {
                PageEntry entry = entries[i];
                if (slayer.soulCrystals.Contains(entry.crystalItem))
                {
                    tooltips.Add(new TooltipLine(Mod, "PageList", $"{entry.entryName} ({entry.mod})")
                    {
                        OverrideColor = entry.entryColor
                    });
                }
            }
            base.ModifyTooltips(tooltips);
        }

        public override bool CanUseItem(Player player)
        {
            SlayerPlayer slayer = Main.LocalPlayer.GetModPlayer<SlayerPlayer>();
            return !player.immune && slayer.soulCrystals.Count > 0;
        }

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            base.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Vector2 spawnLocation;
            if (player.direction == 1)
            {
                spawnLocation = player.Center + new Vector2(48, 0);
            }
            else
            {
                spawnLocation = player.Center + new Vector2(-48, 0);
            }
            Projectile.NewProjectile(source, spawnLocation, Vector2.Zero, type, 50, 0, Main.myPlayer, 0, selectedSoul);
            return false;
        }

        public void SelectSoul(SlayerPlayer slayer)
        {
            if (++selectedSoul >= entries.Count)
            {
                selectedSoul = 0;
            }
            PageEntry entry = entries[selectedSoul];
            if (!slayer.soulCrystals.Contains(entry.crystalItem))
            {
                selectedSoul++;
            }
        }

        public override void UpdateInventory(Player player)
        {
            if (selectedSoul == 0 && entries.Count > 0)
            {
                selectedSoul++;
            }
        }
    }
}
