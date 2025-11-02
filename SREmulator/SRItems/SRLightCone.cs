namespace SREmulator.SRItems;

public static partial class SRLightCones
{
    public static partial SRLightCone? GetItemByName(string? name);
}

public abstract class SRLightCone : SRItem, ISRWarpResultItem
{
}

public abstract class SRStar5LightCone : SRLightCone
{
    public sealed override SRItemRarity Rarity => SRItemRarity.Star5;
}

public abstract class SRStar4LightCone : SRLightCone
{
    public static SRStar4LightCone Case { get; } = new SRStar4LightConeCase();

    public sealed override SRItemRarity Rarity => SRItemRarity.Star4;

    private sealed class SRStar4LightConeCase : SRStar4LightCone
    {
        public override string Name => nameof(SRStar4LightCone);
    }
}

public abstract class SRStar3LightCone : SRLightCone
{
    public static SRStar3LightCone Case { get; } = new SRStar3LightConeCase();

    public sealed override SRItemRarity Rarity => SRItemRarity.Star3;

    private sealed class SRStar3LightConeCase : SRStar3LightCone
    {
        public override string Name => nameof(SRStar3LightCone);
    }
}
