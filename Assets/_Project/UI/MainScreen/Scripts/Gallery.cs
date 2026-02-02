using System.Collections.Generic;
using System.Threading;
using _Project.Scripts.Core.Services.AssetsProvider;
using _Project.Scripts.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.UI.MainScreen.Scripts
{
    public enum SortingType
    {
        All = 0,
        Odd = 1,
        Even = 2
    }

    public class Gallery : MonoBehaviour
    {
        private const string baseUrl = "https://data.ikppbb.com/test-task-unity-data/pics/";

        [Inject] private DiContainer diContainer;

        [SerializeField] private RectTransform galleryItemsContainer;
        [SerializeField] private GalleryItem galleryItemPrefab;

        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private RectTransform viewport;

        [Header("Grid Config")] 
        [SerializeField] private float spacing = 10f;
        [SerializeField] private float verticalPadding = 10f;
        [SerializeField] private float horizontalPadding = 10f;

        private int _totalItemsCount = 66;
        private int _columns;
        private float _cellSize;
        private float _rowHeight;

        private List<GalleryItem> _pool = new List<GalleryItem>();

        private int[] _poolDisplayIndices;

        private List<int> _activeIds = new List<int>();

        private CancellationTokenSource _cts;
        private int _prevStartRow = -1;
        private TextureAssetProvider assetProvider = new TextureAssetProvider();

        public void Initialize(SortingType type)
        {
            _cts = new CancellationTokenSource();

            SetSorting(type);
            
            scrollRect.OnValueChangedAsObservable()
                .SampleFrame(2)
                .Subscribe(OnScroll)
                .AddTo(this);
        }

        public void SetSorting(SortingType type)
        {
            scrollRect.StopMovement();
            galleryItemsContainer.anchoredPosition = Vector2.zero;

            _activeIds.Clear();
            for (var i = 1; i <= _totalItemsCount; i++)
            {
                if (type == SortingType.All)
                {
                    _activeIds.Add(i);
                }
                else if (type == SortingType.Odd && i % 2 != 0)
                {
                    _activeIds.Add(i);
                }
                else if (type == SortingType.Even && i % 2 == 0)
                {
                    _activeIds.Add(i);
                }
            }

            SetupGrid();

            if (_pool.Count == 0)
            {
                CreatePool();
            }
            else
            {
                ResetPoolStatus();
            }

            Refresh(true);
        }
        
        private void SetupGrid()
        {
            _columns = TabletDetectorUtil.IsTablet() ? 3 : 2;

            var width = viewport.rect.width;
            var availableWidth = width - (spacing * (_columns + 1)) - horizontalPadding;
            _cellSize = availableWidth / _columns;
            _rowHeight = _cellSize + spacing;

            var totalRows = Mathf.CeilToInt((float)_activeIds.Count / _columns);
            galleryItemsContainer.sizeDelta =
                new Vector2(galleryItemsContainer.sizeDelta.x, totalRows * _rowHeight + verticalPadding / 2f);
        }

        private void CreatePool()
        {
            var visibleRows = Mathf.CeilToInt(viewport.rect.height / _rowHeight);
            var poolRows = visibleRows + 4;
            var poolSize = poolRows * _columns;

            _poolDisplayIndices = new int[poolSize];

            for (var i = 0; i < poolSize; i++)
            {
                var item = diContainer.InstantiatePrefabForComponent<GalleryItem>(
                    galleryItemPrefab,
                    galleryItemsContainer,
                    new[] { assetProvider }
                );

                item.RectTransform.anchorMin = new Vector2(0, 1);
                item.RectTransform.anchorMax = new Vector2(0, 1);
                item.RectTransform.pivot = new Vector2(0, 1);
                item.RectTransform.sizeDelta = new Vector2(_cellSize, _cellSize);
                item.gameObject.SetActive(false);

                _pool.Add(item);
                _poolDisplayIndices[i] = -1;
            }
        }


        private void ResetPoolStatus()
        {
            _prevStartRow = -1;
            
            for (int i = 0; i < _poolDisplayIndices.Length; i++)
            {
                _poolDisplayIndices[i] = -1;
                _pool[i].gameObject.SetActive(false);
            }
        }
        
        private void OnScroll(Vector2 value)
        {
            Refresh(false);
        }

        private void Refresh(bool force)
        {
            var contentY = Mathf.Max(0, galleryItemsContainer.anchoredPosition.y);
            var topVisibleRow = Mathf.FloorToInt(contentY / _rowHeight);

            var startRenderRow = Mathf.Max(0, topVisibleRow - 2);

            if (startRenderRow == _prevStartRow && !force) return;
            _prevStartRow = startRenderRow;

            var poolSize = _pool.Count;
            var totalDataCount = _activeIds.Count;

            var startDataIndex = startRenderRow * _columns;

            for (var i = 0; i < poolSize; i++)
            {
                var currentDataIndex = startDataIndex + i;
        
                var poolIndex = currentDataIndex % poolSize; 
        
                var item = _pool[poolIndex];

                if (currentDataIndex >= 0 && currentDataIndex < totalDataCount)
                {
                    if (_poolDisplayIndices[poolIndex] != currentDataIndex)
                    {
                        UpdateItemPosition(item, currentDataIndex, poolIndex);
                    }
                
                    item.gameObject.SetActive(true);
                }
                else
                {
                    item.gameObject.SetActive(false);
                    _poolDisplayIndices[poolIndex] = -1;
                }
            }
        }

        private void UpdateItemPosition(GalleryItem item, int dataIndex, int poolIndex)
        {
            _poolDisplayIndices[poolIndex] = dataIndex;

            var row = dataIndex / _columns;
            var col = dataIndex % _columns;

            var x = spacing + (col * (_cellSize + spacing)) + horizontalPadding / 2f;
            var y = -verticalPadding / 2f - (row * _rowHeight);

            item.RectTransform.anchoredPosition = new Vector2(x, y);

            var imageId = _activeIds[dataIndex];
            item.LoadImage(imageId, $"{baseUrl}{imageId}.jpg", _cts.Token);
        }

        private void OnDestroy()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }
    }
}