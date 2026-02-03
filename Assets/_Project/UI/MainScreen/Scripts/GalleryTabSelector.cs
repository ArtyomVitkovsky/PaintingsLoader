using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace _Project.UI.MainScreen.Scripts
{
    public class GalleryTabSelector : MonoBehaviour
    {
        [SerializeField] private GalleryTabButton[] galleryTabButtons;
        [SerializeField] private SortingType defaultTab = SortingType.All;

        private readonly ReactiveProperty<SortingType> _currentTab = new ReactiveProperty<SortingType>();
        public IReadOnlyReactiveProperty<SortingType> OnTabSelected => _currentTab;

        private void Awake()
        {
            _currentTab.Value = defaultTab;

            var buttonClicks = galleryTabButtons.Select(button => 
                button.OnButtonClick.Select(_ => button.Type)
            );

            Observable.Merge(buttonClicks)
                .DistinctUntilChanged()
                .Subscribe(type => _currentTab.Value = type)
                .AddTo(this);

            _currentTab.Subscribe(activeType =>
            {
                foreach (var button in galleryTabButtons)
                {
                    button.SetSelected(button.Type == activeType);
                }
            }).AddTo(this);
        }
    }
}