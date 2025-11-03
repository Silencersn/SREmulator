using SREmulator.SRItems;

namespace SREmulator.SRWarps;

public static class SRWarpItemPoolFactory
{
    internal static int VersionToCelestialInvitationCharacterCount(SRVersion version)
    {
        return (version & SRVersion.SpecifiedVersionMask) switch
        {
            SRVersion.Ver1p0 or
            SRVersion.Ver1p1 or
            SRVersion.Ver1p2 or
            SRVersion.Ver1p3 or
            SRVersion.Ver1p4 or
            SRVersion.Ver1p5 or
            SRVersion.Ver1p6 or
            SRVersion.Ver2p0 or
            SRVersion.Ver2p1 or
            SRVersion.Ver2p2 or
            SRVersion.Ver2p3 or
            SRVersion.Ver2p4 or
            SRVersion.Ver2p5 or
            SRVersion.Ver2p6 or
            SRVersion.Ver2p7 or
            SRVersion.Ver3p0 or
            SRVersion.Ver3p1 => 7,
            SRVersion.Ver3p2 or
            SRVersion.Ver3p3 or
            SRVersion.Ver3p4 or
            SRVersion.Ver3p5 or
            SRVersion.Ver3p6 => 10,

            _ => throw new ArgumentOutOfRangeException(nameof(version)),
        };
    }
    internal static readonly SRStar5Character[] InvitationCharactersOrderedByVersion = [
        // 1.0
        SRCharacters.Bailu,
        SRCharacters.Bronya,
        SRCharacters.Clara,
        SRCharacters.Gepard,
        SRCharacters.Himeko,
        SRCharacters.Welt,
        SRCharacters.Yanqing,

        // 3.2
        SRCharacters.Seele,
        SRCharacters.Blade,
        SRCharacters.FuXuan,
        ];
    public static SRStar5Character[] CreateCelestialInvitationCharacters(SRVersion version)
    {
        return InvitationCharactersOrderedByVersion.Take(VersionToCelestialInvitationCharacterCount(version)).ToArray();
    }

    internal static int VersionToStar5CharacterCount(SRVersion version)
    {
        return (version & SRVersion.SpecifiedVersionMask) switch
        {
            SRVersion.Ver1p0 or
            SRVersion.Ver1p1 or
            SRVersion.Ver1p2 or
            SRVersion.Ver1p3 or
            SRVersion.Ver1p4 or
            SRVersion.Ver1p5 or
            SRVersion.Ver1p6 or
            SRVersion.Ver2p0 or
            SRVersion.Ver2p1 or
            SRVersion.Ver2p2 or
            SRVersion.Ver2p3 or
            SRVersion.Ver2p4 or
            SRVersion.Ver2p5 or
            SRVersion.Ver2p6 or
            SRVersion.Ver2p7 or
            SRVersion.Ver3p0 or
            SRVersion.Ver3p1 => 7,

            _ => throw new ArgumentOutOfRangeException(nameof(version)),
        };
    }
    internal static readonly SRStar5Character[] Star5CharactersOrderedByVersion = [
        // 1.0
        SRCharacters.Bailu,
        SRCharacters.Bronya,
        SRCharacters.Clara,
        SRCharacters.Gepard,
        SRCharacters.Himeko,
        SRCharacters.Welt,
        SRCharacters.Yanqing
        ];
    public static SRStar5Character[] CreateStar5Characters(SRVersion version)
    {
        return Star5CharactersOrderedByVersion.Take(VersionToStar5CharacterCount(version)).ToArray();
    }

