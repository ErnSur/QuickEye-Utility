using UnityEngine;

namespace QuickEye.Utility.Tests
{
    internal class TestSingletonMonoBehaviour : SingletonMonoBehaviour<TestSingletonMonoBehaviour>
    {
        private static bool _enableSingletonInitialization = true;

        public static TestSingletonMonoBehaviour CreateUninitializedInstance()
        {
            var go = new GameObject("Test Singleton- Uninitialized");
            _enableSingletonInitialization = false;
            var instance = go.AddComponent<TestSingletonMonoBehaviour>();
            _enableSingletonInitialization = true;
            return instance;
        }

        protected override void Awake()
        {
            if (_enableSingletonInitialization)
            {
                base.Awake();
            }
        }

        public void Initialize() => Awake();

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Debug.Log($"Destroyed TestSingleton");
        }
    }
}