namespace SREmulator.SRPlayers;

public sealed class SRPlayer : ICloneable
{
    public SRPlayerWarpCurrencyStats WarpCurrencyStats { get; set; } = new();
    public SRPlayerWarpStats CharacterEventStats { get; set; } = new();
    public SRPlayerWarpStats LightConeEventStats { get; set; } = new();
    public SRPlayerWarpStats StellarStats { get; set; } = new();
    public SRPlayerWarpStats DepartureStats { get; set; } = new();
    public SRPlayerEidolonsStats EidolonsStats { get; set; } = new();
    public SRPlayerLevelStats LevelStats { get; set; } = new();

    public SRPlayer() { }

    public SRPlayer Clone()
    {
        return new SRPlayer()
        {
            WarpCurrencyStats = WarpCurrencyStats.Clone(),
            CharacterEventStats = CharacterEventStats.Clone(),
            LightConeEventStats = LightConeEventStats.Clone(),
            StellarStats = StellarStats.Clone(),
            DepartureStats = DepartureStats.Clone(),
            EidolonsStats = EidolonsStats.Clone(),
            LevelStats = LevelStats.Clone(),
        };
    }

    object ICloneable.Clone()
    {
        return Clone();
    }
}

public interface ISRPlayerStats<TSelf> : ICloneable where TSelf : ISRPlayerStats<TSelf>, new()
{
    object ICloneable.Clone()
    {
        return Clone();
    }

    public new TSelf Clone();
}
