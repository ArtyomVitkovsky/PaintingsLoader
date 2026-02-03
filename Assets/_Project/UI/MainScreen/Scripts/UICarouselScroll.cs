using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cysharp.Threading.Tasks;
using UniRx;
using System;
using System.Threading;

public class UICarouselScroll : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Configuration")]
    [SerializeField] private RectTransform content;
    [SerializeField] private float animationDuration = 0.3f;
    [SerializeField] private float autoScrollDelay = 5f;
    [SerializeField] private float swipeThreshold = 50f;

    [SerializeField] private AnimationCurve animationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private readonly IntReactiveProperty _currentIndex = new IntReactiveProperty(0);
    public IReadOnlyReactiveProperty<int> CurrentPage => _currentIndex;

    private int _itemsCount;
    private float _dragStartPosition;
    private float _contentStartPos;
    private bool _isAnimating;
    private CancellationTokenSource _cts;

    private void Awake()
    {
        content.anchorMin = new Vector2(0.5f, 0.5f);
        content.anchorMax = new Vector2(0.5f, 0.5f);
        content.pivot = new Vector2(0.5f, 0.5f);
    }

    private void Start()
    {
        _itemsCount = content.childCount;
        if (_itemsCount == 0)
        {
            return;
        }

        MoveToIndex(0, immediate: true).Forget();
        
        StartAutoScroll();
    }

    private void StartAutoScroll()
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        
        AutoScrollLoop(token).Forget();
    }

    private async UniTaskVoid AutoScrollLoop(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(autoScrollDelay), cancellationToken: token);
            if (!_isAnimating)
            {
                var nextIndex = (_currentIndex.Value + 1) % _itemsCount;
                await MoveToIndex(nextIndex);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isAnimating = true;
        
        _dragStartPosition = eventData.position.x;
        _contentStartPos = content.anchoredPosition.x;

        _cts?.Cancel();
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var dragDelta = eventData.position.x - _dragStartPosition;
        var targetIndex = _currentIndex.Value;

        if (Mathf.Abs(dragDelta) > swipeThreshold)
        {
            if (dragDelta > 0 && _currentIndex.Value > 0)
            {
                targetIndex--;
            }
            else if (dragDelta < 0 && _currentIndex.Value < _itemsCount - 1)
            {
                targetIndex++;
            }
        }

        SnapTo(targetIndex);
    }

    private async UniTask SnapTo(int targetIndex)
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var token = _cts.Token;
        
        await MoveToIndex(targetIndex, token: token);

        StartAutoScroll();
    }

    public async UniTask MoveToIndex(int index, bool immediate = false, CancellationToken token = default)
    {
        if (content.childCount == 0) return;
        
        _isAnimating = true;
        _currentIndex.Value = index;

        var targetChild = content.GetChild(index) as RectTransform;
        if (targetChild == null)
        {
            return;
        }
        
        var targetX = -targetChild.localPosition.x;

        if (immediate)
        {
            content.anchoredPosition = new Vector2(targetX, content.anchoredPosition.y);
        }
        else
        {
            await AnimateToPosition(targetX, token);
        }

        _isAnimating = false;
    }

    private async UniTask AnimateToPosition(float targetX, CancellationToken token)
    {
        var startX = content.anchoredPosition.x;
        float elapsed = 0;

        while (elapsed < animationDuration)
        {
            elapsed += Time.deltaTime;
            var t = elapsed / animationDuration;
            var curveT = animationCurve.Evaluate(t);
            
            var currentX = Mathf.Lerp(startX, targetX, curveT);
            content.anchoredPosition = new Vector2(currentX, content.anchoredPosition.y);
            
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        content.anchoredPosition = new Vector2(targetX, content.anchoredPosition.y);
    }

    private void OnDestroy()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}