﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ShardsOfAtheria.Players;
using ShardsOfAtheria.Projectiles.Weapon.Melee.EnergyScythe;
using ShardsOfAtheria.Utilities;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ShardsOfAtheria.Projectiles.Bases
{
    public abstract class EpicSwingSword : ModProjectile
    {
        public static Asset<Texture2D> SwishTexture => ModContent.Request<Texture2D>(typeof(EpicSwingSword).Namespace.Replace('.', '/') + "/Swish", AssetRequestMode.ImmediateLoad);
        public static Asset<Texture2D> Swish2Texture => ModContent.Request<Texture2D>(typeof(EpicSwingSword).Namespace.Replace('.', '/') + "/Swish2", AssetRequestMode.ImmediateLoad);
        public static SoundStyle HeavySwing => SoundID.DD2_MonkStaffSwing;

        private bool _init;
        public int swingDirection;
        public int hitboxOutwards;
        public int visualOutwards;
        public float rotationOffset;
        public bool forced50;
        public float scale;

        public bool playedSound;

        public bool damaging;
        public int damageTime;

        public int combo;

        private float armRotation;
        private Vector2 angleVector;
        public Vector2 AngleVector { get => angleVector; set => angleVector = Vector2.Normalize(value); }
        public Vector2 BaseAngleVector => Vector2.Normalize(Projectile.velocity);
        public virtual float AnimProgress => 1f - (Main.player[Projectile.owner].itemAnimation * (Projectile.extraUpdates + 1) + Projectile.numUpdates + 1) / (float)(Main.player[Projectile.owner].itemAnimationMax * (Projectile.extraUpdates + 1));

        public int amountAllowedToHit;

        public virtual bool SwingSwitchDir => AnimProgress > 0.6f && AnimProgress < 0.7f;

        public override void SetDefaults()
        {
            Projectile.tileCollide = false;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.penetrate = -1;
            Projectile.localNPCHitCooldown = 50;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.ignoreWater = true;
            amountAllowedToHit = 2;
        }

        public override bool? CanDamage()
        {
            return (AnimProgress > 0.4f && AnimProgress < 0.6f) ? null : false;
        }

        public override void AI()
        {
            var player = Main.player[Projectile.owner];

            if (player.itemAnimation > 1 && player.ownedProjectileCounts[Type] <= 1)
            {
                Projectile.timeLeft = 2;
            }

            var shards = player.ShardsOfAtheria();

            player.heldProj = Projectile.whoAmI;
            if (!_init)
            {
                Initialize(player, shards);
                scale = Projectile.scale;
                Projectile.netUpdate = true;
            }

            if (SwingSwitchDir)
            {
                UpdateDirection(player);
            }

            if (!player.frozen && !player.stoned)
            {
                var arm = Main.GetPlayerArmPosition(Projectile);
                float progress = AnimProgress;
                if (!forced50 && progress >= 0.5f)
                {
                    progress = 0.5f;
                    forced50 = true;
                }
                float swingProgress = SwingProgress(Math.Clamp(progress, 0f, 1f));
                AngleVector = GetOffsetVector(swingProgress);
                Projectile.position = arm + AngleVector * hitboxOutwards;
                Projectile.position.X -= Projectile.width / 2f;
                Projectile.position.Y -= Projectile.height / 2f;
                Projectile.rotation = (arm - Projectile.Center).ToRotation() + rotationOffset;
                UpdateSwing(progress, swingProgress);
                SetArmRotation(player, progress, swingProgress);
                float s = Projectile.scale;
                Projectile.scale = GetScale(swingProgress);
                visualOutwards = (int)GetVisualOuter(progress, swingProgress);
            }

            _init = true;
        }

        public virtual void UpdateSwing(float progress, float interpolatedSwingProgress)
        {

        }

        public virtual void FireProjectile(int type, int damage, float knockback, float velocity = 16f)
        {
            Vector2 position = Projectile.Center;
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), position, AngleVector * Projectile.velocity.Length() * velocity,
                        ModContent.ProjectileType<EnergyWave>(), damage, knockback, Projectile.owner);
        }

        public virtual float SwingProgress(float progress)
        {
            return progress;
        }
        public static float GenericSwing2(float progress, float pow = 2f)
        {
            if (progress > 0.5f)
            {
                return 0.5f - GenericSwing2(0.5f - (progress - 0.5f), pow) + 0.5f;
            }
            return ((float)Math.Sin(Math.Pow(progress, pow) * MathHelper.TwoPi - MathHelper.PiOver2) + 1f) / 2f;
        }
        public static float GenericSwing1(float progress, float pow = 2f, float startSwishing = 0.15f)
        {
            float oldProg = progress;
            float max = 1f - startSwishing;
            if (progress < startSwishing)
            {
                progress *= (float)Math.Pow(progress / startSwishing, pow);
            }
            else if (progress > max)
            {
                progress -= max;
                progress = startSwishing - progress;
                progress *= (float)Math.Pow(progress / startSwishing, pow);
                progress = startSwishing - progress;
                progress += max;
            }
            return MathHelper.Clamp((float)Math.Sin(progress * MathHelper.Pi - MathHelper.PiOver2) / 2f + 0.5f, 0f, oldProg);
        }
        public virtual Vector2 GetOffsetVector(float progress)
        {
            return BaseAngleVector.RotatedBy((progress * MathHelper.Pi - MathHelper.PiOver2) * -swingDirection);
        }
        public virtual float GetScale(float progress)
        {
            return scale;
        }
        public virtual float GetVisualOuter(float progress, float swingProgress)
        {
            return visualOutwards;
        }

        public void UpdateDirection(Player player)
        {
            if (angleVector.X < 0f)
            {
                player.direction = -1;
                Projectile.direction = -1;
            }
            else if (angleVector.X > 0f)
            {
                player.direction = 1;
                Projectile.direction = 1;
            }
        }

        protected virtual void Initialize(Player player, ShardsPlayer shards)
        {
            AngleVector = Projectile.velocity;
            combo = shards.itemCombo;
            ShardsHelpers.CappedMeleeScale(Projectile);
            swingDirection = 1;
            UpdateDirection(player);
            swingDirection *= Projectile.direction;
        }

        protected virtual void SetArmRotation(Player player, float progress, float swingProgress)
        {
            var diff = Main.player[Projectile.owner].MountedCenter - Projectile.Center;
            if (Math.Sign(diff.X) == -player.direction || progress < 0.1f)
            {
                var v = diff;
                v.X = Math.Abs(diff.X);
                armRotation = v.ToRotation();
            }

            if (armRotation > 1.1f)
            {
                player.bodyFrame.Y = 56;
            }
            else if (armRotation > 0.5f)
            {
                player.bodyFrame.Y = 56 * 2;
            }
            else if (armRotation < -0.5f)
            {
                player.bodyFrame.Y = 56 * 4;
            }
            else
            {
                player.bodyFrame.Y = 56 * 3;
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            amountAllowedToHit--;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return amountAllowedToHit > 0 ? null : false;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(swingDirection == -1);
            writer.Write(combo);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            swingDirection = reader.ReadBoolean() ? -1 : 1;
            combo = reader.ReadInt32();
        }
    }
}