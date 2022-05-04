using System;
using UnityEngine;

namespace QuickEye.Utility.Tests
{
    internal class TestComponentBuilder
    {
        public TestComponent BuildGameObject(string name = "Test Component")
        {
            var go = new GameObject(name);
            go.SetActive(false);
            var cc = go.AddComponent<TestComponent>();
            cc.Messages = this;
            go.SetActive(true);
            return cc;
        }

        public Action AwakeCallback;
        public Action FixedUpdateCallback;
        public Action LateUpdateCallback;
        public Action OnAnimatorMoveCallback;
        public Action OnApplicationFocusCallback;
        public Action OnApplicationPauseCallback;
        public Action OnApplicationQuitCallback;
        public Action OnBecameInvisibleCallback;
        public Action OnBecameVisibleCallback;
        public Action OnCollisionEnterCallback;
        public Action OnCollisionEnter2DCallback;
        public Action OnCollisionExitCallback;
        public Action OnCollisionExit2DCallback;
        public Action OnCollisionStayCallback;
        public Action OnCollisionStay2DCallback;
        public Action OnConnectedToServerCallback;
        public Action OnControllerColliderHitCallback;
        public Action OnDestroyCallback;
        public Action OnDisableCallback;
        public Action OnDisconnectedFromServerCallback;
        public Action OnDrawGizmosCallback;
        public Action OnDrawGizmosSelectedCallback;
        public Action OnEnableCallback;
        public Action OnFailedToConnectCallback;
        public Action OnFailedToConnectToMasterServerCallback;
        public Action OnGUICallback;
        public Action OnJointBreakCallback;
        public Action OnJointBreak2DCallback;
        public Action OnMasterServerEventCallback;
        public Action OnMouseDownCallback;
        public Action OnMouseDragCallback;
        public Action OnMouseEnterCallback;
        public Action OnMouseExitCallback;
        public Action OnMouseOverCallback;
        public Action OnMouseUpCallback;
        public Action OnMouseUpAsButtonCallback;
        public Action OnNetworkInstantiateCallback;
        public Action OnParticleCollisionCallback;
        public Action OnParticleSystemStoppedCallback;
        public Action OnParticleTriggerCallback;
        public Action OnParticleUpdateJobScheduledCallback;
        public Action OnPlayerConnectedCallback;
        public Action OnPlayerDisconnectedCallback;
        public Action OnPostRenderCallback;
        public Action OnPreCullCallback;
        public Action OnRenderObjectCallback;
        public Action OnSerializeNetworkViewCallback;
        public Action OnServerInitializedCallback;
        public Action OnTransformChildrenChangedCallback;
        public Action OnTransformParentChangedCallback;
        public Action OnTriggerEnterCallback;
        public Action OnTriggerEnter2DCallback;
        public Action OnTriggerExitCallback;
        public Action OnTriggerExit2DCallback;
        public Action OnTriggerStayCallback;
        public Action OnTriggerStay2DCallback;
        public Action OnWillRenderObjectCallback;
        public Action ResetCallback;
        public Action StartCallback;
        public Action UpdateCallback;
    }

