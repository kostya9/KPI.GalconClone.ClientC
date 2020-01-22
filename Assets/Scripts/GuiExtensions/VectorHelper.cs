using UnityEngine;

namespace Assets.Scripts.GuiExtensions
{
    public static class VectorHelper
    {
        public static Vector3 To2DWorldPosition(Vector2 vector)
        {
            var pos = Camera.main.ScreenToWorldPoint(vector);
            return new Vector3(pos.x, pos.y, 0);
        }
    }
}
