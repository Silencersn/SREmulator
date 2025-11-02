using SREmulator.SRItems;
using SREmulator.SRPlayers;
using SREmulator.SRWarps;
using SREmulator.SRWarps.CommonWarps;
using SREmulator.SRWarps.EventWarps;

namespace SREmulator.CLI;

public class CLIArgs
{
    public sealed class CLIWarpArgs
    {
        internal const string CustomWarpName = "__custom";

        public string WarpName = string.Empty;
        public SRVersion WarpVersion;
        public SRWarpType WarpType;

        public ISRWarpResultItem? Up5;
        public ISRWarpResultItem? Up41;
        public ISRWarpResultItem? Up42;
        public ISRWarpResultItem? Up43;

        public SRWarp CreateWarp()
        {
            if (WarpName is CustomWarpName)
            {
                return WarpType switch
                {
                    SRWarpType.CharacterEventWarp => SRCharacterEventWarps.Create(
                        (SRStar5Character)Up5!,
                        (SRStar4Character)Up41!,
                        (SRStar4Character)Up42!,
                        (SRStar4Character)Up43!,
                        WarpVersion
                    ),

                    SRWarpType.LightConeEventWarp => SRLightConeEventWarps.Create(
                        (SRStar5LightCone)Up5!,
                        (SRStar4LightCone)Up41!,
                        (SRStar4LightCone)Up42!,
                        (SRStar4LightCone)Up43!,
                        WarpVersion
                    ),

                    _ => throw new InvalidOperationException()
                };
            }

            return WarpType switch
            {
                SRWarpType.CharacterEventWarp => SRCharacterEventWarps.GetWarpByNameAndVersion(WarpName, WarpVersion) ?? SRCharacterEventWarps.ButterflyOnSwordtip1,
                SRWarpType.LightConeEventWarp => SRLightConeEventWarps.GetWarpByNameAndVersion(WarpName, WarpVersion) ?? SRLightConeEventWarps.ButterflyOnSwordtip1,
                SRWarpType.StellarWarp => new SRStellarWarp(WarpVersion),
                SRWarpType.DepartureWarp => SRDepartureWarp.DepartureWarp,
                _ => throw new InvalidOperationException()
            };
        }
    }

    public bool Pause = false;
    public bool Silent = false;
    public bool Return = false;
    public bool Export = false;
    public bool Output = false;

    public Dictionary<SRCharacter, int> Eidolons = [];

    private CLIWarpTargetFactory? _targets = null;
    public CLIWarpTargetFactory Targets => _targets ??= new(this);
    public int Attempts = 10000;

    public int Days = 0;
    public bool ExpressSupplyPass = false;
    public int EquilibriumLevel = 6;

    public string Command = "help";

    public bool Help = false;
    public string? Language = null;

    internal SRPlayerWarpCurrencyStats WarpCurrencyStats { get; } = new();
    private readonly SRPlayerWarpStats _characterEventStats = new();
    private readonly SRPlayerWarpStats _lightConeEventStats = new();
    private readonly SRPlayerWarpStats _stellarStats = new();
    private readonly SRPlayerWarpStats _departureStats = new();
    internal SRPlayerWarpStats? CurrentWarpStats
    {
        get
        {
            return WarpArgs.WarpType switch
            {
                SRWarpType.CharacterEventWarp => _characterEventStats,
                SRWarpType.LightConeEventWarp => _lightConeEventStats,
                SRWarpType.StellarWarp => _stellarStats,
                SRWarpType.DepartureWarp => _departureStats,

                _ => null
            };
        }
    }

    internal SRPlayerEidolonsStats EidolonsStats
    {
        get
        {
            return new SRPlayerEidolonsStats(Eidolons);
        }
    }
    private SRPlayer? _player = null;
    internal CLIWarpArgs WarpArgs { get; private set; } = new();
    internal List<SRWarp> Warps = [];

    internal void TryAddAndResetWarp()
    {
        if (WarpArgs.WarpType is not SRWarpType.None)
            Warps.Add(WarpArgs.CreateWarp());
        WarpArgs = new();
    }

    public static CLIArgs Parse(string[] args)
    {
        CLIArgs result = new();
        CLIArgsSource source = new(args);

        while (true)
        {
            try
            {
                string arg = source.Next().ToLower();
                if (arg == string.Empty) break;
                if (arg.StartsWith("--"))
                {
                    string option = arg[2..];

                    if (CLIOptions.Options.ContainsKey(option))
                    {
                        var (applied, message) = CLIOptions.TryApplyOption(option, result, source);
                        if (!applied)
                            source.Warning($"参数错误 （在 '{arg}' 选项中）{(message is null ? string.Empty : $"({message})")}");
                    }
                    else
                    {
                        source.Warning($"无法识别的选项 '{arg}'");
                    }
                }
                else
                {
                    if (CLICommands.Commands.ContainsKey(arg))
                    {
                        result.Command = arg;
                        if (result.Command is "help") return result;
                    }
                    else
                    {
                        source.Warning($"无法识别的命令 '{arg}'");
                    }
                }
            }
            catch (Exception e)
            {
                source.Warning(e.Message);
            }
        }

        result.TryAddAndResetWarp();
        if (result.Warps.Count is 0)
        {
            source.Warning("未选择任何卡池，请使用 --new-warp 指定卡池");
        }
        if (result.Command is "achieve-average-warps" or "achieve-chance" && result.Targets.TargetToCount.Count is 0)
        {
            source.Warning("未设置任何目标，请使用 --target 设置目标");
        }

        _ = result.Targets.Create();

        foreach (var invalidTarget in result.Targets.InvalidTargets)
        {
            source.Warning($"无法实现的目标 '{invalidTarget.Name}' （该目标已被忽略）");
        }

        foreach (var warning in source.Warnings)
        {
            Console.WriteLine($"【警告】{warning}");
        }
        if (source.Warnings.Count > 0)
        {
            Console.WriteLine("----------");
        }

        return result;
    }

    public SRPlayer CreatePlayer()
    {
        if (_player is not null)
            return _player.Clone();

        var player = new SRPlayer
        {
            WarpCurrencyStats = WarpCurrencyStats.Clone(),
            CharacterEventStats = _characterEventStats.Clone(),
            LightConeEventStats = _lightConeEventStats.Clone(),
            StellarStats = _stellarStats.Clone(),
            DepartureStats = _departureStats.Clone(),
            EidolonsStats = EidolonsStats.Clone()
        };
        if (Days > 0)
        {
            player.LevelStats.EquilibriumLevel = EquilibriumLevel;
            player.WarpCurrencyStats.DaysLater(Days, ExpressSupplyPass, player.LevelStats);
        }
        _player = player;
        return _player.Clone();
    }
}
