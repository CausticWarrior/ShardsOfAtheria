using Terraria.ID;
using Terraria.ModLoader;
using ShardsOfAtheria.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using ShardsOfAtheria.Projectiles.Weapon.Ranged;
using Terraria.DataStructures;

namespace ShardsOfAtheria.Items.SevenDeadlySouls.SinfulWeapon
{
    public class TheAmbassador : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Gun Spy TF2'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 26;
            Item.scale = .85f;

            Item.damage = 20;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 4f;
            Item.crit = 5;

            Item.useTime = 40;
            Item.useAnimation = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item41;
            Item.noMelee = true;

            Item.shootSpeed = 16f;
            Item.rare = ItemRarityID.Blue;
            Item.value = Item.sellPrice(0, 10, 25);
            Item.shoot = ProjectileID.PurificationPowder;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, Main.MouseWorld + new Vector2(-10, -10), Vector2.Zero, ModContent.ProjectileType<AmbassadorShot>(), Item.damage, Item.knockBack, player.whoAmI);
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
    }
}