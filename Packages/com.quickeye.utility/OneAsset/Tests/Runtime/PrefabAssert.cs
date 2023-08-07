using NUnit.Framework;
using UnityEngine;

namespace OneAsset.Tests
{
    public static class GameObjectAssert
    {
        public static void IsPrefabInstance(GameObject gameObject)
        {
            // Look for a component that was added in editor
            Assert.NotNull(gameObject.GetComponent<MeshFilter>());
        }
    }
}