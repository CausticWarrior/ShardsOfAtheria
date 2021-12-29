﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ShardsOfAtheria.Items.Placeable;

namespace ShardsOfAtheria.Tiles
{
    public class BionicOre : ModTile
    {
        public override void SetStaticDefaults()
        {
            TileID.Sets.Ore[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileLighted[Type] = true;
			Main.tileSpelunker[Type] = true; // The tile will be affected by spelunker highlighting
			Main.tileOreFinderPriority[Type] = 410;

            ItemDrop = ModContent.ItemType<BionicOreItem>();

            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Bionic Ore");
            AddMapEntry(new Color(100, 100, 100), name);

            DustType = DustID.Platinum;
            SoundType = SoundID.Tink;
            SoundStyle = 1;
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }
    }
}
