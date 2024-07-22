using System;
using UnityEngine;

public static class Utils
{
    public static Color orange = Color.red + Color.green / 2;

    [System.Serializable]
    public struct AgentColor
    {
        public string name;
        public Color color;

        public AgentColor(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }
    }

    private static double timestampThreshold = 5d;

    public static bool TimestampClose(double t1, double t2)
    {
        return Math.Abs(t2 - t1) < timestampThreshold;
    }
}
