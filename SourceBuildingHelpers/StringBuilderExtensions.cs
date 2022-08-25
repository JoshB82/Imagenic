using System.Linq;
using System.Text;

namespace SourceBuildingHelpers;

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
        var otherModifierFormatted = ci.OtherModifier is null
                                   ? string.Empty
                                   : $" {ci.OtherModifier}";

        var typeParametersFormatted = ci.TypeParameters is null
                                    ? string.Empty
                                    : $"<{string.Join(", ", ci.TypeParameters)}>";

        var classesAndInterfacesFormatted = ci.ImplementedInterfaces is null && ci.Inheritance is null
                                          ? string.Empty
                                          : $" : {(ci.Inheritance is null ? string.Empty : $"{ci.Inheritance}, ")} {(ci.ImplementedInterfaces is null ? string.Empty : string.Join(", ", ci.ImplementedInterfaces))}";

        var genericConstraintsFormatted = ci.GenericConstraints is null
                                        ? string.Empty
                                        : $" {string.Join("where ", ci.GenericConstraints)}";

        sb.AppendLine($@"{ci.AccessibilityModifier}{otherModifierFormatted} class {ci.Name}{typeParametersFormatted}{classesAndInterfacesFormatted}{genericConstraintsFormatted}
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
        var otherModifiersFormatted = mi.OtherModifiers is null
                                    ? string.Empty
                                    : $" {string.Join(" ", mi.OtherModifiers)}";

        var typeParametersFormatted = mi.TypeParameters is null
                                    ? string.Empty
                                    : $"<{string.Join(", ", mi.TypeParameters)}>";

        var parametersFormatted = mi.Parameters is null
                                ? string.Empty
                                : string.Join(", ", mi.Parameters.Select(p => $"{p.type} {p.name}"));

        var genericConstraintsFormatted = mi.GenericConstraints is null
                                        ? string.Empty
                                        : $" {string.Join("where ", mi.GenericConstraints)}";

        sb.AppendLine($@"{mi.AccessibilityModifier}{otherModifiersFormatted} {mi.ReturnType} {mi.Name}{typeParametersFormatted}({parametersFormatted}){genericConstraintsFormatted}
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