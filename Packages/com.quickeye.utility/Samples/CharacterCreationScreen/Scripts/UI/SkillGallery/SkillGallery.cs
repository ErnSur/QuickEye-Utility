using UnityEngine;

namespace QuickEye.Samples.CharacterCreation
{
    public class SkillGallery : MonoBehaviour
    {
        [SerializeField]
        private SkillGalleryItems _galleryItems;
        
        public void Setup(CharacterTemplate character)
        {
            _galleryItems.Clear();

            if (character?.skills == null)
                return;

            foreach (var skill in character.skills)
                _galleryItems.AddNew().Setup(skill);
        }
    }
}