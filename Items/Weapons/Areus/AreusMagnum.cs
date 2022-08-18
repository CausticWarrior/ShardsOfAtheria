using Terraria.ID;
using Terraria.ModLoader;
using ShardsOfAtheria.Items.Placeable;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using ShardsOfAtheria.Items.Potions;

namespace ShardsOfAtheria.Items.Weapons.Areus
{
    public class AreusMagnum : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("'Headshots do not crit'");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 44;
            Item.height = 26;
            Item.scale = .85f;

            Item.damage = 37;
            Item.DamageType = DamageClass.Ranged;
            Item.knockBack = 4f;
            Item.crit = 5;

            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = SoundID.Item41;
            Item.noMelee = true;

            Item.shootSpeed = 16f;
            Item.rare = ItemRarityID.Cyan;
            Item.value = Item.sellPrice(0, 0, 25);
            Item.shoot = ItemID.PurificationPowder;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<AreusShard>(), 5)
                .AddIngredient(ModContent.ItemType<SoulOfDaylight>(), 7)
                .AddIngredient(ItemID.HellstoneBar, 10)
                .AddTile(TileID.Hellforge)
                .Register();
        }

        public override void ModifyWeaponDamage(Player player, ref StatModifier damage)
        {
            if (player.HasBuff(ModContent.BuffType<Conductive>()))
            {
                damage += .15f;
            }
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-1, 0);
        }
    }
}