    internal static readonly SRStar5LightCone[] Star5LightConesOrderedByVersion = [
        // 1.0
        SRLightCones.SomethingIrreplaceable,
        SRLightCones.ButTheBattleIsntOver,
        SRLightCones.MomentOfVictory,
        SRLightCones.SleepLikeTheDead,
        SRLightCones.InTheNameOfTheWorld,
        SRLightCones.NightOnTheMilkyWay,
        SRLightCones.TimeWaitsForNoOne
        ];
    internal static int VersionToStar5LightConeCount(SRVersion version)
    {
        return (version & SRVersion.SpecifiedVersionMask) switch
        {
            SRVersion.Ver1p0 or
            SRVersion.Ver1p1 or
            SRVersion.Ver1p2 or
            SRVersion.Ver1p3 or
            SRVersion.Ver1p4 or
            SRVersion.Ver1p5 or
            SRVersion.Ver1p6 or
            SRVersion.Ver2p0 or
            SRVersion.Ver2p1 or
            SRVersion.Ver2p2 or
            SRVersion.Ver2p3 or
            SRVersion.Ver2p4 or
            SRVersion.Ver2p5 or
            SRVersion.Ver2p6 or
            SRVersion.Ver2p7 or
            SRVersion.Ver3p0 or
            SRVersion.Ver3p1 => 7,

            _ => throw new ArgumentOutOfRangeException(nameof(version)),
        };
    }
    public static SRStar5LightCone[] CreateStar5LightCones(SRVersion version)
    {
        return Star5LightConesOrderedByVersion.Take(VersionToStar5LightConeCount(version)).ToArray();
    }

    internal static int VersionToStar4CharacterCount(SRVersion version)
    {
        // 1.1及之后版本新增的4星角色在当时版本只会以Up形式出现
        // 在后一个版本开始才会加入池中
        // 例：雪衣在1.6上半Up，在下半抽不到，在2.0及以后才可被歪出来
        // 光锥同理
        return (version & SRVersion.SpecifiedVersionMask) switch
        {
            SRVersion.Ver1p0 => 13,
            SRVersion.Ver1p1 => 13,
            SRVersion.Ver1p2 => 14,
            SRVersion.Ver1p3 => 15,
            SRVersion.Ver1p4 => 16,
            SRVersion.Ver1p5 => 17,
            SRVersion.Ver1p6 => 18,
            SRVersion.Ver2p0 => 19,
            SRVersion.Ver2p1 => 20,
            SRVersion.Ver2p2 or
            SRVersion.Ver2p3 or
            SRVersion.Ver2p4 or
            SRVersion.Ver2p5 => 21,
            SRVersion.Ver2p6 or
            SRVersion.Ver2p7 or
            SRVersion.Ver3p0 or
            SRVersion.Ver3p1 => 23,

            _ => throw new ArgumentOutOfRangeException(nameof(version)),
        };
    }
    internal static readonly SRStar4Character[] Star4CharactersOrderedByVersion = [
        // 1.0 1
        SRCharacters.Arlan,
        SRCharacters.Asta,
        SRCharacters.DanHeng,
        SRCharacters.Herta,
        SRCharacters.Hook,
        SRCharacters.March7th,
        SRCharacters.Natasha,
        SRCharacters.Pela,
        SRCharacters.Qingque,
        SRCharacters.Sampo,
        SRCharacters.Serval,
        SRCharacters.Sushang,
        SRCharacters.Tingyun,

        // 1.1
        SRCharacters.Yukong,

        // 1.2
        SRCharacters.Luka,

        // 1.3
        SRCharacters.Lynx,

        // 1.4
        SRCharacters.Guinaifen,

        // 1.5
        SRCharacters.Hanya,

        // 1.6
        SRCharacters.Xueyi,

        // 2.0
        SRCharacters.Misha,

        // 2.1
        SRCharacters.Gallagher, 
        
        // 2.5
        SRCharacters.Moze,
        ];
    public static SRStar4Character[] CreateStar4Characters(SRVersion version)
    {
        return Star4CharactersOrderedByVersion.Take(VersionToStar4CharacterCount(version)).ToArray();
    }

