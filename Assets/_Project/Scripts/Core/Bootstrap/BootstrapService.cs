using _Project.Scripts.Constants;
using _Project.Scripts.Core.Services;
using _Project.UI.MainScreen;
using _Project.UI.PremiumPopup;
using Cysharp.Threading.Tasks;
using GameTemplate.UI;
using UnityEngine;
using Zenject;

namespace GameTemplate.Core.Bootstrap
{
    public interface IBootstrapService
    {
        UniTask Bootstrap();
    }

    public class BootstrapService : IBootstrapService
    {
        private readonly IScenesService _scenesService;
        private readonly UIStackNavigator _rootNavigator;
        
        public BootstrapService(
            IScenesService scenesService,
            [Inject(Id = NavigatorIds.RootScreensNavigator)] UIStackNavigator rootNavigator
            )
        {
            _scenesService = scenesService;
            _rootNavigator = rootNavigator;
            
            Bootstrap();
        }

        public async UniTask Bootstrap()
        {
            Debug.Log("[Bootstrap] GameBootstrapper.Bootstrap() – initializing lifetime services...");

            await _scenesService.LoadScene(SceneNames.MAIN_SCENE);
            _rootNavigator.Push<MainMenuScreen>();
            
            Debug.Log("[Bootstrap] Game lifetime services initialization completed.");
        }
    }
}


