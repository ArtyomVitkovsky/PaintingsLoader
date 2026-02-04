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
        private readonly UIStackNavigator _overlayNavigator;
        
        public BootstrapService(
            IScenesService scenesService,
            [Inject(Id = NavigatorIds.RootScreensNavigator)] UIStackNavigator rootNavigator,
            [Inject(Id = NavigatorIds.OverlayScreensNavigator)] UIStackNavigator overlayNavigator
            )
        {
            _scenesService = scenesService;
            _rootNavigator = rootNavigator;
            _overlayNavigator = overlayNavigator;
            
            Bootstrap();
        }

        public async UniTask Bootstrap()
        {
            Debug.Log("[Bootstrap] GameBootstrapper.Bootstrap() – initializing lifetime services...");

            _overlayNavigator.Push<SplashScreen>();

            await _scenesService.LoadScene(SceneNames.MAIN_SCENE);

            await UniTask.WaitForSeconds(1);
            
            _overlayNavigator.Pop();
            
            _rootNavigator.Push<MainMenuScreen>();
            
            Debug.Log("[Bootstrap] Game lifetime services initialization completed.");
        }
    }
}


