using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SagesMania.Tiles;
using SagesMania.Projectiles;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SagesMania.Items.Placeable;
using SagesMania.Buffs;

namespace SagesMania.Items.Weapons
{
	public class BlackAreusSword : AreusWeapon
	{
		public override void SetStaticDefaults()
        {
			Tooltip.SetDefault("Definitely wont(?) shock you");
		}

		public override void SafeSetDefaults() 
		{
			item.damage = 200;
			item.melee = true;
			item.width = 64;
			item.height = 76;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.value = Item.sellPrice(gold: 50);
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.crit = 21;
			item.shoot = ModContent.ProjectileType<ElectricBlade>();
			item.shootSpeed = 10;
			areusResourceCost = 1;
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AreusBarItem>(), 20);
			recipe.AddIngredient(ItemID.FragmentVortex, 20);
			recipe.AddTile(ModContent.TileType<AreusForge>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			// Add the Onfire buff to the NPC for 1 second when the weapon hits an NPC
			// 60 frames = 1 second
			target.AddBuff(BuffID.Electrified, 600);
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			Texture2D texture = mod.GetTexture("Items/Weapons/BlackAreusSword_Glow");
			spriteBatch.Draw
			(
				texture,
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.HasBuff(ModContent.BuffType<Overdrive>()))
				type = ModContent.ProjectileType<ElectricScythe>();
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}

	}
}