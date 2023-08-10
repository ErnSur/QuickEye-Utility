using UnityEngine;

namespace OneAsset
{
    /// <summary>
    /// Adds singleton behaviour to descendant classes.
    /// Loads or creates instance of T when Instance property is used.
    /// Can be combined with <see cref="LoadFromAssetAttribute"/>, <see cref="CreateAssetAutomaticallyAttribute"/> and <see cref="SettingsProviderAssetAttribute"/>
    /// </summary>
    /// <typeparam name="T">Type of the singleton instance</typeparam>
    public abstract class OneScriptableObject<T> : OneScriptableObject
        where T : OneScriptableObject<T>
    {
        private static T _instance;
        
        /// <summary>
        /// Returns a instance of T.
        /// If no instance of T exists, it will create a new one using <see cref="OneAssetLoader.LoadOrCreateInstance{T}"/>
        /// </summary>
        public static T Instance => GetInstance();

        private static T GetInstance()
        {
            if (_instance == null)
                _instance = OneAssetLoader.LoadOrCreateScriptableObject<T>();
            return _instance;
        }
    }
    

    /// <summary>
    /// Non generic base class of <see cref="OneScriptableObject{T}"/>,
    /// useful for non generic polymorphism.
    /// </summary>
    public abstract class OneScriptableObject : ScriptableObject
    {
    }
}