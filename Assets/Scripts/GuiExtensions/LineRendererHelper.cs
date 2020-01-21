using UnityEngine;

namespace Assets.Scripts.GuiExtensions
{
    public static class LineRendererHelper
    {
        public static void DrawCircle(LineRenderer lineRenderer, float radius = 1, float lineWidth = 3, int vertexCount = 100)
        {
            lineRenderer.loop = true;
            lineRenderer.widthMultiplier = lineWidth;

            float deltaTheta = (2f * Mathf.PI) / vertexCount;
            float theta = 0f;

            lineRenderer.positionCount = vertexCount;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), -1f);
                lineRenderer.SetPosition(i, pos);
                theta += deltaTheta;
            }
        }
    }
}
