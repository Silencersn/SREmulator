using SREmulator.SRItems;
using SREmulator.SRWarps;

namespace SREmulator.CLI
{
    public interface ICLIWarpTarget
    {
        public bool Achieved { get; }
        public IReadOnlyDictionary<ISRWarpResultItem, int> Counter { get; }
        public void Check(ISRWarpResultItem item);
        public ICLIWarpTarget Clone();
        public bool TryRemoveWarp(SRWarp warp);
    }

    public sealed class CLIWarpTargetFactory
    {
        private readonly Dictionary<ISRWarpResultItem, int> _targetsToCount = [];
        private readonly List<ISRWarpResultItem> _orderedTargets = [];
        private readonly List<ISRWarpResultItem> _invalidTargets = [];
        private readonly CLIArgs _args;
        private ICLIWarpTarget? _target;
        private Dictionary<ISRWarpResultItem, HashSet<SRWarp>>? _bestWarpsOfItem;

        public IReadOnlyDictionary<ISRWarpResultItem, int> TargetToCount => _targetsToCount;
        public IReadOnlyList<ISRWarpResultItem> OrderedTargets => _orderedTargets;
        public IReadOnlyList<ISRWarpResultItem> InvalidTargets => _invalidTargets;
        public Dictionary<ISRWarpResultItem, HashSet<SRWarp>> BestWarpsOfItem => _bestWarpsOfItem ??= BuildBestWarps();

        public CLIWarpTargetFactory(CLIArgs args)
        {
            _args = args;
        }

        public void AppendTarget(ISRWarpResultItem item, int count)
        {
            ArgumentNullException.ThrowIfNull(item);

            if (_bestWarpsOfItem is not null)
                throw new InvalidOperationException();

            if (count <= 0)
                return;

            if (_targetsToCount.ContainsKey(item))
                _orderedTargets.Remove(item);

            _targetsToCount[item] = count;
            _orderedTargets.Add(item);
        }

        private IEnumerable<(SRWarp Warp, int Weight)> GetWarpWithWeight(ISRWarpResultItem item)
        {
            return _args.Warps.Select(warp =>
            {
                int weight;
                if (warp.AvailableUpItems.Contains(item))
                {
                    weight = 100;
                }
                else if (warp.AvailableItems.Contains(item))
                {
                    if (item.Rarity is SRItemRarity.Star3)
                    {
                        weight = 1;
                    }
                    else if (warp.WarpType is SRWarpType.StellarWarp or SRWarpType.DepartureWarp)
                    {
                        weight = 60;
                    }
                    else if (item is SRCharacter && warp.WarpType is SRWarpType.LightConeEventWarp)
                    {
                        weight = 50 - warp.Common4Characters.Length;
                    }
                    else if (item is SRLightCone && warp.WarpType is SRWarpType.CharacterEventWarp)
                    {
                        weight = 50 - warp.Common4LightCones.Length;
                    }
                    else
                    {
                        weight = 10;
                    }
                }
                else
                {
                    weight = 0;
                }
                return (warp, weight);
            });
        }

        private Dictionary<ISRWarpResultItem, HashSet<SRWarp>> BuildBestWarps()
        {
            Dictionary<ISRWarpResultItem, HashSet<SRWarp>> bestWarps = [];
            foreach (var item in _targetsToCount.Keys)
            {
                var best = GetWarpWithWeight(item)
                    .ToLookup(warpWithWeight => warpWithWeight.Weight)
                    .OrderByDescending(group => group.Key)
                    .FirstOrDefault(group => group.Key > 0)?
                    .Select(tuple => tuple.Warp)
                    .ToHashSet();
                if (best is null)
                {
                    _invalidTargets.Add(item);
                    continue;
                }
                bestWarps[item] = best;
            }
            foreach (var invalidTarget in _invalidTargets)
            {
                _targetsToCount.Remove(invalidTarget);
                _orderedTargets.Remove(invalidTarget);
            }
            return bestWarps;
        }

