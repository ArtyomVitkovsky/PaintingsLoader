using UniRx;
using UnityEngine;

namespace _Project.UI.PremiumPopup.Scripts
{
    public class PriceOptionSelector : MonoBehaviour
    {
        [SerializeField] private PriceOptionItem[] priceOptions;

        private readonly IntReactiveProperty _selectedPriceIndex = new IntReactiveProperty(2);
    
        private void Awake()
        {
            for (int i = 0; i < priceOptions.Length; i++)
            {
                int index = i;
                priceOptions[i].Button.OnClickAsObservable()
                    .Subscribe(_ => _selectedPriceIndex.Value = index)
                    .AddTo(this);
            }

            _selectedPriceIndex.Subscribe(activeIndex =>
            {
                for (int i = 0; i < priceOptions.Length; i++)
                {
                    priceOptions[i].SetSelected(i == activeIndex);
                }
            }).AddTo(this);
        }

        // TODO : initialize text from RC
        // public void InitializeOptions()
        // {
        //     for (int i = 0; i < priceOptions.Length; i++)
        //     {
        //         priceOptions[i].Initialize("");
        //     }
        // }
    }
}