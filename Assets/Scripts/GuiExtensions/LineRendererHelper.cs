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

        // maybe will be deleted in future, cause there's no need in clicking on the unit
        public static void DrawTriangle(LineRenderer lineRenderer)
        {
            //Material lineMaterial = new Material(Shader.Find("Unlit/Transparent"));
            //lineRenderer.material = lineMaterial;
            lineRenderer.positionCount = 4;
            lineRenderer.startWidth = 1f;
            lineRenderer.endWidth = 1f;
            lineRenderer.SetPosition(0, new Vector3(0.0f, 0.0f, 0.0f));
            lineRenderer.SetPosition(1, new Vector3(5.0f, 5.0f, 5.0f));
            lineRenderer.SetPosition(2, new Vector3(7.0f, 2.0f, 6.0f));
            lineRenderer.SetPosition(3, new Vector3(7.0f, 2.0f, 6.0f));
            lineRenderer.useWorldSpace = false;
            lineRenderer.startColor = Color.white;
            lineRenderer.endColor = Color.white;
        }
    }
}
