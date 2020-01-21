using System.Linq;
using UnityEngine;

namespace KPI.GalconClone.ClientC
{
    public static class GameObjectExtensions
    {
        public static GameObject GetDirectChildByName(this GameObject go, string targetName)
        {
            foreach (var transform in go.GetComponentsInChildren<Transform>(true))
            {
                var child = transform.gameObject;
                if (child.name == targetName)
                {
                    return child;
                }
            }

            return null;
        }
    }
}