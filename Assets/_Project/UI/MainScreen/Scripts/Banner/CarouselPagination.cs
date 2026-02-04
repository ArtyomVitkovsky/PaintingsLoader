using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class CarouselPagination : MonoBehaviour
{
    [SerializeField] private UICarouselScroll carousel;
    [SerializeField] private Image[] dots;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    private void Start()
    {
        carousel.CurrentPage
            .Subscribe(UpdateDots)
            .AddTo(this);
    }

    private void UpdateDots(int index)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].sprite = (i == index) ? activeSprite : inactiveSprite;
        }
    }
}