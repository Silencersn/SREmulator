﻿using SREmulator.SRItems;
using SREmulator.SRPlayers;

namespace SREmulator.SRWarps.CommonWarps
{
    public sealed class SRDepartureWarp : SRWarp
    {
        public static readonly SRDepartureWarp DepartureWarp = new();

        public override SRWarpType WarpType => SRWarpType.DepartureWarp;

        public override ISRWarpResultItem Up5 => null!;
        public override ISRWarpResultItem[] Common5Characters => SRCharacters.AllNonLimitedStar5Characters;
        public override ISRWarpResultItem[] Common5LightCones => SRCharacters.AllNonLimitedStar5Characters;
        public override ISRWarpResultItem[] Up4 => null!;
        public override ISRWarpResultItem[] Common4Characters => SRWarpItemPoolFactory.CreateStar4Characters(SRVersion.Ver1p0);
        public override ISRWarpResultItem[] Common4LightCones => SRWarpItemPoolFactory.CreateStar4LightCones(SRVersion.Ver1p0);
        public override ISRWarpResultItem[] Common3 => SRLightCones.AllStar3LightCones;

        public override bool PreWarp(SRPlayer player, int count)
        {
            if (player.DepartureStats.ExtraInfo is not DepartureStats stats)
            {
                stats = new DepartureStats();
                player.DepartureStats.ExtraInfo = stats;
            }
            if (stats.Counter >= 50) return false;
            if (count is not 1 or 10) return false;
            if (stats.NoCost >= count)
            {
                stats.NoCost -= count;
                return true;
            }
            if (player.WarpCurrencyStats.TryConsumeStarRailPass(8))
            {
                stats.NoCost += 10 - count;
                return true;
            }
            return false;
        }

        public override ISRWarpResultItem OnWarp(SRPlayer player)
        {
            if (player.DepartureStats.ExtraInfo is not DepartureStats stats)
            {
                stats = new DepartureStats();
                player.DepartureStats.ExtraInfo = stats;
            }
            if (stats.Counter is 49) return SRWarpCore.OneOf(Common5Characters);
            return SRWarpCore.CommonWarp(WarpStats, player.DepartureStats);
        }

        public override void PostWarp(SRPlayer player, ISRWarpResultItem item)
        {
            player.WarpCurrencyStats.GetWarpReward(item, player.CharacterStats);
            if (player.DepartureStats.ExtraInfo is not DepartureStats stats)
            {
                stats = new DepartureStats();
                player.DepartureStats.ExtraInfo = stats;
            }
            stats.Counter++;
        }

        private class DepartureStats
        {
            public int Counter;
            public int NoCost;
        }
    }
}
