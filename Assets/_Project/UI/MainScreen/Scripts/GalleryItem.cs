using System.Threading;
using _Project.Scripts.Core.Services.AssetsProvider;
using Cysharp.Threading.Tasks;
using TMPro;
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

        private TextureAssetProvider _assetProvider;

        private int currentId;
        private bool isLoaded;

        public RectTransform RectTransform => rectTransform;

        [Inject]
        public void Construct(TextureAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
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
    
        private bool IsVisible(RectTransform viewport)
        {
            var center = transform.position;
        
            return RectTransformUtility.RectangleContainsScreenPoint(viewport, center, null);
        }
    }
}