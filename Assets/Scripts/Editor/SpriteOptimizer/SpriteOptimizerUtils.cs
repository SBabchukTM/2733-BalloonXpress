using UnityEngine;

namespace Editor.SpriteOptimizer
{
    public static class SpriteOptimizerUtils
    {
        public static GUIStyle SetButtonStyle()
        {
            var buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 14,
                fontStyle = FontStyle.Bold,
                normal = {textColor = new Color(0.35f, 0.9f, 0.11f)},
                hover = {textColor = new Color(0.07f, 1f, 0.58f)},
                active = {textColor = new Color(0.18f, 0.5f, 0.4f)},
                padding = new RectOffset(20, 20, 10, 10),
                margin = new RectOffset(10, 10, 10, 10)
            };

            if (Event.current.type == EventType.Repaint)
            {
                buttonStyle.hover.background = Texture2D.grayTexture;
                buttonStyle.hover.textColor = new Color(0.84f, 0.9f, 0.03f);
            }

            return buttonStyle;
        }
    }
}