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
}
