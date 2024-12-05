﻿using Microsoft.CodeAnalysis;
using SREmulator.SourceGeneration.Receivers;
using System.Collections.Generic;
using System.Text;

namespace SREmulator.SourceGeneration
{
    [Generator(LanguageNames.CSharp)]
    public class SRLightConeGenerator : ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SRLightConeKeysClassReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (SRLightConeKeysClassReceiver)context.SyntaxContextReceiver;
            var semanticModel = context.Compilation.GetSemanticModel(receiver.Keys.SyntaxTree);
            var keys = Microsoft.CodeAnalysis.CSharp.CSharpExtensions.GetDeclaredSymbol(semanticModel, receiver.Keys);
            List<(string Key, int Rarity, bool Limited, string Type)> lightCones = new List<(string Key, int Rarity, bool Limited, string Type)>();

            foreach (var member in keys.GetMembers())
            {
                var attributeData = member.GetAttribute(receiver.Attribute);
                string key = (string)attributeData.ConstructorArguments[0].Value;
                int rarity = (int)attributeData.ConstructorArguments[1].Value;
                bool limited = (bool)attributeData.ConstructorArguments[2].Value;

                string type;
                if (rarity is 5) type = limited ? "SRLimitedStar5LightCone" : "SRNonLimitedStar5LightCone";
                else if (rarity is 4) type = "SRStar4LightCone";
                else type = "SRStar3LightCone";

                lightCones.Add((key, rarity, limited, type));
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("// <auto-generated/>");
            builder.AppendLine("namespace SREmulator.SRItems");
            builder.AppendLine("{");
            builder.AppendLine("    partial class SRLightCones");
            builder.AppendLine("    {");
            foreach (var lightCone in lightCones)
            {
                builder.AppendLine($"        private static {lightCone.Type} _{lightCone.Key} = null;");
                builder.AppendLine($"        public static {lightCone.Type} {lightCone.Key} => _{lightCone.Key} ??= new {lightCone.Key}();");
            }
            builder.AppendLine("    }");
            builder.AppendLine();

            foreach (var character in lightCones)
            {
                builder.AppendLine($"    public sealed record class {character.Key} : {character.Type}");
                builder.AppendLine("    {");
                builder.AppendLine($"        public override string Name => Localizations.Localization.{character.Key};");
                builder.AppendLine("    }");
                builder.AppendLine();
            }

            builder.AppendLine("}");

            context.AddSource("SRLightCones.g.cs", builder.ToString());
        }
    }
}
