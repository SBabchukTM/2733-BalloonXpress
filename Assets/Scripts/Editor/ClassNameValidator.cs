using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class ClassNameValidator
{
    private static readonly Regex IdentifierRegex = new Regex(@"^[A-Z][A-Za-z0-9]*$");

    public static bool IsValid(string className)
    {
        if (string.IsNullOrEmpty(className))
        {
            Debug.LogError("Class name cannot be empty.");
            return false;
        }

        if (!IdentifierRegex.IsMatch(className))
        {
            Debug.LogError("Class name must start with an uppercase letter and contain only letters and numbers.");
            return false;
        }

        if (!CodeDomProvider.CreateProvider("C#").IsValidIdentifier(className))
        {
            Debug.LogError("Class name is a reserved keyword.");
            return false;
        }

        if (AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetTypes().Any(t => t.Name == className)))
        {
            Debug.LogError($"A class with the name '{className}' already exists.");
            return false;
        }

        return true;
    }
}
