using GameTemplate.Core.Bootstrap;
using GameTemplate.UI;
using Zenject;

namespace _Project.Scripts.Core.Services
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            UIScreenInstanceManagerInstaller.Install(Container);
            
            SceneServiceInstaller.Install(Container);
            
            BootstrapServiceInstaller.Install(Container);
        }
    }
}