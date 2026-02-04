using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameTemplate.UI;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.UI.PremiumPopup
{
    public class PremiumPopupScreen : UIQueueScreen
    {
        [SerializeField] private RectTransform premiumContainer;
        [SerializeField] private float minContainerHeight;
        [SerializeField] private float maxContainerHeight;
        [SerializeField] private float animationDuration;
        [SerializeField] private float scaleAnimationDelay;
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private ComponentScaleAnimation containerScaleAnimation;
        [SerializeField] private ComponentScaleAnimation screenScaleAnimation;

        [SerializeField] private AnimatedButton backButton;

        private CancellationTokenSource animationCts;

        private void Awake()
        {
            backButton.OnButtonClick.Subscribe(HandleBackButtonClick).AddTo(this);
        }

        private void HandleBackButtonClick(AnimatedButton animatedButton)
        {
            Close();
        }

        private async UniTask Close()
        {
            await PlayContainerAnimation(true);
            await screenScaleAnimation.PlayBackwardsAnimation();
            Navigator.Dequeue();
        }

        private void Start()
        {
            Open();
        }

        private async UniTask Open()
        {
            SetContainerHeight(0, false);

            await screenScaleAnimation.PlayAnimation();
            
            PlayContainerAnimation(false, 0.05f).Forget();
        }

        [Button]
        private void PlayForward()
        {
            PlayContainerAnimation(false).Forget();
        }
    
        [Button]
        private void PlayBackward()
        {
            PlayContainerAnimation(true).Forget();
        }
    
        private async UniTask PlayContainerAnimation(bool backwards, float delay = 0)
        {
            animationCts?.Cancel();
            animationCts = new CancellationTokenSource();
            var token = animationCts.Token;

            await UniTask.WaitForSeconds(delay, cancellationToken: token);

            var timeElapsed = 0f;
        
            if (animationDuration <= 0)
            {
                SetContainerHeight(1, backwards);
                return;
            }
        
            PlayScaleAnimation(backwards, token);

            try 
            {
                while (timeElapsed < animationDuration)
                {
                    timeElapsed += Time.unscaledDeltaTime;
                    var progress = Mathf.Clamp01(timeElapsed / animationDuration);
                
                    var curveInput = backwards ? 1f - progress : progress;
                    var curveValue = animationCurve.Evaluate(curveInput);
                
                    SetContainerHeight(curveValue, false);

                    await UniTask.Yield(PlayerLoopTiming.Update, token);
                }

            
                SetContainerHeight(1, backwards);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async Task PlayScaleAnimation(bool backwards, CancellationToken token)
        {
            await UniTask.WaitForSeconds(scaleAnimationDelay, cancellationToken: token);
            if (!backwards)
            {
                containerScaleAnimation.PlayAnimation();
            }
            else
            {
                containerScaleAnimation.PlayBackwardsAnimation();
            }
        }

        private void SetContainerHeight(float curveValue, bool backwards)
        {
            var t = backwards ? 1f - curveValue : curveValue;
        
            var height = Mathf.LerpUnclamped(minContainerHeight, maxContainerHeight, t);
        
            premiumContainer.sizeDelta = new Vector2(
                premiumContainer.sizeDelta.x, 
                height);
        }

        private void OnDestroy()
        {
            animationCts?.Cancel();
            animationCts?.Dispose();
            containerScaleAnimation.StopAnimation();
        }
    }
}
