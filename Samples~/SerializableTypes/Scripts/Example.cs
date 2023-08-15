using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QuickEye.Utility.SerializableTypes
{
    [CreateAssetMenu]
    public class Example : ScriptableObject
    {
        [SerializeField]
        private UnityDictionary<PokemonType, PokemonType> pokemonTypeWeakness =
            new UnityDictionary<PokemonType, PokemonType>();

        [SerializeField]
        private UnityTimeSpan buffDuration;

        [TimeOfDay]
        [SerializeField]
        private UnityTimeSpan clockTime;

        [SerializeField]
        private UnityDateOnly userBirthday;

        [SerializeField]
        private UnityDateTime lastLogin;
    }

    [Flags]
    public enum PokemonType
    {
        None = 0,
        Normal = 1,
        Fire = 2,
        Water = 4,
        Grass = 8,
        Ice = 16,
    }
}