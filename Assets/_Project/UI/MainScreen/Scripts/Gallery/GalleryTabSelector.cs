using System;
using System.Linq;
using System.Threading;
using _Project.UI.MainScreen.Scripts.Gallery;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace _Project.UI.MainScreen.Scripts
{
    public class GalleryTabSelector : MonoBehaviour
    {
        [SerializeField] private GalleryTabButton[] galleryTabButtons;
        [SerializeField] private SortingType defaultTab = SortingType.All;
        
        [Header("Marker")]
        [SerializeField] private RectTransform marker;
        [SerializeField] private float duration = 0.25f;

        private readonly ReactiveProperty<SortingType> _currentTab = new ReactiveProperty<SortingType>();
        public IReadOnlyReactiveProperty<SortingType> OnTabSelected => _currentTab;

        private CancellationTokenSource markerCts;

        private void Awake()
        {
            _currentTab.Value = defaultTab;

            foreach (var tabButton in galleryTabButtons)
            {
                tabButton.OnButtonClick.Subscribe(_ =>
                {
                    _currentTab.Value = tabButton.Type;
                    MoveMarkerTask(tabButton);
                    
                }).AddTo(this);
            }

            _currentTab.Subscribe(activeType =>
            {
                foreach (var button in galleryTabButtons)
                {
                    button.SetSelected(button.Type == activeType);
                }
            }).AddTo(this);
        }
        
        private async UniTask MoveMarkerTask(GalleryTabButton targetButton)
        {
            markerCts?.Cancel();
            markerCts = new CancellationTokenSource();
            var token = markerCts.Token;
            
            var targetRect = targetButton.GetComponent<RectTransform>();

            var startPos = marker.anchoredPosition;
            var startSize = marker.sizeDelta;
    
            var endPos = new Vector2(targetRect.anchoredPosition.x, startPos.y);
            var endSize = new Vector2(targetRect.rect.width, startSize.y);

            float elapsed = 0;
            while (elapsed < duration && !token.IsCancellationRequested)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.SmoothStep(0, 1, elapsed / duration);

                marker.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
                marker.sizeDelta = Vector2.Lerp(startSize, endSize, t);

                await UniTask.NextFrame(token);
            }
    
            marker.anchoredPosition = endPos;
            marker.sizeDelta = endSize;
        }
    }
}