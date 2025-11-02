using SREmulator.SRItems;

namespace SREmulator.SRPlayers;

public sealed class SRPlayerEidolonsStats : ISRPlayerStats<SRPlayerEidolonsStats>
{
    internal Dictionary<SRCharacter, int> CharacterEidolons { get; set; } = [];

    public SRPlayerEidolonsStats()
    {

    }
    public SRPlayerEidolonsStats(Dictionary<SRCharacter, int> eidolons)
    {
        CharacterEidolons = new(eidolons);
    }

    public bool TryAdd(SRCharacter character, out int eidolons)
    {
        if (!CharacterEidolons.TryGetValue(character, out eidolons)) eidolons = -1;
        if (eidolons >= 6) return false;
        CharacterEidolons[character] = eidolons += 1;
        return true;
    }

    public SRPlayerEidolonsStats Clone()
    {
        return new SRPlayerEidolonsStats(CharacterEidolons);
    }
}
