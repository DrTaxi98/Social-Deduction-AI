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
}
