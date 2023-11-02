using System;
using QuickEye.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace QuickEye.Samples.CharacterCreation
{
    public class CharacterGalleryItem : MonoBehaviour
    {
        public event Action<CharacterTemplate> Clicked; 
        [SerializeField]
        private Image _avatar;

        [SerializeField]
        private Button _button;
        
        public void Initialize(CharacterTemplate template)
        {
            _button.onClick.AddListener(() => Clicked?.Invoke(template));
            _avatar.sprite = template.avatar;
        }
    }
    [Serializable]
    public class CharacterGalleryContainer : Container<CharacterGalleryItem>
    {
    }
}