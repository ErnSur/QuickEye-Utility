using System;
using UnityEngine;

namespace QuickEye.Samples.CharacterCreation
{
    public class CharacterGallery : MonoBehaviour
    {
        public event Action<CharacterTemplate> CharacterSelected;

        [SerializeField]
        private CharacterGalleryItems _galleryItems;

        public void Initialize(CharacterTemplate[] fighters)
        {
            foreach (var characterTemplate in fighters)
            {
                var item = _galleryItems.AddNew();
                item.Initialize(characterTemplate);
                item.Clicked += c => CharacterSelected?.Invoke(c);
            }
        }
    }
}