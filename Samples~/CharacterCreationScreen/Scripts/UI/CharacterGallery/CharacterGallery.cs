using QuickEye.Samples.UIEvents;
using UnityEngine;

namespace QuickEye.Samples.CharacterCreation
{
    public class CharacterGallery : MonoBehaviour
    {
        [SerializeField]
        private CharacterGalleryContainer _galleryItems;

        public void Initialize(CharacterTemplate[] fighters)
        {
            foreach (var characterTemplate in fighters)
            {
                var item = _galleryItems.AddNew();
                item.Initialize(characterTemplate);
                item.Clicked += CharacterSelected.Trigger;
            }
        }
    }
}