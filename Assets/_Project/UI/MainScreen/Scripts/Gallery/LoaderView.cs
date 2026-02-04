using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.UI.MainScreen.Scripts
{
    public class LoaderView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private ComponentFadeAnimation fadeAnimation;
        [SerializeField] private RectTransform loaderTransform;
        [SerializeField] private float rotationSpeed = 360f;
    
        private bool _isRotating;

        public async UniTask Show()
        {
            SetActive(true);

            await fadeAnimation.PlayAnimation();
            _isRotating = true;
        }

        private void SetActive(bool isActive)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.interactable = isActive;
            canvasGroup.blocksRaycasts = isActive;
        }

        public async UniTask Hide()
        {
            await fadeAnimation.PlayBackwardsAnimation();

            _isRotating = false;
            
            SetActive(false);
        }

        private void Update()
        {
            if (_isRotating && loaderTransform != null)
            {
                loaderTransform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
            }
        }
    }
}