        public ICLIWarpTarget Create()
        {
            if (_target is not null)
                return _target.Clone();

            _bestWarpsOfItem = BuildBestWarps();
            if (_bestWarpsOfItem.Count is 1)
            {
                var pair = _bestWarpsOfItem.First();
                _target = new CLISingleTargetTarget(pair.Key, pair.Value.First(), _targetsToCount[pair.Key]);
            }
            else
            {
                _target = new CLIMultipleTargetsMultipleWarpsTarget(this);
            }
            return _target.Clone();
        }
    }

    internal sealed class CLIMultipleTargetsMultipleWarpsTarget : ICLIWarpTarget
    {
        private readonly CLIWarpTargetFactory _factory;
        private readonly Dictionary<ISRWarpResultItem, int> _targetsCounter;
        private readonly Dictionary<ISRWarpResultItem, HashSet<SRWarp>> _bestWarpsOfItem;
        private readonly bool _noStar3;
        private readonly bool _noStar4;
        private int _targetsCount;
        private bool _achieved;

        public bool Achieved => _achieved;

        public IReadOnlyDictionary<ISRWarpResultItem, int> Counter => _targetsCounter;

        internal CLIMultipleTargetsMultipleWarpsTarget(CLIWarpTargetFactory factory)
        {
            _factory = factory;
            _targetsCounter = _factory.OrderedTargets.ToDictionary(static target => target, _ => 0);
            _targetsCount = _targetsCounter.Count;
            _bestWarpsOfItem = new(_factory.BestWarpsOfItem);
            _noStar3 = _targetsCounter.Keys.All(item => item.Rarity is not SRItemRarity.Star3);
            _noStar4 = _targetsCounter.Keys.All(item => item.Rarity is not SRItemRarity.Star4);
            _achieved = _targetsCounter.Count is 0;
        }

        public void Check(ISRWarpResultItem item)
        {
            if (_achieved)
                return;

            if (_noStar3 && item.Rarity is SRItemRarity.Star3)
                return;

            if (_noStar4 && item.Rarity is SRItemRarity.Star4)
                return;

            if (!_targetsCounter.TryGetValue(item, out var count))
                return;

            _targetsCounter[item] = ++count;

            if (count >= _factory.TargetToCount[item] && _bestWarpsOfItem.Remove(item))
            {
                if (--_targetsCount is 0)
                    _achieved = true;
            }
        }

        public ICLIWarpTarget Clone()
        {
            return new CLIMultipleTargetsMultipleWarpsTarget(_factory);
        }

        public bool TryRemoveWarp(SRWarp warp)
        {
            bool ret = _bestWarpsOfItem.Values.All(warps => warps.Count > 1 || !warps.Contains(warp));
            if (ret)
            {
                foreach (var warps in _bestWarpsOfItem.Values)
                {
                    warps.Remove(warp);
                }
            }
            return ret;
        }
    }

    internal sealed class CLISingleTargetTarget : ICLIWarpTarget
    {
        private readonly ISRWarpResultItem _target;
        private readonly SRWarp _warp;
        private readonly int _targetCount;
        private readonly Dictionary<ISRWarpResultItem, int> _counter;
        public bool Achieved => _counter[_target] >= _targetCount;

        public IReadOnlyDictionary<ISRWarpResultItem, int> Counter => _counter;

        internal CLISingleTargetTarget(ISRWarpResultItem target, SRWarp warp, int targetCount)
        {
            _target = target;
            _warp = warp;
            _targetCount = targetCount;
            _counter = [];
            _counter[_target] = 0;
        }

        public void Check(ISRWarpResultItem item)
        {
            if (item == _target)
                _counter[_target]++;
        }

        public ICLIWarpTarget Clone()
        {
            return new CLISingleTargetTarget(_target, _warp, _targetCount);
        }

        public bool TryRemoveWarp(SRWarp warp)
        {
            return Achieved;
        }
    }
}
