using UnityEngine;
using UnityEngine.UI;

namespace QuickEye.Samples.CharacterCreation
{
    public class CharacterPreview : MonoBehaviour
    {
        [SerializeField]
        private Image _avatar;

        [SerializeField]
        private GameObject _statsContainer;

        [SerializeField]
        private Text _hpLabel, _nameLabel;

        [SerializeField]
        private Sprite _nothingSelectedAvatar;
        
        public void Setup(CharacterTemplate selectedFighter)
        {
            UpdateCharacterDetails(selectedFighter);
        }

        private void UpdateCharacterDetails(CharacterTemplate character)
        {
            if (character == null)
            {
                _statsContainer.SetActive(false);

                _avatar.sprite = _nothingSelectedAvatar;
                _nameLabel.text = "";
            }
            else
            {
                _statsContainer.SetActive(true);
                _hpLabel.text = $"HP: {character.hp}";

                _avatar.sprite = character.avatar;
                _nameLabel.text = character.name;
            }
        }
    }
}