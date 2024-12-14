﻿using Microsoft.CodeAnalysis;
using SREmulator.SourceGeneration.Constants;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SREmulator.SourceGeneration
{
    [Generator(LanguageNames.CSharp)]
    public sealed class SRCharacterEventWarpGenerator : SREventWarpGenerator
    {
        public override string Type => SRItemTypes.Character;
        public override string Attribute => SRAttributes.SRCharacterEventWarpAttribute;
    }

    [Generator(LanguageNames.CSharp)]
    public sealed class SRLightConeEventWarpGenerator : SREventWarpGenerator
    {
        public override string Type => SRItemTypes.LightCone;
        public override string Attribute => SRAttributes.SRLightConeEventWarpAttribute;
    }

    public abstract class SREventWarpGenerator : ISourceGenerator
    {
        public abstract string Type { get; }
        public abstract string Attribute { get; }

        private struct WarpsData
        {
            public string[] Names;
            public WarpData[] WarpDatas;
        }
        private struct WarpData
        {
            public string Key;
            public int Index;
            public int Major;
            public int Minor;
            public string Up5;
            public string Up41;
            public string Up42;
            public string Up43;

            public string WarpName => $"{Key}{Index}";
            public string Version => $"SRVersion.Ver{Major}p{Minor}";
        }

        private string GetClassName(WarpData warpData)
        {
            return $"{Type}_{warpData.WarpName}";
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(SRKeysClassReceiver.Creator(SRKeys.SREventWarpKeys));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (SRKeysClassReceiver)context.SyntaxContextReceiver;
            var semanticModel = context.Compilation.GetSemanticModel(receiver.Keys.SyntaxTree);
            var keys = Microsoft.CodeAnalysis.CSharp.CSharpExtensions.GetDeclaredSymbol(semanticModel, receiver.Keys);
            List<WarpsData> warpsDatas = new List<WarpsData>();

            foreach (var member in keys.GetMembers())
            {
                if (!member.ContainsAttribute(SRAttributes.SRAliasesAttribute)) continue;

                WarpsData warpsData = default;
                member.GetAttribute(SRAttributes.SRAliasesAttribute).Deconstruct(0, out warpsData.Names);

                List<WarpData> warps = new List<WarpData>();
                foreach (var attributeData in member.GetAttributes(Attribute))
                {
                    WarpData data = default;
                    attributeData.Deconstruct(0, out data.Key);
                    attributeData.Deconstruct(1, out data.Index);
                    attributeData.Deconstruct(2, out data.Major);
                    attributeData.Deconstruct(3, out data.Minor);
                    attributeData.Deconstruct(4, out data.Up5);
                    attributeData.Deconstruct(5, out data.Up41);
                    attributeData.Deconstruct(6, out data.Up42);
                    attributeData.Deconstruct(7, out data.Up43);

                    warps.Add(data);
                }
                warpsData.WarpDatas = warps.ToArray();

                warpsDatas.Add(warpsData);
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("// <auto-generated/>");
            builder.AppendLine("#nullable enable");
            builder.AppendLine("using SREmulator.SRItems;");
            builder.AppendLine("namespace SREmulator.SRWarps.EventWarps");
            builder.AppendLine("{");
            builder.AppendLine(1, $"partial class SR{Type}EventWarps");
            builder.AppendLine(1, "{");
            foreach (var warp in warpsDatas.SelectMany(warpsData => warpsData.WarpDatas))
            {
                builder.AppendLine(2, $"private static SR{Type}EventWarp _{warp.WarpName} = null;");
                builder.AppendLine(2, $"public static SR{Type}EventWarp {warp.WarpName} => _{warp.WarpName} ??= new {GetClassName(warp)}();");
            }
            builder.AppendLine(2, $"public static partial SR{Type}EventWarp? GetWarpByNameAndVersion(string name, SRVersion version)");
            builder.AppendLine(2, "{");
            builder.AppendLine(3, "return name.ToLower() switch");
            builder.AppendLine(3, "{");
            foreach (var warpsData in warpsDatas)
            {
                builder.AppendLine(4, $"\"{string.Join("\" or \"", warpsData.Names)}\"");
                builder.AppendLine(4, "=> version switch");
                builder.AppendLine(4, "{");
                foreach (var warpData in warpsData.WarpDatas)
                {
                    builder.AppendLine(5, $"{warpData.Version} => SR{Type}EventWarps.{warpData.WarpName},");
                }
                builder.AppendLine(5, $"_ => SR{Type}EventWarps.{warpsData.WarpDatas[0].WarpName}");
                builder.AppendLine(4, "},");
            }
            //builder.AppendLine(4, $"_ => SR{Type}EventWarps.{warpsDatas.Last().WarpDatas[0].WarpName}");
            builder.AppendLine(4, $"_ => null");
            builder.AppendLine(3, "};");

            builder.AppendLine(2, "}");
            builder.AppendLine(1, "}");
            builder.AppendLine();

            foreach (var warp in warpsDatas.SelectMany(warpsData => warpsData.WarpDatas))
            {
                builder.AppendLine(1, $"public sealed class {GetClassName(warp)} : SR{Type}EventWarp");
                builder.AppendLine(1, "{");
                builder.AppendLine(2, $"public override SRVersion Version => {warp.Version};");
                builder.AppendLine(2, $"public override SRStar5{Type} UpStar5{Type} => SR{Type}s.{warp.Up5};");
                builder.AppendLine(2, $"public override SRStar4{Type} UpStar4{Type}1 => SR{Type}s.{warp.Up41};");
                builder.AppendLine(2, $"public override SRStar4{Type} UpStar4{Type}2 => SR{Type}s.{warp.Up42};");
                builder.AppendLine(2, $"public override SRStar4{Type} UpStar4{Type}3 => SR{Type}s.{warp.Up43};");
                builder.AppendLine(1, "}");
                builder.AppendLine();
            }

            builder.AppendLine("}");

            context.AddSource($"SR{Type}EventWarps.g.cs", builder.ToString());
        }
    }
}
