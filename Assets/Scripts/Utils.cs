using UnityEngine;

public static class Utils
{
    public static Color orange = Color.red + Color.green / 2;
    public static Color opaque = new Color(0, 0, 0, 0.8f);

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
