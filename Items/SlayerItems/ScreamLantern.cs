using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent.Creative;
using Microsoft.Xna.Framework;
using ShardsOfAtheria.Projectiles.Weapon.Magic;
using Terraria.DataStructures;
using Terraria.Audio;

namespace ShardsOfAtheria.Items.SlayerItems
{
    public class ScreamLantern : SlayerItem
    {
        int shockwave = 0;

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Fires a 2 burst of shockwaves that can bounce off of tiles\n" +
                "Shockwaves get faster after each bounce\n" +
                "'I like ya cut g'\n" +
                "'No voice to cry suffering'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 28;

            Item.damage = 40;
            Item.DamageType = DamageClass.Magic;
            Item.knockBack = 4f;
            Item.crit = 6;

            Item.useTime = 12;
            Item.useAnimation = 24;
            Item.reuseDelay = 36;
            Item.useStyle = ItemUseStyleID.RaiseLamp;
            Item.UseSound = SoundID.Item38;
            Item.autoReuse = true;
            Item.noMelee = true;
            Item.holdStyle = ItemHoldStyleID.HoldLamp;

            Item.shootSpeed = 20f;
            Item.rare = ModContent.RarityType<SlayerRarity>();
            Item.value = Item.sellPrice(0, 1, 25);
            Item.shoot = ModContent.ProjectileType<ScreamShockwave>();
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (shockwave >= 10)
            {
                shockwave = 0;
            }
            shockwave++;
            if (shockwave == 9 || shockwave == 10)
            {
                Projectile.NewProjectile(source, player.Center, new Vector2(0, -1).RotatedBy(MathHelper.ToRadians(72)) * 16, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, player.Center, new Vector2(0, -1).RotatedBy(MathHelper.ToRadians(144)) * 16, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, player.Center, new Vector2(0, -1).RotatedBy(MathHelper.ToRadians(216)) * 16, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, player.Center, new Vector2(0, -1).RotatedBy(MathHelper.ToRadians(288)) * 16, type, damage, knockback, player.whoAmI);
                Projectile.NewProjectile(source, player.Center, new Vector2(0, -1).RotatedBy(MathHelper.ToRadians(360)) * 16, type, damage, knockback, player.whoAmI);
                return false;
            }

            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}