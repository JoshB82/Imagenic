using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceBuildingHelpers
{
    public static class StringBuilderExtensions
    {
        public static int IndentationLevel { get; set; }

        // Namespaces
        public static StringBuilder BeginNamespace(this StringBuilder sb, string name)
        {
            sb.AppendLine($@"namespace {name}
{{");
            return sb;
        }

        public static StringBuilder EndNamespace(this StringBuilder sb)
        {
            sb.AppendLine("}");
            return sb;
        }

        // Classes
        public static StringBuilder BeginClass(this StringBuilder sb, ClassInfo ci)
        {
            sb.AppendLine($@"{ci.AccessibilityModifier} {ci.OtherModifier} class {ci.Name}
{{");
            return sb;
        }

        public static StringBuilder BeginAbstractClass(this StringBuilder sb, ClassInfo ci)
        {
            ci.OtherModifier = "abstract";
            return sb.BeginClass(ci);
        }

        public static StringBuilder BeginSealedClass(this StringBuilder sb, ClassInfo ci)
        {
            ci.OtherModifier = "sealed";
            return sb.BeginClass(ci);
        }

        public static StringBuilder EndClass(this StringBuilder sb)
        {
            sb.AppendLine("}");
            return sb;
        }

        // Methods
        public static StringBuilder BeginMethod(this StringBuilder sb, MethodInfo mi)
        {
            sb.AppendLine($@"{mi.AccessibilityModifier} {string.Join(" ", mi.OtherModifiers)} {mi.ReturnType} {mi.Name}({string.Join(", ", mi.Parameters.Select(p => $"{p.type} {p.name}"))})
{{");
            return sb;
        }

        public static StringBuilder BeginStaticMethod(this StringBuilder sb, MethodInfo mi)
        {
            mi.OtherModifiers = new[] { "static" };
            return sb.BeginMethod(mi);
        }

        public static StringBuilder BeginAbstractMethod(this StringBuilder sb, MethodInfo mi)
        {
            mi.OtherModifiers = new[] { "abstract" };
            return sb.BeginMethod(mi);
        }

        public static StringBuilder BeginVirtualMethod(this StringBuilder sb, MethodInfo mi)
        {
            mi.OtherModifiers = new[] { "virtual" };
            return sb.BeginMethod(mi);
        }

        public static StringBuilder BeginOverrideMethod(this StringBuilder sb, MethodInfo mi)
        {
            mi.OtherModifiers = new[] { "override" };
            return sb.BeginMethod(mi);
        }

        public static StringBuilder EndMethod(this StringBuilder sb)
        {
            sb.AppendLine("}");
            return sb;
        }

        // Properties
        public static StringBuilder AddProperty(this StringBuilder sb, PropertyInfo pi)
        {
            sb.AppendLine($"{pi.AccessibilityModifier} {string.Join(" ", pi.OtherModifiers)} {pi.Type} {pi.Name} {{ {string.Join(" ", pi.PropertyAccessors.Select(a => $"{(a.accessibilityModifier is null ? string.Empty : a.accessibilityModifier)} {a.name};"))} }}");
            return sb;
        }

        public static StringBuilder AddStaticProperty(this StringBuilder sb, PropertyInfo pi)
        {
            pi.OtherModifiers = new[] { "static" };
            return sb.AddProperty(pi);
        }

        public static StringBuilder AddAbstractProperty(this StringBuilder sb, PropertyInfo pi)
        {
            pi.OtherModifiers = new[] { "abstract" };
            return sb.AddProperty(pi);
        }

        public static StringBuilder AddVirtualProperty(this StringBuilder sb, PropertyInfo pi)
        {
            pi.OtherModifiers = new[] { "virtual" };
            return sb.AddProperty(pi);
        }

        public static StringBuilder AddOverrideProperty(this StringBuilder sb, PropertyInfo pi)
        {
            pi.OtherModifiers = new[] { "override" };
            return sb.AddProperty(pi);
        }
    }

    public class ClassInfo
    {
        public required string Name { get; set; }

        public string AccessibilityModifier { get; set; } = "public";
        public string OtherModifier { get; set; }

        public IEnumerable<string> TypeParameters { get; set; }
        public IEnumerable<string> GenericConstraints { get; set; }

        public string Inheritance { get; set; }
        public IEnumerable<string> ImplementedInterfaces { get; set; }
    }

    public class MethodInfo
    {
        public required string Name { get; set; }

        public required string ReturnType { get; set; }

        public string AccessibilityModifier { get; set; } = "public";
        public IEnumerable<string> OtherModifiers { get; set; }

        public IEnumerable<string> TypeParameters { get; set; }
        public IEnumerable<string> GenericConstraints { get; set; }

        public IEnumerable<(string type, string name)> Parameters { get; set; }
    }

    public class PropertyInfo
    {
        public required string Name { get; set; }

        public required string Type { get; set; }

        public string AccessibilityModifier { get; set; } = "public";
        public IEnumerable<string> OtherModifiers { get; set; }

        public IEnumerable<(string accessibilityModifier, string name)> PropertyAccessors { get; set; }
    }
}