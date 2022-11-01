﻿using Terraria;
using Terraria.ModLoader;
using ShardsOfAtheria.Projectiles.Pets;
using Microsoft.Xna.Framework;
using ShardsOfAtheria.Players;

namespace ShardsOfAtheria.Buffs.Pets
{
    public class NovaPetBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Smol Nova");
            Description.SetDefault("Look at her now");
            Main.vanityPet[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<SmolNova>()] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Vector2 toOwner = Vector2.Normalize(player.Center + new Vector2(5, 0));
                Projectile.NewProjectile(player.GetSource_Buff(buffIndex), player.Center - new Vector2(5, 0), toOwner * 5f, ModContent.ProjectileType<SmolNova>(), 0, 0f, player.whoAmI);
            }
        }
    }
}
