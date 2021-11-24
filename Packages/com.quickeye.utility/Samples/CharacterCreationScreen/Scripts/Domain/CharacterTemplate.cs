using System;
using UnityEngine;

namespace QuickEye.Samples.CharacterCreation
{
    [Serializable]
    public class CharacterTemplate
    {
        public Sprite avatar;
        public Skill[] skills;
        public int hp;
        public string name;
    }
}