using System;
using Cysharp.Threading.Tasks;
using GameTemplate.UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.UI.PaintingViewPopup
{
    public class PaintingViewPopupScreen : UIQueueScreen
    {
        public static Action<DiContainer> GetContextDecorator(Texture texture)
        {
            return container =>
            {
                container.BindInstance(texture);
            };
        }
    
        [SerializeField] private RawImage displayImage;
        [SerializeField] private AnimatedButton closeButton;
        [SerializeField] private ComponentScaleAnimation screenScaleAnimation;
        [SerializeField] private ComponentFadeAnimation fadeAnimation;

        private Texture paintingTexture;

        [Inject]
        public void Construct(Texture texture)
        {
            paintingTexture = texture;
        }

        void Start()
        {
            closeButton.OnButtonClick
                .Subscribe(HandleCloseButton)
                .AddTo(this);
        
            displayImage.texture = paintingTexture;

            AnimateScreenOpening();
        }

        private void HandleCloseButton(AnimatedButton button)
        {
            Close();
        }

        private async UniTask Close()
        {
            var scaleTask = screenScaleAnimation.PlayBackwardsAnimation();
            var fadeTask = fadeAnimation.PlayBackwardsAnimation();

            await UniTask.WhenAll(scaleTask, fadeTask);
        
            Navigator.Dequeue();
        }
    
        private void AnimateScreenOpening()
        {
            screenScaleAnimation.PlayAnimation().Forget();
            fadeAnimation.PlayAnimation().Forget();
        }
    }
}
