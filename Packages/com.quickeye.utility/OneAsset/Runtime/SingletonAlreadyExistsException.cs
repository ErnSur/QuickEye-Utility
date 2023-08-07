using System;
using UnityEngine;

namespace OneAsset
{
    public class SingletonAlreadyExistsException : Exception
    {
        internal SingletonAlreadyExistsException(OneGameObject obj) : base(
            $"Singleton of type {obj.GetType()} already exists. Destroying \"{GetGameObjectPath(obj.gameObject)}\"")
        {
        }

        private static string GetGameObjectPath(GameObject obj)
        {
            var path = $"/{obj.name}";
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = $"/{obj.name}{path}";
            }

            path = $"{obj.scene.name}{path}";
            return path;
        }
    }
}