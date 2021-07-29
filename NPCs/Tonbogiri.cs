﻿using SagesMania.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SagesMania.NPCs
{
    public class Tonbogiri : ModNPC
    {
        public override void SetStaticDefaults()
        {
        }

        public override void SetDefaults()
        {
            npc.width = 116;
            npc.height = 116;
            npc.damage = 20;
            npc.defense = 8;
            npc.lifeMax = 40;
            npc.HitSound = SoundID.NPCHit4;
            npc.DeathSound = SoundID.NPCDeath52;
            npc.knockBackResist = 0.4f;
            npc.aiStyle = 23;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * .5f);
            npc.damage = (int)(npc.damage * .5f);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Player player = Main.LocalPlayer;
            if (!(spawnInfo.player.ZoneHoly || spawnInfo.player.ZoneCrimson || spawnInfo.player.ZoneCorrupt || Main.eclipse
                || spawnInfo.player.ZoneTowerNebula || spawnInfo.player.ZoneTowerVortex || spawnInfo.player.ZoneTowerSolar
                || spawnInfo.player.ZoneTowerStardust || Main.pumpkinMoon || Main.snowMoon || spawnInfo.playerSafe) && Main.hardMode
                && spawnInfo.player.ZoneBeach)
                return .25f;
            return 0f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextFloat() < .5f)
                Item.NewItem(npc.getRect(), ModContent.ItemType<SoulOfSpite>(), Main.rand.Next(3, 6));
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (Main.rand.NextFloat() >= .5f)
                target.AddBuff(BuffID.Bleeding, 10 * 60);
        }
    }
}