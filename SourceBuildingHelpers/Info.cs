using System.Collections.Generic;

namespace SourceBuildingHelpers;

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