using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GUIExtensions
{
    public static class LabelHelper
    {
        public static void DrawOutline(Rect rect, string text, GUIStyle style, Color outColor, Color inColor, float size)
        {
            float halfSize = size * 0.5F;
            GUIStyle backupStyle = new GUIStyle(style);
            Color backupColor = GUI.color;

            style.normal.textColor = outColor;
            GUI.color = outColor;

            rect.x -= halfSize;
            GUI.Label(rect, text, style);

            rect.x += size;
            GUI.Label(rect, text, style);

            rect.x -= halfSize;
            rect.y -= halfSize;
            GUI.Label(rect, text, style);

            rect.y += size;
            GUI.Label(rect, text, style);

            rect.y -= halfSize;
            style.normal.textColor = inColor;
            GUI.color = backupColor;
            GUI.Label(rect, text, style);

            style = backupStyle;
        }
    }
}
