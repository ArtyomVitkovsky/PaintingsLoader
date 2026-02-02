using UnityEngine;
using Zenject;

namespace GameTemplate.UI
{
    public abstract class UINavigator<TScreen> where TScreen : UIScreenBase
    {
        [Inject] protected UIScreenInstanceManager InstanceManager;
    }
}