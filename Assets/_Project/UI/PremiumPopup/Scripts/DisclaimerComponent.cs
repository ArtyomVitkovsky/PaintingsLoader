using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.UI.PremiumPopup
{
    public class DisclaimerComponent : MonoBehaviour
    {
        [SerializeField] private Button restoreButton;
        [SerializeField] private Button privacyPolicyButton;
        [SerializeField] private Button termsOfServiceButton;

        private void Awake()
        {
            restoreButton.OnClickAsObservable()
                .Select(_ => "Restore")
                .Subscribe(HandleDisclaimerButtonClick)
                .AddTo(this);
            
            privacyPolicyButton.OnClickAsObservable()
                .Select(_ => "Privacy Policy")
                .Subscribe(HandleDisclaimerButtonClick)
                .AddTo(this);
            
            termsOfServiceButton.OnClickAsObservable()
                .Select(_ => "Terms of Service")
                .Subscribe(HandleDisclaimerButtonClick)
                .AddTo(this);
        }

        private void HandleDisclaimerButtonClick(string option)
        {
            Debug.Log($"Redirect to {option}");
        }
    }
}