using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QuickEye.Samples.ContainerUsage
{
    public class NoContainerUsageExample : MonoBehaviour
    {
        [SerializeField]
        private Image pictureItemPrefab;
        
        [SerializeField]
        private Transform pictureGalleryContainer;
        
        [SerializeField]
        private Sprite[] pictures;
        
        private readonly List<Image> _galleryItems = new List<Image>();

        private void Awake()
        {
            SetupGallery();
        }

        public void UpdatePictures(Sprite[] newPictures)
        {
            pictures = newPictures;
            SetupGallery();
        }

        public void UpdatePicturesColor(Color color)
        {
            foreach (var image in _galleryItems)
                image.color = color;
        }

        private void SetupGallery()
        {
            ClearGallery();
            foreach (var picture in pictures)
                CreateNewGalleryItem(picture);
        }

        private void CreateNewGalleryItem(Sprite picture)
        {
            var item = Instantiate(pictureItemPrefab, pictureGalleryContainer);
            _galleryItems.Add(item);
            item.sprite = picture;
        }

        private void ClearGallery()
        {
            for (var i = _galleryItems.Count - 1; i >= 0; i--)
            {
                Destroy(_galleryItems[i].gameObject);
                _galleryItems.RemoveAt(i);
            }
        }
    }
}