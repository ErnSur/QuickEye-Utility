using System;
using UnityEditor;
using UnityEngine;

namespace Samples.GlobalEvents
{
    class SampleUsage : MonoBehaviour
    {
        public PlayerHpChanged ev;
        public PlayerMpChanged ev2;
        void Awake()
        {
            ev = PlayerHpChanged.Instance;
            ev2 = PlayerMpChanged.Instance;
        }

        void OnEnable()
        {
            PlayerHpChanged.Register(OnPlayerHpChanged);
            PlayerMpChanged.Register(OnPlayerManaChanged);
            
            PlayerManaChange.Register(OnPlayerManaChanged);
        }

        void OnPlayerManaChanged(float mana)
        {
            Debug.Log($"Player Mana changed {mana}");
        }

        void OnDisable()
        {
            PlayerHpChanged.Unregister(OnPlayerHpChanged);
            PlayerMpChanged.Unregister(OnPlayerManaChanged);
            
            PlayerManaChange.Unregister(OnPlayerManaChanged);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayerHealthChange.Trigger();
                PlayerManaChange.Trigger(33);
                //PlayerHPChanged.Trigger(70);
            }
        }

        void OnPlayerHpChanged(float hp)
        {
            Debug.Log($"HP changed: {hp}");
        }
    }
}