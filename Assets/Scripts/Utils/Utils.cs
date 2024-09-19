using System;
using UnityEngine;

public static class Utils
{
    public static Color Orange { get; } = Color.red + Color.green / 2;

    [Serializable]
    public struct NameColor
    {
        public string name;
        public Color color;

        public NameColor(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }
    }

    public static string ReplaceString(string oldString, string oldValue, string newValue)
    {
        int index = oldString.IndexOf(oldValue);
        string newString = oldString.Substring(0, index) + newValue +
            oldString.Substring(index + oldValue.Length);
        return newString;
    }
}
