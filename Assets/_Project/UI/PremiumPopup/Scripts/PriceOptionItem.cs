using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI.PremiumPopup.Scripts
{
    public class PriceOptionItem : MonoBehaviour
    {
        [SerializeField] private Image selectionToggle;
        [SerializeField] private Sprite selectedSprite;
        [SerializeField] private Sprite unselectedSprite;
        [SerializeField] private Button button;
        [SerializeField] private TMP_Text optionText;

        public Button Button => button;

        public void Initialize(string text)
        {
            optionText.SetText(text);
        }

        public void SetSelected(bool isSelected)
        {
            selectionToggle.sprite = isSelected ? selectedSprite : unselectedSprite;
        }
    }
}