    internal static int VersionToStar4LightConeCount(SRVersion version)
    {
        return (version & SRVersion.SpecifiedVersionMask) switch
        {
            SRVersion.Ver1p0 or
            SRVersion.Ver1p1 or
            SRVersion.Ver1p2 or
            SRVersion.Ver1p3 or
            SRVersion.Ver1p4 or
            SRVersion.Ver1p5 or
            SRVersion.Ver1p6 or
            SRVersion.Ver2p0 => 21,
            SRVersion.Ver2p1 => 22,
            SRVersion.Ver2p2 => 23,
            SRVersion.Ver2p3 => 24,
            SRVersion.Ver2p4 => 25,
            SRVersion.Ver2p5 => 26,
            SRVersion.Ver2p6 => 27,
            SRVersion.Ver2p7 or
            SRVersion.Ver3p0 => 28,
            SRVersion.Ver3p1 => 29,

            _ => throw new ArgumentOutOfRangeException(nameof(version)),
        };
    }
    internal static readonly SRStar4LightCone[] Star4LightConesOrderedByVersion = [
        // 1.0
        SRLightCones.DayOneOfMyNewLife,
        SRLightCones.MakeTheWorldClamor,
        SRLightCones.Swordplay,
        SRLightCones.MemoriesOfThePast,
        SRLightCones.OnlySilenceRemains,
        SRLightCones.TheMolesWelcomeYou,
        SRLightCones.PostOpConversation,
        SRLightCones.ASecretVow,
        SRLightCones.DanceDanceDance,
        SRLightCones.ConcertForTwo,
        SRLightCones.SharedFeeling,
        SRLightCones.EyesOfThePrey,
        SRLightCones.TrendOfTheUniversalMarket,
        SRLightCones.TheBirthOfTheSelf,
        SRLightCones.LandausChoice,
        SRLightCones.PerfectTiming,
        SRLightCones.UnderTheBlueSky,
        SRLightCones.GoodNightAndSleepWell,
        SRLightCones.PlanetaryRendezvous,
        SRLightCones.SubscribeForMore,
        SRLightCones.PoisedToBloom,

        // 2.0
        SRLightCones.IndeliblePromise,
        
        // 2.1
        SRLightCones.ResolutionShinesAsPearlsOfSweat,
        
        // 2.2
        SRLightCones.BoundlessChoreo,

        // 2.3
        SRLightCones.AfterTheCharmonyFall,

        // 2.4
        SRLightCones.GeniusesRepose,

        // 2.5
        SRLightCones.ShadowedByNight,

        // 2.6
        SRLightCones.DreamsMontage,

        // 3.0
        SRLightCones.GeniusesGreetings
        ];
    public static SRStar4LightCone[] CreateStar4LightCones(SRVersion version)
    {
        return Star4LightConesOrderedByVersion.Take(VersionToStar4LightConeCount(version)).ToArray();
    }

    internal static readonly SRStar3LightCone[] Star3LightConesOrderedByVersion = [
        // 1.0
        SRLightCones.DartingArrow,
        SRLightCones.DataBank,
        SRLightCones.Defense,
        SRLightCones.FineFruit,
        SRLightCones.HiddenShadow,
        SRLightCones.Loop,
        SRLightCones.Mediation,
        SRLightCones.MeshingCogs,
        SRLightCones.Multiplication,
        SRLightCones.MutualDemise,
        SRLightCones.Passkey,
        SRLightCones.Pioneering,
        SRLightCones.Sagacity,
        SRLightCones.ShatteredHome,
        SRLightCones.Void,
        SRLightCones.Adversarial,
        SRLightCones.Amber,
        SRLightCones.Arrows,
        SRLightCones.Chorus,
        SRLightCones.CollapsingSky,
        SRLightCones.Cornucopia,

        // 3.0
        SRLightCones.Shadowburn,
        SRLightCones.Reminiscence
        ];
    internal static int VersionToStar3LightConeCount(SRVersion version)
    {
        return (version & SRVersion.SpecifiedVersionMask) switch
        {
            SRVersion.Ver1p0 or
            SRVersion.Ver1p1 or
            SRVersion.Ver1p2 or
            SRVersion.Ver1p3 or
            SRVersion.Ver1p4 or
            SRVersion.Ver1p5 or
            SRVersion.Ver1p6 or
            SRVersion.Ver2p0 or
            SRVersion.Ver2p1 or
            SRVersion.Ver2p2 or
            SRVersion.Ver2p3 or
            SRVersion.Ver2p4 or
            SRVersion.Ver2p5 or
            SRVersion.Ver2p6 or
            SRVersion.Ver2p7 => 21,
            SRVersion.Ver3p0 => 22,

            _ => throw new ArgumentOutOfRangeException(nameof(version)),
        };
    }
    public static SRStar3LightCone[] CreateStar3LightCones(SRVersion version)
    {
        return Star3LightConesOrderedByVersion.Take(VersionToStar3LightConeCount(version)).ToArray();
    }
}
