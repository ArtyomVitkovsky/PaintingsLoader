using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.UI.MainScreen.Scripts
{
    public class LoaderView : MonoBehaviour
    {
        [SerializeField] private ComponentFadeAnimation fadeAnimation;
        [SerializeField] private RectTransform loaderTransform;
        [SerializeField] private float rotationSpeed = 360f;
    
        private bool _isRotating;

        public async UniTask Show()
        {
            gameObject.SetActive(true);
            await fadeAnimation.PlayAnimation();
            _isRotating = true;
        }

        public async UniTask Hide()
        {
            await fadeAnimation.PlayBackwardsAnimation();
        
            _isRotating = false;
            gameObject.SetActive(false);
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