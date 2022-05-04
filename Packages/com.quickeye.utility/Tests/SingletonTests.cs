using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace QuickEye.Utility.Tests
{
    internal class SingletonTests
    {
       // [SetUp]
        public void Setup()
        {
        }

        [UnityTest]
        public IEnumerator CannotCreateInstance_WhenAppIsQuiting()
        {
            // yield return EditorSceneManager.LoadSceneAsyncInPlayMode("Assets/Tests/TestScene.unity",
            //     new LoadSceneParameters(LoadSceneMode.Single));
            yield return new EnterPlayMode();
            var testComponent = new TestComponentBuilder
            {
                OnDestroyCallback = () =>
                {
                    var singleton = TestSingletonMonoBehaviour.Instance;
                }
            }.BuildGameObject();
            yield return new ExitPlayMode();
        }

       // [UnityTearDown]
        public IEnumerator TearDown()
        {
            Object.Destroy(TestSingletonMonoBehaviour.Instance);
            yield return new ExitPlayMode();
        }


        [Test]
        public void ShouldNot_CreateNewInstance_WhenUninitializedInstanceIsInTheScene()
        {
            var uninitializedSingleton = TestSingletonMonoBehaviour.CreateUninitializedInstance();
            var singleton = TestSingletonMonoBehaviour.Instance;
            Assert.AreSame(uninitializedSingleton, singleton);
        }

        [Test]
        public void Should_CreateNewGameObject_WhenInstanceIsCalled()
        {
            var singleton = TestSingletonMonoBehaviour.Instance;
            Assert.IsNotNull(singleton);
        }

        [Test]
        public void Should_InstantiatePrefab_WhenSingletonAttributeIsUsed()
        {
            var singleton = TestSingletonMonoBehaviour.Instance;
            Assert.IsNotNull(singleton);
        }

        [Test]
        public void ShouldReturn_InitializedInstance_WhenNo()
        {
        }

        [Test]
        public void ShouldBeAbleTo_DestroyAndRecreateSingletonInstance()
        {
            var i = TestSingletonMonoBehaviour.Instance;
            Object.Destroy(i);
            i = TestSingletonMonoBehaviour.Instance;
            Assert.NotNull(i);
        }
    }
}