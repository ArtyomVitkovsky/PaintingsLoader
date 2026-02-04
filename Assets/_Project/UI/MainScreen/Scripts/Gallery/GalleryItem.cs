using System;
using System.Threading;
using _Project.Scripts.Core.Services.AssetsProvider;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.UI.MainScreen.Scripts
{
    public class GalleryItem : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RawImage image;
        [SerializeField] private LoaderView loader;
        [SerializeField] private GameObject premiumBadge;
        [SerializeField] private AnimatedButton selectionButton;

        private TextureAssetProvider _assetProvider;

        private int currentId;
        private bool isLoaded;
        
        public bool IsPremium { get; private set; }

        public RectTransform RectTransform => rectTransform;
        
        public AnimatedButton SelectionButton => selectionButton;
        public Texture Texture { get; private set; }

        [Inject]
        public void Construct(TextureAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        public void SetPremiumStatus(bool isPremium)
        {
            IsPremium = isPremium;
            
            premiumBadge.SetActive(isPremium);
        }
        
        public async UniTask LoadImage(int id, string url, CancellationToken token)
        {
            if (currentId == id && isLoaded)
            {
                return;
            }

            currentId = id;
            image.texture = null;
            isLoaded = false;
        
            loader.Show();
        
            Texture = await _assetProvider.GetTexture(url, token);
            if (Texture != null && !token.IsCancellationRequested)
            {
                image.texture = Texture;
                isLoaded = true;
            
                if (currentId == id)
                {
                    loader.Hide();
                }
            }
        }

        public void SetActive(bool isActive)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.interactable = isActive;
            canvasGroup.blocksRaycasts = isActive;
        }
    }
}