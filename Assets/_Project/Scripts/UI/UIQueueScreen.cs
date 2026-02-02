using UnityEngine;

namespace GameTemplate.UI
{
    public abstract class UIQueueScreen : UIScreenBase
    {
        [Header("Queue Screen Settings")]
        [SerializeField] private bool autoCloseOnShowComplete = false;

        public bool AutoCloseOnShowComplete => autoCloseOnShowComplete;

        protected void NotifyCompleted()
        {
            RequestClose();
        }
    }
}


