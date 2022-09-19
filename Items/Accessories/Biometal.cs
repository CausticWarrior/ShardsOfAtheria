﻿using Microsoft.Xna.Framework;
using ShardsOfAtheria.Buffs;
using ShardsOfAtheria.Items.Placeable;
using ShardsOfAtheria.Players;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace ShardsOfAtheria.Items.Accessories
{
    public class Biometal : ModItem
    {
        public override void Load()
        {
            // The code below runs only if we're not loading on a server
            if (Main.netMode != NetmodeID.Server)
            {
                // Add equip textures
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Head}", EquipType.Head, this);
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Body}", EquipType.Body, this);
                EquipLoader.AddEquipTexture(Mod, $"{Texture}_{EquipType.Legs}", EquipType.Legs, this);

            }
        }

        // Called in SetStaticDefaults
        private void SetupDrawing()
        {
            int equipSlotHead = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Head);
            int equipSlotBody = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Body);
            int equipSlotLegs = EquipLoader.GetEquipSlot(Mod, Name, EquipType.Legs);

            ArmorIDs.Head.Sets.DrawHead[equipSlotHead] = true;
            ArmorIDs.Body.Sets.HidesTopSkin[equipSlotBody] = true;
            ArmorIDs.Body.Sets.HidesArms[equipSlotBody] = true;
            ArmorIDs.Legs.Sets.HidesBottomSkin[equipSlotLegs] = true;
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Equipping transforms the user into a Mega Man\n" +
                "25% Increased damage\n" +
                "Increases movement speed by 10%\n" +
                "Increased life regen\n" +
                "Increased life by 100 and mana by 40\n" +
                "Grants dash, wall sliding and immunity to fall damage and certain debuffs");

            SetupDrawing();

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            var list = ShardsOfAtheria.OverdriveKey.GetAssignedKeys();
            string keyname = "Not bound";

            if (list.Count > 0)
            {
                keyname = list[0];
            }

            tooltips.Add(new TooltipLine(Mod, "Damage", $"Press '[i:{keyname}]' to activate or deactivate Overdrive\n" +
                "Overdrive doubles all damage\n" +
                "Overdrive lasts until you get hit or run out of Overdrive time"));
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.scale = .7f;
            Item.accessory = true;
            Item.canBePlacedInVanityRegardlessOfConditions = true;
            Item.defense = 20;

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 5);
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BionicBarItem>(), 15)
                .AddIngredient(ItemID.SoulofNight, 5)
                .AddIngredient(ItemID.SoulofLight, 5)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (!player.HasBuff(ModContent.BuffType<Megamerged>()))
            {
                player.AddBuff(ModContent.BuffType<Megamerged>(), 60);
                if (ModContent.GetInstance<ConfigClientSide>().biometalSound)
                {
                    if (player.Male)
                        SoundEngine.PlaySound(new SoundStyle($"{nameof(ShardsOfAtheria)}/Sounds/Item/MegamergeMale"));
                    else SoundEngine.PlaySound(new SoundStyle($"{nameof(ShardsOfAtheria)}/Sounds/Item/MegamergeFemale"));
                }
            }

            player.GetModPlayer<SoAPlayer>().Biometal = true;
            player.GetModPlayer<SoAPlayer>().BiometalHideVanity = hideVisual;

            player.extraFall += 45;
            player.GetDamage(DamageClass.Generic) += 0.25f;
            player.statLifeMax2 += 100;
            player.statManaMax2 += 40;
            player.noFallDmg = true;
            player.buffImmune[BuffID.Bleeding] = true;
            player.buffImmune[BuffID.Poisoned] = true;
            player.buffImmune[BuffID.Weak] = true;
            player.buffImmune[BuffID.WitheredWeapon] = true;
            player.buffImmune[BuffID.Venom] = true;
            player.spikedBoots++;

            BiometalDashPlayer mp = player.GetModPlayer<BiometalDashPlayer>();
            //If the dash is not active, immediately return so we don't do any of the logic for it
            if (!mp.DashActive)
                return;

            player.eocDash = mp.DashTimer;
            player.armorEffectDrawShadowEOCShield = true;

            //This is where we set the afterimage effect.  You can replace these two lines with whatever you want to happen during the dash
            //Some examples include:  spawning dust where the player is, adding buffs, making the player immune, etc.
            //Here we take advantage of "player.eocDash" and "player.armorEffectDrawShadowEOCShield" to get the Shield of Cthulhu's afterimage effect

            //If the dash has just started, apply the dash velocity in whatever direction we wanted to dash towards
            if (mp.DashTimer == BiometalDashPlayer.MAX_DASH_TIMER)
            {
                Vector2 newVelocity = player.velocity;

                if (mp.DashDir == BiometalDashPlayer.DashLeft && player.velocity.X > -mp.DashVelocity || mp.DashDir == BiometalDashPlayer.DashRight && player.velocity.X < mp.DashVelocity)
                {
                    //X-velocity is set here
                    int dashDirection = mp.DashDir == BiometalDashPlayer.DashRight ? 1 : -1;
                    newVelocity.X = dashDirection * mp.DashVelocity;
                }

                player.velocity = newVelocity;
            }

            //Decrement the timers
            mp.DashTimer--;
            mp.DashDelay--;

            if (mp.DashDelay == 0)
            {
                //The dash has ended.  Reset the fields
                mp.DashDelay = BiometalDashPlayer.MAX_DASH_DELAY;
                mp.DashTimer = BiometalDashPlayer.MAX_DASH_TIMER;
                mp.DashActive = false;
            }
        }

        public override void UpdateVanity(Player player)
        {
            if (!player.HasBuff(ModContent.BuffType<Megamerged>()))
            {
                player.AddBuff(ModContent.BuffType<Megamerged>(), 60);
                if (ModContent.GetInstance<ConfigClientSide>().biometalSound)
                {
                    if (player.Male)
                        SoundEngine.PlaySound(new SoundStyle($"{nameof(ShardsOfAtheria)}/Sounds/Item/MegamergeMale"));
                    else SoundEngine.PlaySound(new SoundStyle($"{nameof(ShardsOfAtheria)}/Sounds/Item/MegamergeFemale"));
                }
            }
        }

        public override void UpdateInventory(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<Megamerged>()))
            {
                player.ClearBuff(ModContent.BuffType<Megamerged>());
                SoundEngine.PlaySound(SoundID.Item4);
            }
        }

        public override void HoldItem(Player player)
        {
            if (player.HasBuff(ModContent.BuffType<Megamerged>()))
            {
                player.ClearBuff(ModContent.BuffType<Megamerged>());
                SoundEngine.PlaySound(SoundID.Item4);
            }
        }

        // TODO: Fix this once new hook prior to FrameEffects added.
        // Required so UpdateVanitySet gets called in EquipTextures
        public override bool IsVanitySet(int head, int body, int legs) => true;
    }

    public class BiometalDashPlayer : ModPlayer
    {
        //These indicate what direction is what in the timer arrays used
        public static readonly int DashRight = 2;
        public static readonly int DashLeft = 3;

        //The direction the player is currently dashing towards.  Defaults to -1 if no dash is ocurring.
        public int DashDir = -1;

        //The fields related to the dash accessory
        public bool DashActive = false;
        public int DashDelay = MAX_DASH_DELAY;
        public int DashTimer = MAX_DASH_TIMER;
        //The initial velocity.  10 velocity is about 37.5 tiles/second or 50 mph
        public readonly float DashVelocity = 10f;
        //These two fields are the max values for the delay between dashes and the length of the dash in that order
        //The time is measured in frames
        public static readonly int MAX_DASH_DELAY = 50;
        public static readonly int MAX_DASH_TIMER = 35;

        public override void ResetEffects()
        {
            //ResetEffects() is called not long after player.doubleTapCardinalTimer's values have been set

            //Check if the ExampleDashAccessory is equipped and also check against this priority:
            // If the Shield of Cthulhu, Master Ninja Gear, Tabi and/or Solar Armour set is equipped, prevent this accessory from doing its dash effect
            //The priority is used to prevent undesirable effects.
            //Without it, the player is able to use the ExampleDashAccessory's dash as well as the vanilla ones
            bool dashBuffActive = false;

            //This is the loop used in vanilla to update/check the not-vanity accessories
            for (int i = 3; i < 8 + Player.extraAccessorySlots; i++)
            {
                Item item = Player.armor[i];

                //Set the flag for the ExampleDashAccessory being equipped if we have it equipped OR immediately return if any of the accessories are
                // one of the higher-priority ones
                if (item.type == ModContent.ItemType<Biometal>())
                    dashBuffActive = true;
                else if (item.type == ItemID.EoCShield || item.type == ItemID.MasterNinjaGear || item.type == ItemID.Tabi)
                    return;
            }

            //If we don't have the ExampleDashAccessory equipped or the player has the Solor armor set equipped, return immediately
            //Also return if the player is currently on a mount, since dashes on a mount look weird, or if the dash was already activated
            if (!dashBuffActive || Player.setSolar || Player.mount.Active || DashActive)
                return;

            //When a directional key is pressed and released, vanilla starts a 15 tick (1/4 second) timer during which a second press activates a dash
            //If the timers are set to 15, then this is the first press just processed by the vanilla logic.  Otherwise, it's a double-tap
            if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15)
                DashDir = DashRight;
            else if (Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[DashLeft] < 15)
                DashDir = DashLeft;
            else
                return;  //No dash was activated, return

            DashActive = true;

            //Here you'd be able to set an effect that happens when the dash first activates
            //Some examples include:  the larger smoke effect from the Master Ninja Gear and Tabi
        }
    }
}