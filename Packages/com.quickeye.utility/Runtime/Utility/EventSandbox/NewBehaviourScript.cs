using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace QuickEye.Utility
{
    public interface ISingletonGameEvent<TEvent,TArg> : ISingleton<TEvent> where TEvent : GameEvent<TArg>
    {
        public static void Register(Action<TArg> callback)
        {
            Instance.Register(callback);
        }
    } 
    
    public interface ISingleton<T> where T : ScriptableObject
    {
        private static T _instance; 
        public static T Instance => _instance != null
            ? _instance
            : _instance = SingletonScriptableObjectManager.LoadOrCreateInstance<T>();
    }
    
    public class Sing : ScriptableObject ,ISingleton<Sing>
    {
        void Test()
        {
            //PlayerHpChange.Register(default);
            //ISingleton<PlayerHpChange>.Instance.Register(default);
            ISingletonGameEvent<PlayerHpChange,float>.Register(default);
            //GameEvent.Register<PlayerHpChange>();
           // GameEvent.Register<FloatEvent>();
            var x = ISingleton<Sing>.Instance;
        }
    }
    public class SingletonScriptableObjectManager
    {
        internal static TryCreateAsset TryCreateAssetAction;
        
        
        public static T LoadOrCreateInstance<T>() where T : ScriptableObject
        {
            // Try to load asset from `SingletonAssetAttribute` path
            if (TryLoadFromResources<T>(out var asset))
                return asset;
            // Try to create asset at `CreateAssetAutomaticallyAttribute` path
            if (TryCreateAsset<T>() && TryLoadFromResources(out asset))
                return asset;
            // Throw Exception if class has `SingletonAssetAttribute` and asset instance is mandatory 
            var att = typeof(T).GetCustomAttribute<SingletonAssetAttribute>();
            if (att?.Mandatory == true)
                throw new Exception($"Object of type: {typeof(T).FullName} requires singleton asset.");
            // Create and return singleton instance
            var obj = ScriptableObject.CreateInstance<T>();
            obj.name = typeof(T).Name;
            return obj;
        }
        
        /// <summary>
        /// If in Editor, try to create an asset at path specified in `CreateAssetAutomaticallyAttribute`
        /// </summary>
        private static bool TryCreateAsset<T>() where T : ScriptableObject
        {
            if (!Application.isEditor || TryCreateAssetAction == null)
                return false;
            var att = typeof(T).GetCustomAttribute<CreateAssetAutomaticallyAttribute>();
            if (att == null)
                return false;
            var obj = ScriptableObject.CreateInstance<T>();
            obj.name = typeof(T).Name;
            if (TryCreateAssetAction(obj))
                return true;
            throw new EditorAssetFactoryException(att.FullAssetPath);
        }

        private static bool TryLoadFromResources<T>(out T obj) where T : ScriptableObject
        {
            var attr = typeof(T).GetCustomAttribute<SingletonAssetAttribute>();
            if (attr == null)
                return obj = null;
            obj = Resources.Load<T>(attr.ResourcesPath);
            return obj != null;
        }
    }
    public class NewBehaviourScript : MonoBehaviour
    {
        public GameEvent<float> someSer;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