    internal class TestComponent : MonoBehaviour
    {
        public TestComponentBuilder Messages;
        private void Awake()                           => Messages.AwakeCallback?.Invoke();
        private void FixedUpdate()                     => Messages.FixedUpdateCallback?.Invoke();
        private void LateUpdate()                      => Messages.LateUpdateCallback?.Invoke();
        private void OnAnimatorMove()                  => Messages.OnAnimatorMoveCallback?.Invoke();
        private void OnApplicationFocus()              => Messages.OnApplicationFocusCallback?.Invoke();
        private void OnApplicationPause()              => Messages.OnApplicationPauseCallback?.Invoke();
        private void OnApplicationQuit()               => Messages.OnApplicationQuitCallback?.Invoke();
        private void OnBecameInvisible()               => Messages.OnBecameInvisibleCallback?.Invoke();
        private void OnBecameVisible()                 => Messages.OnBecameVisibleCallback?.Invoke();
        private void OnCollisionEnter()                => Messages.OnCollisionEnterCallback?.Invoke();
        private void OnCollisionEnter2D()              => Messages.OnCollisionEnter2DCallback?.Invoke();
        private void OnCollisionExit()                 => Messages.OnCollisionExitCallback?.Invoke();
        private void OnCollisionExit2D()               => Messages.OnCollisionExit2DCallback?.Invoke();
        private void OnCollisionStay()                 => Messages.OnCollisionStayCallback?.Invoke();
        private void OnCollisionStay2D()               => Messages.OnCollisionStay2DCallback?.Invoke();
        private void OnConnectedToServer()             => Messages.OnConnectedToServerCallback?.Invoke();
        private void OnControllerColliderHit()         => Messages.OnControllerColliderHitCallback?.Invoke();
        private void OnDestroy()                       => Messages.OnDestroyCallback?.Invoke();
        private void OnDisable()                       => Messages.OnDisableCallback?.Invoke();
        private void OnDisconnectedFromServer()        => Messages.OnDisconnectedFromServerCallback?.Invoke();
        private void OnDrawGizmos()                    => Messages.OnDrawGizmosCallback?.Invoke();
        private void OnDrawGizmosSelected()            => Messages.OnDrawGizmosSelectedCallback?.Invoke();
        private void OnEnable()                        => Messages.OnEnableCallback?.Invoke();
        private void OnFailedToConnect()               => Messages.OnFailedToConnectCallback?.Invoke();
        private void OnFailedToConnectToMasterServer() => Messages.OnFailedToConnectToMasterServerCallback?.Invoke();
        private void OnGUI()                           => Messages.OnGUICallback?.Invoke();
        private void OnJointBreak()                    => Messages.OnJointBreakCallback?.Invoke();
        private void OnJointBreak2D()                  => Messages.OnJointBreak2DCallback?.Invoke();
        private void OnMasterServerEvent()             => Messages.OnMasterServerEventCallback?.Invoke();
        private void OnMouseDown()                     => Messages.OnMouseDownCallback?.Invoke();
        private void OnMouseDrag()                     => Messages.OnMouseDragCallback?.Invoke();
        private void OnMouseEnter()                    => Messages.OnMouseEnterCallback?.Invoke();
        private void OnMouseExit()                     => Messages.OnMouseExitCallback?.Invoke();
        private void OnMouseOver()                     => Messages.OnMouseOverCallback?.Invoke();
        private void OnMouseUp()                       => Messages.OnMouseUpCallback?.Invoke();
        private void OnMouseUpAsButton()               => Messages.OnMouseUpAsButtonCallback?.Invoke();
        private void OnNetworkInstantiate()            => Messages.OnNetworkInstantiateCallback?.Invoke();
        private void OnParticleCollision()             => Messages.OnParticleCollisionCallback?.Invoke();
        private void OnParticleSystemStopped()         => Messages.OnParticleSystemStoppedCallback?.Invoke();
        private void OnParticleTrigger()               => Messages.OnParticleTriggerCallback?.Invoke();
        private void OnParticleUpdateJobScheduled()    => Messages.OnParticleUpdateJobScheduledCallback?.Invoke();
        private void OnPlayerConnected()               => Messages.OnPlayerConnectedCallback?.Invoke();
        private void OnPlayerDisconnected()            => Messages.OnPlayerDisconnectedCallback?.Invoke();
        private void OnPostRender()                    => Messages.OnPostRenderCallback?.Invoke();
        private void OnPreCull()                       => Messages.OnPreCullCallback?.Invoke();
        private void OnRenderObject()                  => Messages.OnRenderObjectCallback?.Invoke();
        private void OnSerializeNetworkView()          => Messages.OnSerializeNetworkViewCallback?.Invoke();
        private void OnServerInitialized()             => Messages.OnServerInitializedCallback?.Invoke();
        private void OnTransformChildrenChanged()      => Messages.OnTransformChildrenChangedCallback?.Invoke();
        private void OnTransformParentChanged()        => Messages.OnTransformParentChangedCallback?.Invoke();
        private void OnTriggerEnter()                  => Messages.OnTriggerEnterCallback?.Invoke();
        private void OnTriggerEnter2D()                => Messages.OnTriggerEnter2DCallback?.Invoke();
        private void OnTriggerExit()                   => Messages.OnTriggerExitCallback?.Invoke();
        private void OnTriggerExit2D()                 => Messages.OnTriggerExit2DCallback?.Invoke();
        private void OnTriggerStay()                   => Messages.OnTriggerStayCallback?.Invoke();
        private void OnTriggerStay2D()                 => Messages.OnTriggerStay2DCallback?.Invoke();
        private void OnWillRenderObject()              => Messages.OnWillRenderObjectCallback?.Invoke();
        private void Reset()                           => Messages.ResetCallback?.Invoke();
        private void Start()                           => Messages.StartCallback?.Invoke();
        private void Update()                          => Messages.UpdateCallback?.Invoke();
    }
}