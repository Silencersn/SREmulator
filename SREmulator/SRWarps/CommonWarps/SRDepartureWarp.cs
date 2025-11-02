using SREmulator.SRItems;
using SREmulator.SRPlayers;

namespace SREmulator.SRWarps.CommonWarps;

public sealed class SRDepartureWarp : SRWarp
{
    public static readonly SRDepartureWarp DepartureWarp = new();

    public override SRWarpType WarpType => SRWarpType.DepartureWarp;

    public override ISRWarpResultItem Up5 => null!;
    public override ISRWarpResultItem[] Common5Characters => SRWarpItemPoolFactory.CreateStar5Characters(SRVersion.Ver1p0);
    public override ISRWarpResultItem[] Common5LightCones => null!;
    public override ISRWarpResultItem[] Up4 => null!;
    public override ISRWarpResultItem[] Common4Characters => SRWarpItemPoolFactory.CreateStar4Characters(SRVersion.Ver1p0);
    public override ISRWarpResultItem[] Common4LightCones => SRWarpItemPoolFactory.CreateStar4LightCones(SRVersion.Ver1p0);
    public override ISRWarpResultItem[] Common3 => SRWarpItemPoolFactory.CreateStar3LightCones(SRVersion.Ver1p0);

    public override bool PreWarp(SRPlayer player, int count)
    {
        var stats = new DepartureStats(player);

        if (stats.Counter >= 50)
            return false;

        if (count is not (1 or 10))
            return false;

        if (stats.NoCost >= count)
        {
            stats.NoCost -= (uint)count;
            return true;
        }

        if ((stats.Counter + stats.NoCost) <= 40 && player.WarpCurrencyStats.TryConsumeStarRailPass(8))
        {
            stats.NoCost += 10 - (uint)count;
            return true;
        }

        return false;
    }

    public override ISRWarpResultItem OnWarp(SRPlayer player)
    {
        var stats = new DepartureStats(player);
        if (stats.Counter is 49) return SRWarpCore.OneOf(Common5Characters);
        return SRWarpCore.CommonWarp(WarpStats, player.DepartureStats);
    }

    public override void PostWarp(SRPlayer player, ISRWarpResultItem item)
    {
        player.WarpCurrencyStats.GetWarpRewards(item, player.EidolonsStats);
        var stats = new DepartureStats(player);
        stats.Counter++;
    }

    private class DepartureStats
    {
        private readonly SRPlayer _player;

        public DepartureStats(SRPlayer player)
        {
            _player = player;
        }

        public ulong ExtraInfo
        {
            get => _player.DepartureStats.ExtraInfo ??= 0;
            set => _player.DepartureStats.ExtraInfo = value;
        }
        public uint Counter
        {
            get => (uint)(ExtraInfo >> 32);
            set => ExtraInfo = (ExtraInfo & uint.MaxValue) | (ulong)value << 32;
        }
        public uint NoCost
        {
            get => (uint)ExtraInfo;
            set => ExtraInfo = (ExtraInfo & (ulong)uint.MaxValue << 32) | value;
        }
    }
}
