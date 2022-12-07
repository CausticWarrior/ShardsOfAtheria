﻿using Microsoft.Xna.Framework;
using ShardsOfAtheria.Globals.Elements;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ShardsOfAtheria.Projectiles.Weapon.Ranged
{
    public class PrometheusFire : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            ProjectileElements.FireProj.Add(Type);
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.DamageType = DamageClass.Ranged;

            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.arrow = false;
            Projectile.light = 1f;
            Projectile.extraUpdates = 1;
            Projectile.timeLeft = 120;
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation();
            // Set both direction and spriteDirection to 1 or -1 (right and left respectively)
            // Projectile.direction is automatically set correctly in Projectile.Update, but we need to set it here or the textures will draw incorrectly on the 1st frame.
            Projectile.spriteDirection = Projectile.direction = (Projectile.velocity.X > 0).ToDirectionInt();
            // Adding Pi to rotation if facing left corrects the drawing
            Projectile.rotation = Projectile.velocity.ToRotation() + (Projectile.spriteDirection == 1 ? 0f : MathHelper.Pi);
            if (Projectile.spriteDirection == 1) // facing right
            {
                DrawOffsetX = -19;
                DrawOriginOffsetX = 9;
            }
            else
            {
                DrawOffsetX = 0;
                DrawOriginOffsetX = -9; // Math works out that this is negative of the other value.
            }
            // Loop through the 4 animation frames, spending 5 ticks on each.
            if (++Projectile.frameCounter >= 15)
            {
                Projectile.frameCounter = 0;
                if (++Projectile.frame >= 4)
                {
                    Projectile.frame = 0;
                }
            }
            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustID.Torch,
                    Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 200, Scale: 1.2f);
                dust.velocity += Projectile.velocity * 0.3f;
                dust.velocity *= 0.2f;
            }
            if (Main.rand.NextBool(4))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustID.Torch,
                    0, 0, 254, Scale: 0.3f);
                dust.velocity += Projectile.velocity * 0.5f;
                dust.velocity *= 0.5f;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.OnFire, 10 * 60);
        }

        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item74, Projectile.position);
            for (int i = 0; i < 10; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustID.Torch,
                    Projectile.velocity.X * .2f, Projectile.velocity.Y * .2f, 200, Scale: 1.2f);
                dust.velocity += Projectile.velocity * 0.3f;
                dust.velocity *= 0.2f;

                Dust dust2 = Dust.NewDustDirect(Projectile.position, Projectile.height, Projectile.width, DustID.Torch,
                    0, 0, 254, Scale: 0.3f);
                dust2.velocity += Projectile.velocity * 0.5f;
                dust2.velocity *= 0.5f;
            }
            base.Kill(timeLeft);
        }
    }
}
