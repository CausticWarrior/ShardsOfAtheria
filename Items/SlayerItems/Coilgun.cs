using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ShardsOfAtheria.Items.Weapons.Ammo;
using Microsoft.Xna.Framework;

namespace ShardsOfAtheria.Items.SlayerItems
{
	public class Coilgun : SlayerItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Uses Rods as ammo\n" +
				"Tears through enemy armor\n" +
				"'Uses electro magnets to fire projectiles at insane velocities'\n" +
				"'Areus Railgun's older brother'");
		}

		public override void SetDefaults() 
		{
			Item.damage = 150;
			Item.DamageType = DamageClass.Magic;
			Item.noMelee = true;
			Item.width = 44;
			Item.height = 26;
			Item.useTime = 48;
			Item.useAnimation = 48;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 4f;
			Item.rare = ItemRarityID.Expert;
			Item.UseSound = SoundID.Item38;
			Item.autoReuse = false;
			Item.crit = 20;
			Item.value = Item.sellPrice(gold: 25);
			Item.shoot = ItemID.PurificationPowder;
			Item.shootSpeed = 20f;
			Item.useAmmo = ModContent.ItemType<AreusRod>();

			Item.mana = 20;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}

        public override void HoldItem(Player player)
        {
			player.armorPenetration = 20;
        }
	}
}