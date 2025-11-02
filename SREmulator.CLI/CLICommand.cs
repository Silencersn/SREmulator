using SREmulator.SRItems;
using SREmulator.SRPlayers;
using SREmulator.SRWarps;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SREmulator.CLI
{
    public static class CLICommands
    {
        public static readonly Dictionary<string, CLICommand> Commands;

        static CLICommands()
        {
            Commands = [];

            AppendCommand<HelpCommand>();
            AppendCommand<ResultStatisticsCommand>();
            AppendCommand<AchieveAverageWarpsCommand>();
            AppendCommand<AchieveChanceCommand>();
            AppendCommand<AchieveDistributionCommand>();

            static void AppendCommand<T>() where T : CLICommand, new()
            {
                var command = new T();
                Commands.Add(command.Name, command);
            }
        }

        public static void TryExecute(CLIArgs args)
        {
            Commands.TryGetValue(args.Command, out var command);
            command ??= Commands["help"];
            command.Execute(args);
        }
    }

    public abstract class CLICommand
    {
        public abstract string Name { get; }

        public abstract void Execute(CLIArgs args);

        protected static string GetFileName(string name, string extension)
        {
            return $"{name}.{DateTime.Now:yyyy-MM-dd-HH-mm-ss-fffffff}.{extension}";
        }

        protected static ConsoleColor SetColor(ISRWarpResultItem item)
        {
            var origColor = Console.ForegroundColor;
            Console.ForegroundColor = item.Rarity switch
            {
                SRItemRarity.Star5 => ConsoleColor.Yellow,
                SRItemRarity.Star4 => ConsoleColor.Magenta,
                SRItemRarity.Star3 => ConsoleColor.Gray,
                _ => Console.ForegroundColor,
            };
            return origColor;
        }

        protected static void Print(ISRWarpResultItem item, string? info = null)
        {
            var origColor = SetColor(item);
            Console.Write(item.Name);
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(info ?? string.Empty);
            Console.ForegroundColor = origColor;
        }
    }

    public sealed class HelpCommand : CLICommand
    {
        public override string Name => "help";

        public override void Execute(CLIArgs args)
        {
            Console.WriteLine(CLI.Help);
        }
    }

    public sealed class ResultStatisticsCommand : CLICommand
    {
        public override string Name => "result-statistics";

        public override void Execute(CLIArgs args)
        {
            Dictionary<ISRWarpResultItem, int> result = [];
            int maxNameLength = 0;
            int counter = 0;

            if (args.Warps.Count is 0) return;
            var warp = args.Warps[0];
            var player = args.CreatePlayer();
            StringBuilder builder = args.Export ? new StringBuilder() : null!;
            if (args.Export) builder.AppendLine("对象类型,对象名称,对象星级,跃迁类型");

            string warpTypeName = warp.WarpType switch
            {
                SRWarpType.DepartureWarp => "始发跃迁",
                SRWarpType.StellarWarp => "群星跃迁",
                SRWarpType.CharacterEventWarp => "角色活动跃迁",
                SRWarpType.LightConeEventWarp => "光锥活动跃迁",
                _ => string.Empty
            };

            while (warp.TryWarp(player, out var item))
            {
                result.TryGetValue(item, out var count);
                result[item] = count + 1;
                counter++;

                if (args.Export)
                {
                    builder.AppendLine($"{(item is SRCharacter ? "角色" : "光锥")}," +
                        $"{item.Name}," +
                        $"{(int)item.Rarity}," +
                        $"{warpTypeName}"
                        );
                }

                if (args.Silent) continue;

                var origColor = SetColor(item);

                maxNameLength = Math.Max(maxNameLength, item.Name.Length);

                if (args.Return)
                {
                    if (counter > 1)
                    {
                        Console.CursorLeft = 0;
                        Console.CursorTop--;
                    }
                    Console.WriteLine(item.Name.PadRight(maxNameLength * 2));
                }
                else
                {
                    Console.WriteLine(item.Name);
                }

                Console.ForegroundColor = origColor;

                if (args.Pause) _ = Console.ReadKey(true);
            }

            if (args.Export) File.WriteAllText(GetFileName("warps", "csv"), builder.ToString());

            builder = args.Output ? new StringBuilder() : null!;
            if (args.Output) builder.AppendLine("名称,数量,占比");

            Console.WriteLine("----------");
            Console.WriteLine(counter);
            foreach (var pair in result.OrderByDescending(pair => pair.Key.Rarity).ThenByDescending(pair => pair.Value))
            {
                Print(pair.Key, $"\n{pair.Value}\t({(double)pair.Value / counter:0.00%})");
                if (args.Output) builder.AppendLine($"{pair.Key},{pair.Value},{(double)pair.Value / counter:0.00%}");
            }

            if (args.Output) File.WriteAllText(GetFileName(Name, "csv"), builder.ToString());
        }
    }

    public sealed class AchieveAverageWarpsCommand : CLICommand
    {
        public override string Name => "achieve-average-warps";

        public override void Execute(CLIArgs args)
        {
            int total = args.Attempts;
            ulong warps = 0;
            int successful = 0;

            Parallel.For(0, total, _ =>
            {
                var player = args.CreatePlayer();
                player.WarpCurrencyStats.NoWarpRewards = true;
                player.WarpCurrencyStats.UnlimitedResources = true;
                var target = args.Targets.Create();
                ulong counter = 0;

                foreach (var warp in args.Warps)
                {
                    while (!target.TryRemoveWarp(warp) && warp.TryWarp(player, out var item))
                    {
                        counter++;
                        target.Check(item);
                    }
                }

                if (target.Achieved) Interlocked.Increment(ref successful);
                Interlocked.Add(ref warps, counter);
            });

            foreach (var pair in args.Targets.TargetToCount)
            {
                Print(pair.Key, $": {pair.Value}");
            }
            var resultChance = (double)successful / total;
            if (resultChance is not 1)
                Console.WriteLine(resultChance.ToString("0.00%"));

            var result = ((double)warps / total).ToString("0.##");
            Console.WriteLine(result);
            if (args.Output) File.WriteAllText(GetFileName(Name, "txt"), result);
        }
    }

    public sealed class AchieveChanceCommand : CLICommand
    {
        public override string Name => "achieve-chance";

        public override void Execute(CLIArgs args)
        {
            int total = args.Attempts;
            int successful = 0;

            Parallel.For(0, total, _ =>
            {
                var player = args.CreatePlayer();
                var target = args.Targets.Create();

                foreach (var warp in args.Warps)
                {
                    while (!target.TryRemoveWarp(warp) && warp.TryWarp(player, out var item))
                    {
                        target.Check(item);
                    }
                }
                if (target.Achieved) Interlocked.Increment(ref successful);
            });

            foreach (var pair in args.Targets.TargetToCount)
            {
                Print(pair.Key, $": {pair.Value}");
            }
            var result = ((double)successful / total).ToString("0.00%");
            Console.WriteLine(result);
            if (args.Output) File.WriteAllText(GetFileName(Name, "txt"), result);
        }
    }

    public sealed class AchieveDistributionCommand : CLICommand
    {
        public override string Name => "achieve-distribution";

        private static readonly object _lock = new();
        public override void Execute(CLIArgs args)
        {
            int total = args.Attempts;
            Dictionary<IReadOnlyDictionary<ISRWarpResultItem, int>, int> distribution = new(DictComparer.Instance);

            Parallel.For(0, total, _ =>
            {
                var player = args.CreatePlayer();
                var target = args.Targets.Create();

                foreach (var warp in args.Warps)
                {
                    while (!target.TryRemoveWarp(warp) && warp.TryWarp(player, out var item))
                    {
                        target.Check(item);
                    }
                }
                lock (_lock)
                {
                    distribution.TryGetValue(target.Counter, out int count);
                    distribution[target.Counter] = count + 1;
                }
            });

            var targetToCount = args.Targets.TargetToCount;
            int sum = distribution.Values.Sum();
            StringBuilder builder = args.Output ? new() : null!;

            foreach (var result in distribution.OrderByDescending(pair => pair.Value))
            {
                foreach (var target in args.Targets.OrderedTargets)
                {
                    var count = result.Key[target];
                    Print(target, $": {count}");
                    if (args.Output) builder.Append($"\"{target.Name}\",{count},");
                }
                var ratio = ((double)result.Value / sum).ToString("0.00%");
                Console.WriteLine(ratio);
                Console.WriteLine();
                if (args.Output) builder.AppendLine($"{ratio}");
            }

            if (args.Output) File.WriteAllText(GetFileName(Name, "csv"), builder.ToString());
        }

        private sealed class DictComparer : IEqualityComparer<IReadOnlyDictionary<ISRWarpResultItem, int>>
        {
            public static IEqualityComparer<IReadOnlyDictionary<ISRWarpResultItem, int>> Instance { get; } = new DictComparer();

            bool IEqualityComparer<IReadOnlyDictionary<ISRWarpResultItem, int>>.Equals(IReadOnlyDictionary<ISRWarpResultItem, int>? x, IReadOnlyDictionary<ISRWarpResultItem, int>? y)
            {
                if (x is null) return y is null;
                if (y is null) return false;
                if (x.Count != y.Count) return false;
                foreach (var pair in x)
                {
                    if (!y.TryGetValue(pair.Key, out var value)) return false;
                    if (pair.Value != value) return false;
                }
                return true;
            }

            int IEqualityComparer<IReadOnlyDictionary<ISRWarpResultItem, int>>.GetHashCode([DisallowNull] IReadOnlyDictionary<ISRWarpResultItem, int> obj)
            {
                int hash = 0;
                foreach (var pair in obj)
                {
                    hash = HashCode.Combine(hash, pair);
                }
                return hash;
            }
        }
    }
}
