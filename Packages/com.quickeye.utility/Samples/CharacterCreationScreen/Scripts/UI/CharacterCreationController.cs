using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace QuickEye.Samples.CharacterCreation
{
    public class CharacterCreationController : MonoBehaviour
    {
        [FormerlySerializedAs("_selectableFighters")]
        [SerializeField]
        private CharacterTemplate[] selectableFighters;

        [SerializeField]
        private SkillGallery skillGallery;

        [SerializeField]
        private CharacterPreview characterPreview;

        [SerializeField]
        private CharacterGallery characterGallery;

        private void Awake()
        {
            characterGallery.Initialize(selectableFighters);
            characterGallery.CharacterSelected += c =>
            {
                skillGallery.Setup(c);
                characterPreview.Setup(c);
            };
        }
    }
}