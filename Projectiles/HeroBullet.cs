﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ShardsOfAtheria.Projectiles
{
    public class HeroBullet : ModProjectile {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
        }

        public override void SetDefaults() {
            Projectile.width = 2;
            Projectile.height = 2;

            Projectile.DamageType = DamageClass.Ranged;
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.tileCollide = true;
            Projectile.arrow = true;
            Projectile.light = 0.5f;
            Projectile.extraUpdates = 1;
            AIType = ProjectileID.Bullet;
            DrawOffsetX = -4;
        }
    }
}
