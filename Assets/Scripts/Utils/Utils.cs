using System;
using UnityEngine;

public static class Utils
{
    private const double TIMESTAMPS_CLOSENESS_THRESHOLD = 5d;

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

    public static bool AreTimestampsClose(double t1, double t2)
    {
        return Math.Abs(t2 - t1) < TIMESTAMPS_CLOSENESS_THRESHOLD;
    }
}
