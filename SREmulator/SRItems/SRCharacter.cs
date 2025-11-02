namespace SREmulator.SRItems;

public static partial class SRCharacters
{
    public static partial SRCharacter? GetItemByName(string? name);
}

public abstract class SRCharacter : SRItem, ISRWarpResultItem
{
}

public abstract class SRStar5Character : SRCharacter
{
    public sealed override SRItemRarity Rarity => SRItemRarity.Star5;
}

public abstract class SRStar4Character : SRCharacter
{
    public static SRStar4Character Case { get; } = new SRStar4CharacterCase();

    public sealed override SRItemRarity Rarity => SRItemRarity.Star4;

    private sealed class SRStar4CharacterCase : SRStar4Character
    {
        public override string Name => nameof(SRStar4Character);
    }
}
