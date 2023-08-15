using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using QuickEye.ScriptableObjectVariants;
using UnityEditor;
using UnityEngine;

namespace ScriptableVariants.Editor.Tests
{
    public class GetModifiedFieldsTests
    {
        private TestSo _target;
        private SerializedObject _serializedObject;
        private SerializedProperty _prop1;
        private SerializedProperty _prop2;
        private TestWindow _window;

        [SetUp]
        public void SetUp()
        {
            _target = ScriptableObject.CreateInstance<TestSo>();
            _window = ScriptableObject.CreateInstance<TestWindow>();
            _serializedObject = new SerializedObject(_target);
            _prop1 = _serializedObject.FindProperty(nameof(TestSo.prop1));
            _prop2 = _serializedObject.FindProperty(nameof(TestSo.prop2));
        }

        [TearDown]
        public void TearDown()
        {
            _window.Show();
            _window.Close();
            _prop1.Dispose();
            _prop2.Dispose();
            _serializedObject.Dispose();
            Object.DestroyImmediate(_target);
            Object.DestroyImmediate(_window);
        }

        [Test]
        public void Should_UpdateChangeCheckScope_When_UpdatingGuiChanged()
        {
            using (var changeCheckScope = new EditorGUI.ChangeCheckScope())
            {
                GUI.changed = true;
                Assert.IsTrue(changeCheckScope.changed);
            }
        }

        [Test]
        public async Task PropertyWrapperScope_LastPropProblem()
        {
            var queue = new Queue<int>();
            _window.Draw = () =>
            {
                queue.Clear();
                using (new PropertyWrapperScope(OnStart, OnEnd))
                {
                    EditorGUI.PropertyField(new Rect(), _prop1);
                    // This can cause a problem, the last OnEnd is called on scope dispose
                    queue.Enqueue(2);
                }
            };


            await ShowWindow();

            void OnStart(Rect arg1, SerializedProperty arg2)
            {
                queue.Enqueue(1);
            }

            void OnEnd(Rect arg1, SerializedProperty arg2)
            {
                queue.Enqueue(3);
            }

            CollectionAssert.AreEqual(queue, new[] { 1, 2, 3 });
        }

        [Test]
        public void Should_hasModifiedProperties_ReturnTrue()
        {
            Assert.AreEqual(false, _serializedObject.hasModifiedProperties);
            _prop1.intValue = 5;
            Assert.AreEqual(true, _serializedObject.hasModifiedProperties);
        }
        
        [Test]
        public void Should_BeAbleToCacheSerializedProperty()
        {
            _prop1.intValue = 0;
            var valueBefore = _prop1.Copy();
            _prop1.intValue = 5;

            // var iterator = _serializedObject.GetIterator();
            // for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            // {
            //     
            // }
            Assert.AreEqual(0, valueBefore.intValue);
        }

        private async Task ShowWindow()
        {
            _window.Show();
            await Task.Delay(1);
        }
    }
}