using UnityEngine;

namespace OneAsset
{
    /// <summary>
    /// Adds singleton behaviour to descendant classes.
    /// Loads or creates instance of T when Instance property is used.
    /// <para>Add <see cref="LoadFromAssetAttribute"/> to load instance from prefab asset.</para>
    /// </summary>
    /// <typeparam name="T">Type of the singleton instance</typeparam>
    public class OneGameObject<T> : OneGameObject where T : OneGameObject<T>
    {
        private static T _instance;

        /// <summary>
        /// <para>Returns a instance of T.</para>
        /// <para>If no instance of the T exists, it will create a new one.</para>
        /// <para>If multiple instances of T exist, the first one executing the <see cref="Awake"/> will be preserved while the rest will self destruct on their <see cref="Awake"/>.</para>
        /// <para>If the only instance of T will be destroyed, the new one will be created on the next access to this property.</para>
        /// <para>If T has a <see cref="LoadFromAssetAttribute"/> the instance will be loaded from a prefab asset.</para>
        /// </summary>
        /// <exception cref="AssetIsMissingException">Thrown when T has a <see cref="LoadFromAssetAttribute"/> but no asset was found at path provided</exception>
        public static T Instance => GetInstance();

        /// <summary>
        /// MonoBehaviour's Awake Message. When overriden, class descendants need to call the base implementation of it to keep singleton behavior. 
        /// </summary>
        protected virtual void Awake() => Initialize();

        private void Initialize()
        {
            if (_instance != this)
                if (_instance != null || !(this is T))
                {
                    Destroy(gameObject);
                    throw new SingletonAlreadyExistsException(this);
                }

            ForceDontDestroyOnLoad();
            _instance = (T)this;
        }

        /// <summary>
        /// MonoBehaviour's OnDestroy Message. When overriden, class descendants need to call the base implementation of it to keep singleton behavior.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (_instance != this)
                return;
            _instance = null;
        }

        private void ForceDontDestroyOnLoad()
        {
            // `Object.DontDestroyOnLoad` only works for root GameObjects
            // That's why we want to leave parents that aren't OneGameObjects.
            var singletonParents = GetComponentsInParent<OneGameObject>();
            if (singletonParents.Length <= 1)
                transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }

        private static T GetInstance()
        {
            // When app is quitting singleton objects will be destroyed
            // In this case we shouldn't try to create a new singleton
            if (IsAppQuitting)
                return null;
            if (_instance == null)
                _instance = OneAssetLoader.CreateOrLoadGameObject(typeof(T)) as T;

            return _instance;
        }
    }
    
    /// <summary>
    /// Non generic base class of <see cref="OneGameObject{T}"/>,
    /// useful for non generic polymorphism.
    /// </summary>
    // Execute Order allows us to execute Awake before all non-singletons
    // This allows other scripts to access initialized singletons in their awake methods.
    [DefaultExecutionOrder(-10000)]
    public abstract class OneGameObject : MonoBehaviour
    {
        protected static bool IsAppQuitting;

        protected virtual void OnApplicationQuit()
        {
            IsAppQuitting = true;
        }
    }
}