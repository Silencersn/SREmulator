﻿using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SREmulator.SourceGeneration
{
    [Generator(LanguageNames.CSharp)]
    public class SRLightConeEventWarpGenerator : ISourceGenerator
    {
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
                WarpsData warpsData = new WarpsData();
                var aliases = member.GetAttribute(SRAttributes.SRAliasesAttribute).ConstructorArguments[0].Values;
                warpsData.Names = aliases.Select(item => (string)item.Value).ToArray();
                List<WarpData> warps = new List<WarpData>();

                var attributeDatas = member.GetAttributes(SRAttributes.SRLightConeEventWarpAttribute);

                foreach (var attributeData in attributeDatas)
                {
                    string key = (string)attributeData.ConstructorArguments[0].Value;
                    int index = (int)attributeData.ConstructorArguments[1].Value;
                    int major = (int)attributeData.ConstructorArguments[2].Value;
                    int minor = (int)attributeData.ConstructorArguments[3].Value;
                    string up5 = (string)attributeData.ConstructorArguments[4].Value;
                    string up41 = (string)attributeData.ConstructorArguments[5].Value;
                    string up42 = (string)attributeData.ConstructorArguments[6].Value;
                    string up43 = (string)attributeData.ConstructorArguments[7].Value;

                    WarpData data = new WarpData()
                    {
                        Key = key,
                        Index = index,
                        Major = major,
                        Minor = minor,
                        Up5 = up5,
                        Up41 = up41,
                        Up42 = up42,
                        Up43 = up43,
                    };

                    warps.Add(data);
                }
                warpsData.WarpDatas = warps.ToArray();
                warpsDatas.Add(warpsData);
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("// <auto-generated/>");
            builder.AppendLine("using SREmulator.SRItems;");
            builder.AppendLine("namespace SREmulator.SRWarps.LightConeEventWarps");
            builder.AppendLine("{");
            builder.AppendLine(1, "partial class SRLightConeEventWarps");
            builder.AppendLine(1, "{");
            foreach (var warp in warpsDatas.SelectMany(warpsData => warpsData.WarpDatas))
            {
                builder.AppendLine(2, $"private static SRLightConeEventWarp _{warp.Key}{warp.Index} = null;");
                builder.AppendLine(2, $"public static SRLightConeEventWarp {warp.Key}{warp.Index} => _{warp.Key}{warp.Index} ??= new {warp.Key}{warp.Index}();");
            }
            builder.AppendLine(2, "public static SRLightConeEventWarp GetWarpByNameAndVersion(string name, SRVersion version)");
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
                    builder.AppendLine(5, $"SRVersion.Ver{warpData.Major}p{warpData.Minor} => SRLightConeEventWarps.{warpData.Key}{warpData.Index},");
                }
                builder.AppendLine(5, $"_ => SRLightConeEventWarps.{warpsData.WarpDatas[0].Key}{warpsData.WarpDatas[0].Index}");
                builder.AppendLine(4, "},");
            }
            builder.AppendLine(4, $"_ => SRLightConeEventWarps.{warpsDatas.Last().WarpDatas[0].Key}{warpsDatas.Last().WarpDatas[0].Index}");
            builder.AppendLine(3, "};");

            builder.AppendLine(2, "}");
            builder.AppendLine(1, "}");
            builder.AppendLine();

            foreach (var warp in warpsDatas.SelectMany(warpsData => warpsData.WarpDatas))
            {
                builder.AppendLine(1, $"public sealed class {warp.Key}{warp.Index} : SRLightConeEventWarp");
                builder.AppendLine(1, "{");
                builder.AppendLine(2, $"public override SRVersion Version => SRVersion.Ver{warp.Major}p{warp.Minor};");
                builder.AppendLine(2, $"public override SRStar5LightCone UpStar5LightCone => SRLightCones.{warp.Up5};");
                builder.AppendLine(2, $"public override SRStar4LightCone UpStar4LightCone1 => SRLightCones.{warp.Up41};");
                builder.AppendLine(2, $"public override SRStar4LightCone UpStar4LightCone2 => SRLightCones.{warp.Up42};");
                builder.AppendLine(2, $"public override SRStar4LightCone UpStar4LightCone3 => SRLightCones.{warp.Up43};");
                builder.AppendLine(1, "}");
                builder.AppendLine();
            }

            builder.AppendLine("}");

            context.AddSource("SRLightConeEventWarps.g.cs", builder.ToString());
        }
    }
}
