using SREmulator.SRItems;
using SREmulator.SRWarps;

namespace SREmulator.CLI;

public static class CLIOptions
{
    public static readonly Dictionary<string, CLIOption> Options;

    static CLIOptions()
    {
        Options = [];

        AppendOption<PauseOption>();
        AppendOption<SilentOption>();
        AppendOption<ReturnOption>();
        AppendOption<ExportOption>();
        AppendOption<OutputOption>();
        AppendOption<StarRailPassOption>();
        AppendOption<StarRailSpecialPassOption>();
        AppendOption<UndyingStarlightOption>();
        AppendOption<StellarJadeOption>();
        AppendOption<OneiricShardOption>();
        AppendOption<EidolonOption>();
        AppendOption<UnlimitedResourcesOption>();
        AppendOption<Counter5Option>();
        AppendOption<Guarantee5Option>();
        AppendOption<Counter5CharacterOption>();
        AppendOption<Counter5LightConeOption>();
        AppendOption<Counter4Option>();
        AppendOption<Guarantee4Option>();
        AppendOption<Counter4CharacterOption>();
        AppendOption<Counter4LightConeOption>();
        AppendOption<WarpNameOption>();
        AppendOption<WarpVersionOption>();
        AppendOption<TargetOption>();
        AppendOption<AttemptsOption>();
        AppendOption<DaysOption>();
        AppendOption<ExpressSupplyPassOption>();
        AppendOption<HelpOption>();
        AppendOption<LanguageOption>();
        AppendOption<NoRewardsOption>();
        AppendOption<NewWarpOption>();
        AppendOption<CustomWarpOption>();
        AppendOption<EquilibriumLevelOption>();

        static void AppendOption<T>() where T : CLIOption, new()
        {
            var option = new T();
            Options.Add(option.Name, option);
        }
    }

    public static (bool Applied, string? Message) TryApplyOption(string name, CLIArgs args, CLIArgsSource source)
    {
        if (!Options.TryGetValue(name, out CLIOption? option))
            return (false, $"无法识别的选项 --'{name}'");

        return option.TryApplyToCLIArgs(args, source);
    }
}

public abstract class CLIOption
{
    public abstract string Name { get; }

    public abstract (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source);
}

public sealed class PauseOption : CLIOption
{
    public override string Name => "pause";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.Pause = true;
        return (true, null);
    }
}

public sealed class SilentOption : CLIOption
{
    public override string Name => "silent";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.Silent = true;
        return (true, null);
    }
}

public sealed class ReturnOption : CLIOption
{
    public override string Name => "return";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.Return = true;
        return (true, null);
    }
}

public sealed class ExportOption : CLIOption
{
    public override string Name => "export";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.Export = true;
        return (true, null);
    }
}

public sealed class OutputOption : CLIOption
{
    public override string Name => "output";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.Output = true;
        return (true, null);
    }
}

public sealed class StarRailPassOption : CLIOption
{
    public override string Name => "star-rail-pass";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.WarpCurrencyStats.StarRailPass = source.NextInt32(0);
        return (true, null);
    }
}

public sealed class StarRailSpecialPassOption : CLIOption
{
    public override string Name => "star-rail-special-pass";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.WarpCurrencyStats.StarRailSpecialPass = source.NextInt32(0);
        return (true, null);
    }
}

public sealed class UndyingStarlightOption : CLIOption
{
    public override string Name => "undying-starlight";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.WarpCurrencyStats.UndyingStarlight = source.NextInt32(0);
        return (true, null);
    }
}

public sealed class StellarJadeOption : CLIOption
{
    public override string Name => "stellar-jade";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.WarpCurrencyStats.StellarJade = source.NextInt32(0);
        return (true, null);
    }
}

public sealed class OneiricShardOption : CLIOption
{
    public override string Name => "oneiric-shard";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.WarpCurrencyStats.OneiricShard = source.NextInt32(0);
        return (true, null);
    }
}

public sealed class EidolonOption : CLIOption
{
    public override string Name => "eidolon";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        var character = source.NextWarpResultItem<SRCharacter>();
        if (character is null)
            return (false, null);
        int count = source.NextInt32(-1, 6);
        args.Eidolons[character] = count;
        return (true, null);
    }
}

public sealed class UnlimitedResourcesOption : CLIOption
{
    public override string Name => "unlimited-resources";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.WarpCurrencyStats.UnlimitedResources = true;
        return (true, null);
    }
}


public sealed class Counter5Option : CLIOption
{
    public override string Name => "counter5";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        if (args.CurrentWarpStats is null)
            return (false, "未指定卡池类型，需先使用 '--new-warp' 指定卡池类型");

        args.CurrentWarpStats.Counter5 = source.NextInt32(0, 89);
        return (true, null);
    }
}

public sealed class Guarantee5Option : CLIOption
{
    public override string Name => "guarantee5";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        if (args.CurrentWarpStats is null)
            return (false, "未指定卡池类型，需先使用 '--new-warp' 指定卡池类型");

        args.CurrentWarpStats.Guarantee5 = true;
        return (true, null);
    }
}

public sealed class Counter5CharacterOption : CLIOption
{
    public override string Name => "counter5character";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        if (args.CurrentWarpStats is null)
            return (false, "未指定卡池类型，需先使用 '--new-warp' 指定卡池类型");

        args.CurrentWarpStats.Counter5Character = source.NextInt32(0);
        return (true, null);
    }
}

public sealed class Counter5LightConeOption : CLIOption
{
    public override string Name => "counter5lightcone";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        if (args.CurrentWarpStats is null)
            return (false, "未指定卡池类型，需先使用 '--new-warp' 指定卡池类型");

