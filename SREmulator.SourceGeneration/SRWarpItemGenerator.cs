﻿using Microsoft.CodeAnalysis;
using SREmulator.SourceGeneration.Constants;
using System.Collections.Generic;
using System.Text;

namespace SREmulator.SourceGeneration
{
    [Generator(LanguageNames.CSharp)]
    public sealed class SRCharacterGenerator : SRWarpItemGenerator
    {
        public override string Keys => SRKeys.SRCharacterKeys;
        public override string Type => SRItemTypes.Character;
        public override string Attribute => SRAttributes.SRCharacterAttribute;
        public override int ID => SRIDs.CharacterID;

        public override string GetClassType(int rarity, bool limited)
        {
            if (rarity is 5) return limited ? "SRLimitedStar5Character" : "SRNonLimitedStar5Character";
            return "SRStar4Character";
        }
    }

    [Generator(LanguageNames.CSharp)]
    public sealed class SRLightConeGenerator : SRWarpItemGenerator
    {
        public override string Keys => SRKeys.SRLightConeKeys;
        public override string Type => SRItemTypes.LightCone;
        public override string Attribute => SRAttributes.SRLightConeAttribute;
        public override int ID => SRIDs.LightConeID;

        public override string GetClassType(int rarity, bool limited)
        {
            if (rarity is 5) return limited ? "SRLimitedStar5LightCone" : "SRNonLimitedStar5LightCone";
            if (rarity is 4) return "SRStar4LightCone";
            return "SRStar3LightCone";
        }
    }

    public abstract class SRWarpItemGenerator : ISourceGenerator
    {
        public abstract string Keys { get; }
        public abstract string Type { get; }
        public abstract string Attribute { get; }
        public abstract int ID { get; }

        public abstract string GetClassType(int rarity, bool limited);

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(SRKeysClassReceiver.Creator(Keys));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var receiver = (SRKeysClassReceiver)context.SyntaxContextReceiver;
            var semanticModel = context.Compilation.GetSemanticModel(receiver.Keys.SyntaxTree);
            var keys = Microsoft.CodeAnalysis.CSharp.CSharpExtensions.GetDeclaredSymbol(semanticModel, receiver.Keys);
            List<(string Key, int Rarity, bool Limited, string Type)> items = new List<(string Key, int Rarity, bool Limited, string Type)>();

            foreach (var member in keys.GetMembers())
            {
                var attributeData = member.GetAttribute(Attribute);

                (string Key, int Rarity, bool Limited, string Type) item = default;
                attributeData.Deconstruct(
                    out item.Key,
                    out item.Rarity,
                    out item.Limited
                    );
                item.Type = GetClassType(item.Rarity, item.Limited);

                items.Add(item);
            }

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("// <auto-generated/>");
            builder.AppendLine("namespace SREmulator.SRItems");
            builder.AppendLine("{");
            builder.AppendLine(1, $"partial class SR{Type}s");
            builder.AppendLine(1, "{");
            foreach (var item in items)
            {
                builder.AppendLine(2, $"private static {item.Type} _{item.Key} = null;");
                builder.AppendLine(2, $"public static {item.Type} {item.Key} => _{item.Key} ??= new {item.Key}();");
            }
            builder.AppendLine(1, "}");
            builder.AppendLine();

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                builder.AppendLine(1, $"public sealed class {item.Key} : {item.Type}");
                builder.AppendLine(1, "{");
                builder.AppendLine(2, $"public override string Name => Localizations.Localization.{item.Key};");
                builder.AppendLine(2, $"public override int Id => {ID + i + 1};");
                builder.AppendLine(1, "}");
                builder.AppendLine();
            }

            builder.AppendLine("}");

            context.AddSource($"SR{Type}s.g.cs", builder.ToString());
        }
    }
}
