using UnityEngine;

namespace _Project.Scripts.Utils
{
    public enum SafeMode
    {
        All,
        OnlyTop,
        OnlyBottom,
        TopAndBottom
    }
    
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaFitter : MonoBehaviour
    {
        [SerializeField] private SafeMode mode = SafeMode.All;

        private RectTransform _rectTransform;
        private Rect _lastSafeArea = new Rect(0, 0, 0, 0);

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            Refresh();
        }

        private void Update()
        {
            if (_lastSafeArea != UnityEngine.Device.Screen.safeArea)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            Rect safeArea = UnityEngine.Device.Screen.safeArea;
            _lastSafeArea = safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= UnityEngine.Device.Screen.width;
            anchorMin.y /= UnityEngine.Device.Screen.height;
            anchorMax.x /= UnityEngine.Device.Screen.width;
            anchorMax.y /= UnityEngine.Device.Screen.height;

            switch (mode)
            {
                case SafeMode.OnlyTop:
                    anchorMin.x = 0;
                    anchorMin.y = 0;
                    anchorMax.x = 1;
                    break;

                case SafeMode.OnlyBottom:
                    anchorMin.x = 0;
                    anchorMax.x = 1;
                    anchorMax.y = 1;
                    break;

                case SafeMode.TopAndBottom:
                    anchorMin.x = 0;
                    anchorMax.x = 1;
                    break;

                case SafeMode.All:
                    break;
            }

            _rectTransform.anchorMin = anchorMin;
            _rectTransform.anchorMax = anchorMax;

            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;
        }
    }
}