        args.CurrentWarpStats.Counter5LightCone = source.NextInt32(0);
        return (true, null);
    }
}

public sealed class Counter4Option : CLIOption
{
    public override string Name => "counter4";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        if (args.CurrentWarpStats is null)
            return (false, "未指定卡池类型，需先使用 '--new-warp' 指定卡池类型");

        args.CurrentWarpStats.Counter4 = source.NextInt32(0);
        return (true, null);
    }
}

public sealed class Guarantee4Option : CLIOption
{
    public override string Name => "guarantee4";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        if (args.CurrentWarpStats is null)
            return (false, "未指定卡池类型，需先使用 '--new-warp' 指定卡池类型");

        args.CurrentWarpStats.Guarantee4 = true;
        return (true, null);
    }
}

public sealed class Counter4CharacterOption : CLIOption
{
    public override string Name => "counter4character";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        if (args.CurrentWarpStats is null)
            return (false, "未指定卡池类型，需先使用 '--new-warp' 指定卡池类型");

        args.CurrentWarpStats.Counter4Character = source.NextInt32(0);
        return (true, null);
    }
}

public sealed class Counter4LightConeOption : CLIOption
{
    public override string Name => "counter4lightcone";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        if (args.CurrentWarpStats is null)
            return (false, "未指定卡池类型，需先使用 '--new-warp' 指定卡池类型");

        args.CurrentWarpStats.Counter4LightCone = source.NextInt32(0);
        return (true, null);
    }
}

public sealed class WarpNameOption : CLIOption
{
    public override string Name => "warp-name";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.WarpArgs.WarpName = source.Next();
        return (true, null);
    }
}

public sealed class WarpVersionOption : CLIOption
{
    public override string Name => "warp-version";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        var major = source.NextInt32(1, 3);
        var minor = source.NextInt32(0, 7);
        args.WarpArgs.WarpVersion = SRVersions.CreateAvailable(major, minor);
        return (true, null);
    }
}

public sealed class TargetOption : CLIOption
{
    public override string Name => "target";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        var item = source.NextWarpResultItem();
        if (item is null)
            return (false, null);
        int count = source.NextInt32(0);
        args.Targets.AppendTarget(item, count);
        return (true, null);
    }
}

public sealed class AttemptsOption : CLIOption
{
    public override string Name => "attempts";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.Attempts = source.NextInt32(1);
        return (true, null);
    }
}

public sealed class DaysOption : CLIOption
{
    public override string Name => "days";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.Days = source.NextInt32(0);
        return (true, null);
    }
}

public sealed class ExpressSupplyPassOption : CLIOption
{
    public override string Name => "express-supply-pass";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.ExpressSupplyPass = true;
        return (true, null);
    }
}

public sealed class HelpOption : CLIOption
{
    public override string Name => "help";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.Help = true;
        return (true, null);
    }
}

public sealed class LanguageOption : CLIOption
{
    public override string Name => "language";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.Language = source.Next();
        return (true, null);
    }
}

public sealed class NoRewardsOption : CLIOption
{
    public override string Name => "no-rewards";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.WarpCurrencyStats.NoWarpRewards = true;
        return (true, null);
    }
}

public sealed class NewWarpOption : CLIOption
{
    public override string Name => "new-warp";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.TryAddAndResetWarp();
        var name = source.Next();
        args.WarpArgs.WarpType = SRWarpTypes.FromeName(name);
        if (args.WarpArgs.WarpType is SRWarpType.None)
            return (false, $"无法识别的卡池类型 '{name}'");
        return (true, null);
    }
}

public sealed class CustomWarpOption : CLIOption
{
    public override string Name => "custom-warp";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.WarpArgs.WarpName = CLIArgs.CLIWarpArgs.CustomWarpName;
        if (args.WarpArgs.WarpType is SRWarpType.CharacterEventWarp)
        {
            args.WarpArgs.Up5 = source.NextWarpResultItem<SRStar5Character>();
            if (args.WarpArgs.Up5 is null)
                return (false, null);
            args.WarpArgs.Up41 = source.NextWarpResultItem<SRStar4Character>();
            if (args.WarpArgs.Up41 is null)
                return (false, null);
            args.WarpArgs.Up42 = source.NextWarpResultItem<SRStar4Character>();
            if (args.WarpArgs.Up42 is null)
                return (false, null);
            args.WarpArgs.Up43 = source.NextWarpResultItem<SRStar4Character>();
            if (args.WarpArgs.Up43 is null)
                return (false, null);
        }
        else if (args.WarpArgs.WarpType is SRWarpType.LightConeEventWarp)
        {
            args.WarpArgs.Up5 = source.NextWarpResultItem<SRStar5LightCone>();
            if (args.WarpArgs.Up5 is null)
                return (false, null);
            args.WarpArgs.Up41 = source.NextWarpResultItem<SRStar4LightCone>();
            if (args.WarpArgs.Up41 is null)
                return (false, null);
            args.WarpArgs.Up42 = source.NextWarpResultItem<SRStar4LightCone>();
            if (args.WarpArgs.Up42 is null)
                return (false, null);
            args.WarpArgs.Up43 = source.NextWarpResultItem<SRStar4LightCone>();
            if (args.WarpArgs.Up43 is null)
                return (false, null);
        }
        else
        {
            return (false, "未指定卡池类型，需先使用 '--new-warp' 指定卡池类型");
        }
        return (true, null);
    }
}

public sealed class EquilibriumLevelOption : CLIOption
{
    public override string Name => "equilibrium-level";

    public override (bool Applied, string? Message) TryApplyToCLIArgs(CLIArgs args, CLIArgsSource source)
    {
        args.EquilibriumLevel = source.NextInt32(0, 6);
        return (true, null);
    }
}
