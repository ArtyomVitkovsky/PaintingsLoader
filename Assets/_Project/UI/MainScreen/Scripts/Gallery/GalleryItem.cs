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

        [Inject]
        public void Construct(TextureAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
        }

        private void HandleSelectionButtonClick(AnimatedButton button)
        {
            throw new NotImplementedException();
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
        
            var texture = await _assetProvider.GetTexture(url, token);
            if (texture != null && !token.IsCancellationRequested)
            {
                image.texture = texture;
                isLoaded = true;
            
                if (currentId == id)
                {
                    loader.Hide();
                }
            }
        }
    }
}