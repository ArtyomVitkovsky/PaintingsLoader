using _Project.UI.MainScreen.Scripts.Gallery;
using UnityEngine;

namespace _Project.UI.MainScreen.Scripts
{
    public class GalleryTabButton : AnimatedButton
    {
        [SerializeField] private SortingType sortingType;

        public SortingType Type => sortingType;
    }
}