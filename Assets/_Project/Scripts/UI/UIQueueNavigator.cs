using System.Collections.Generic;
using GameTemplate.UI;
using UnityEngine;

namespace GameTemplate.UI
{
    public class UIQueueNavigator : UINavigator<UIQueueScreen>
    {
        private Transform screensContainer;
        private List<UIQueueScreen> registeredScreens;
        private readonly Queue<UIQueueScreen> screenQueue = new Queue<UIQueueScreen>();
        private UIQueueScreen currentQueueScreen;

        private readonly Dictionary<System.Type, UIQueueScreen> prefabByType =
            new Dictionary<System.Type, UIQueueScreen>();

        protected UIQueueNavigator(List<UIQueueScreen> screens, Transform container)
        {
            registeredScreens = screens;
            screensContainer = container;

            foreach (UIQueueScreen screenPrefab in registeredScreens)
            {
                if (screenPrefab == null) continue;
                prefabByType[screenPrefab.GetType()] = screenPrefab;
            }
        }

        public T Enqueue<T>(System.Action<T> initializer = null) where T : UIQueueScreen
        {
            UIQueueScreen prefab = GetPrefabForType<T>();
            if (prefab == null) return null;

            UIQueueScreen instance = InstanceManager.CreateInstance(prefab, screensContainer, screen =>
            {
                screen.gameObject.SetActive(false);
                initializer?.Invoke((T)screen);
            });

            instance.OnCloseRequested += HandleScreenCloseRequested;
            screenQueue.Enqueue(instance);

            TryShowNext();

            return instance as T;
        }

        private void TryShowNext()
        {
            if (currentQueueScreen != null)
            {
                return;
            }

            if (screenQueue.Count == 0)
            {
                return;
            }

            currentQueueScreen = screenQueue.Dequeue();
            currentQueueScreen.gameObject.SetActive(true);
            currentQueueScreen.OnShow();
        }

        private void HandleScreenCloseRequested(UIScreenBase screen)
        {
            if (screen == currentQueueScreen)
            {
                currentQueueScreen.OnHide();
                currentQueueScreen.OnCloseRequested -= HandleScreenCloseRequested;
                InstanceManager.DestroyInstance(currentQueueScreen);
                currentQueueScreen = null;

                TryShowNext();
            }
        }

        public void ClearQueue()
        {
            if (currentQueueScreen != null)
            {
                currentQueueScreen.OnCloseRequested -= HandleScreenCloseRequested;
                InstanceManager.DestroyInstance(currentQueueScreen);
                currentQueueScreen = null;
            }

            while (screenQueue.Count > 0)
            {
                var screen = screenQueue.Dequeue();
                screen.OnCloseRequested -= HandleScreenCloseRequested;
                InstanceManager.DestroyInstance(screen);
            }
        }

        private UIQueueScreen GetPrefabForType<T>() where T : UIQueueScreen
        {
            return prefabByType.TryGetValue(typeof(T), out var prefab) ? prefab : null;
        }
    }
}