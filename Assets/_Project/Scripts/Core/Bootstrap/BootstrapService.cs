using _Project.Scripts.Constants;
using _Project.Scripts.Core.Services;
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
        
        public BootstrapService(
            IScenesService scenesService
            )
        {
            _scenesService = scenesService;
            
            Bootstrap();
        }

        public async UniTask Bootstrap()
        {
            Debug.Log("[Bootstrap] GameBootstrapper.Bootstrap() – initializing lifetime services...");
            
            Debug.Log("[Bootstrap] Game lifetime services initialization completed.");
        }
    }
}


