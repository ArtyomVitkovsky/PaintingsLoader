using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace _Project.UI.MainScreen.Scripts
{
    public class GalleryTabSelector : MonoBehaviour
    {
        [SerializeField] private GalleryTabButton[] galleryTabButtons;

        private readonly Subject<SortingType> _onTabSelected = new Subject<SortingType>();
        public IObservable<SortingType> OnTabSelected => _onTabSelected;
        
        private void Awake()
        {
            var buttonStreams = galleryTabButtons.Select(button => 
                button.OnButtonClick.Select(_ => button.Type)
            );

            buttonStreams.Merge()
                .Subscribe(_onTabSelected)
                .AddTo(this);
        }

        private void OnButtonClick(AnimatedButton animatedButton)
        {
            if (animatedButton is GalleryTabButton galleryTabButton)
            {
                _onTabSelected?.OnNext(galleryTabButton.Type);
            }
        }

        private void OnDestroy()
        {
            _onTabSelected.OnCompleted();
            _onTabSelected.Dispose();
        }
    }
}