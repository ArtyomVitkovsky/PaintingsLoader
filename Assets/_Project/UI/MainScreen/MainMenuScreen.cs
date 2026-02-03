using _Project.UI.MainScreen.Scripts;
using GameTemplate.UI;
using UniRx;
using UnityEngine;

namespace _Project.UI.MainScreen
{
    public class MainMenuScreen : UIStackScreen
    {
        [SerializeField] private Gallery gallery;
        [SerializeField] private GalleryTabSelector tabSelector;

        private void Start()
        {
            gallery.Initialize(SortingType.All);

            tabSelector.OnTabSelected.Subscribe(OnTabSelected).AddTo(this);
        }

        private void OnTabSelected(SortingType type)
        {
            gallery.SetSorting(type);
        }
    }
}
