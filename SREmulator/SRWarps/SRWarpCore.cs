using SREmulator.SRItems;
using SREmulator.SRPlayers;

namespace SREmulator.SRWarps;

internal sealed class SRWarpStats
{
    public readonly SRWarpType WarpType;
    public readonly ISRWarpResultItem Up5;
    public readonly ISRWarpResultItem[] Common5Characters;
    public readonly ISRWarpResultItem[] Common5LightCones;
    public readonly ISRWarpResultItem[] Up4;
    public readonly ISRWarpResultItem[] Common4Characters;
    public readonly ISRWarpResultItem[] Common4LightCones;
    public readonly ISRWarpResultItem[] Common3;

    public SRWarpStats(SRWarpType warpType, ISRWarpResultItem up5, ISRWarpResultItem[] common5Characters, ISRWarpResultItem[] common5LightCones, ISRWarpResultItem[] up4, ISRWarpResultItem[] common4Characters, ISRWarpResultItem[] common4LightCones, ISRWarpResultItem[] common3)
    {
        WarpType = warpType;
        Up5 = up5;
        Common5Characters = common5Characters;
        Common5LightCones = common5LightCones;
        Up4 = up4;
        Common4Characters = common4Characters;
        Common4LightCones = common4LightCones;
        Common3 = common3;
    }
}

internal static class SRWarpCore
{
    internal static ISRWarpResultItem OneOf(ReadOnlySpan<ISRWarpResultItem> choices)
    {
        return choices[Random.Shared.Next(0, choices.Length)];
    }
    internal static bool NextBool(int numerator, int denominator)
    {
        return Random.Shared.Next(denominator) < numerator;
    }
    internal static int RollChance()
    {
        return Random.Shared.Next(1000);
    }
    internal static bool ContainsUp4(ReadOnlySpan<ISRWarpResultItem> up4, ISRWarpResultItem item)
    {
        return up4[0].Equals(item) || up4[1].Equals(item) || up4[2].Equals(item);
    }

    public static int GetStellarWarpStar5TypeWeight(int counter5Type)
    {
        // 原神
        return counter5Type switch
        {
            <= 145 => 30,
            >= 146 => 30 + 300 * (counter5Type - 145)
        };
    }
    public static int GetCharacterWarpStar4TypeWeight(int counter4Type)
    {
        // 原神
        return counter4Type switch
        {
            <= 16 => 255,
            >= 17 => 255 + 2550 * (counter4Type - 16)
        };
    }
    public static int GetLightConeWarpStar4TypeWeight(int counter4Type)
    {
        // 原神
        return counter4Type switch
        {
            <= 13 => 300,
            >= 14 => 300 + 3000 * (counter4Type - 13)
        };
    }

    public static ISRWarpResultItem GetCommonStar5(int counter5Character, int counter5LightCone, SRWarpStats warpStats)
    {
        if (warpStats.WarpType is SRWarpType.CharacterEventWarp or SRWarpType.DepartureWarp)
        {
            return OneOf(warpStats.Common5Characters);
        }
        if (warpStats.WarpType is SRWarpType.LightConeEventWarp)
        {
            return OneOf(warpStats.Common5LightCones);
        }

        int weightCharacter = GetStellarWarpStar5TypeWeight(counter5Character);
        int weightLightCone = GetStellarWarpStar5TypeWeight(counter5LightCone);
        if (NextBool(weightCharacter, weightCharacter + weightLightCone))
        {
            return OneOf(warpStats.Common5Characters);
        }
        else
        {
            return OneOf(warpStats.Common5LightCones);
        }
    }
    public static ISRWarpResultItem GetCommonStar4(int counter4Character, int counter4LightCone, SRWarpStats warpStats)
    {
        int weightCharacter;
        int weightLightCone;
        if (warpStats.WarpType is SRWarpType.CharacterEventWarp)
        {
            weightCharacter = GetCharacterWarpStar4TypeWeight(counter4Character);
            weightLightCone = GetCharacterWarpStar4TypeWeight(counter4LightCone);
        }
        else
        {
            weightCharacter = GetLightConeWarpStar4TypeWeight(counter4Character);
            weightLightCone = GetLightConeWarpStar4TypeWeight(counter4LightCone);
        }
        if (NextBool(weightCharacter, weightCharacter + weightLightCone))
        {
            return OneOf(warpStats.Common4Characters);
        }
        else
        {
            return OneOf(warpStats.Common4LightCones);
        }
    }
    public static ISRWarpResultItem GetEventWarpStar5(bool guarantee5, SRWarpStats warpStats, int numerator, int denominator)
    {
        if (guarantee5 || NextBool(numerator, denominator))
        {
            return warpStats.Up5;
        }
        return GetCommonStar5(default, default, warpStats);
    }
    public static ISRWarpResultItem GetEventWarpStar4(bool guarantee4, int counter4Character, int counter4LightCone, SRWarpStats warpStats, int numerator, int denominator)
    {
        if (guarantee4 || NextBool(numerator, denominator))
        {
            return OneOf(warpStats.Up4);
        }
        return GetCommonStar4(counter4Character, counter4LightCone, warpStats);
    }

