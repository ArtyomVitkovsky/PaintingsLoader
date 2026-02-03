using UnityEngine;

namespace _Project.UI.MainScreen.Scripts
{
    public class GalleryTabButton : AnimatedButton
    {
        [SerializeField] private SortingType sortingType;
        [SerializeField] private GameObject selectionMarker;

        public SortingType Type => sortingType;

        public override void SetSelected(bool selected)
        {
            base.SetSelected(selected);
            
            selectionMarker.SetActive(selected);
        }
    }
}