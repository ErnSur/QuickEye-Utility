using System;
using QuickEye.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace QuickEye.Samples.CharacterCreation
{
    [Serializable]
    public class SkillGalleryItems : PoolContainer<SkillGalleryItem>
    {
    }

    public class SkillGalleryItem : MonoBehaviour
    {
        [SerializeField]
        private Image _icon;

        [SerializeField]
        private Sprite _noSkillIcon;
        
        public void Setup( Skill skill)
        {
            _icon.sprite = skill != null ? skill.icon : _noSkillIcon;
        }
    }
}