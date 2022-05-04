using System;
using QuickEye.Utility;
using UnityEngine;
using UnityEngine.UI;

namespace QuickEye.Samples.ContainerUsage
{
    public class ContainerUsageExample : MonoBehaviour
    {
        [SerializeField]
        private PictureGallery pictureGallery;

        [SerializeField]
        private Sprite[] pictures;

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
            foreach (var image in pictureGallery)
                image.color = color;
        }

        private void SetupGallery()
        {
            pictureGallery.Clear();
            foreach (var picture in pictures)
                pictureGallery.AddNew().sprite = picture;
        }
        
        [Serializable]
        public class PictureGallery : Container<Image> { }
    }
}