    public static int GetCharacterWarpStar5Chance(int counter5)
    {
        return counter5 switch
        {
            <= 72 => 6,
            <= 88 => 6 + (counter5 - 72) * 60,
            89 => 1000,

            _ => throw new InvalidOperationException()
        };
    }
    public static int GetCharacterWarpStar4Chance(int counter4)
    {
        return counter4 switch
        {
            <= 7 => 51,
            8 => 51 + 510,
            >= 9 => 1000,
        };
    }

    public static int GetLightConeWarpStar5Chance(int counter5)
    {
        return counter5 switch
        {
            <= 64 => 8,
            <= 78 => 8 + (counter5 - 64) * 70,
            79 => 1000,
            _ => throw new InvalidOperationException()
        };
    }
    public static int GetLightConeWarpStar4Chance(int counter4)
    {
        return counter4 switch
        {
            <= 6 => 66,
            <= 8 => 66 + (counter4 - 6) * 400,
            >= 9 => 1000,
        };
    }

    public static ISRWarpResultItem EventWarp(SRWarpStats warpStats, SRPlayerWarpStats playerStats, int chance5, int chance4, int numerator, int denominator)
    {
        int roll = RollChance();
        if (roll < chance5)
        {
            var result = GetEventWarpStar5(playerStats.Guarantee5, warpStats, numerator, denominator);
            playerStats.GetStar5(warpStats.Up5.Equals(result), result is SRCharacter);
            return result;
        }
        else if (roll < chance5 + chance4)
        {
            var result = GetEventWarpStar4(playerStats.Guarantee4, playerStats.Counter4Character, playerStats.Counter4LightCone, warpStats, numerator, denominator);
            playerStats.GetStar4(ContainsUp4(warpStats.Up4, result), result is SRCharacter);
            return result;
        }
        else
        {
            playerStats.GetStar3();
            return OneOf(warpStats.Common3);
        }
    }
    public static ISRWarpResultItem CharacterEventWarp(SRWarpStats warpStats, SRPlayerWarpStats playerStats)
    {
        int chance5 = GetCharacterWarpStar5Chance(playerStats.Counter5);
        int chance4 = GetCharacterWarpStar4Chance(playerStats.Counter4);
        return EventWarp(warpStats, playerStats, chance5, chance4, 1, 2);
    }
    public static ISRWarpResultItem LightConeEventWarp(SRWarpStats warpStats, SRPlayerWarpStats playerStats)
    {
        int chance5 = GetLightConeWarpStar5Chance(playerStats.Counter5);
        int chance4 = GetLightConeWarpStar4Chance(playerStats.Counter4);
        return EventWarp(warpStats, playerStats, chance5, chance4, 3, 4);
    }
    public static ISRWarpResultItem CommonWarp(SRWarpStats warpStats, SRPlayerWarpStats playerStats)
    {
        int chance5 = GetCharacterWarpStar5Chance(playerStats.Counter5);
        int chance4 = GetCharacterWarpStar4Chance(playerStats.Counter4);
        int roll = RollChance();
        if (roll < chance5)
        {
            var result = GetCommonStar5(playerStats.Counter5Character, playerStats.Counter5LightCone, warpStats);
            playerStats.GetStar5(default, result is SRCharacter);
            return result;
        }
        else if (roll < chance5 + chance4)
        {
            var result = GetCommonStar4(playerStats.Counter4Character, playerStats.Counter4LightCone, warpStats);
            playerStats.GetStar4(default, result is SRCharacter);
            return result;
        }
        else
        {
            playerStats.GetStar3();
            return OneOf(warpStats.Common3);
        }
    }
}
