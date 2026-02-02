using System;
using UnityEngine;

namespace GameTemplate.UI
{
    public abstract class UIScreenBase : MonoBehaviour
    {
        [Header("Screen Settings")]
        [SerializeField] private bool isPoolable = false;

        public event Action<UIScreenBase> OnCloseRequested;

        public bool IsPoolable => isPoolable;

        public virtual void Initialize()
        {
        }

        public virtual void OnShow()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnHide()
        {
            gameObject.SetActive(false);
        }

        public void RequestClose()
        {
            OnCloseRequested?.Invoke(this);
        }
    }
}


