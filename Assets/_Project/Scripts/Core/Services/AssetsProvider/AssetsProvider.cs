using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Core.Services.AssetsProvider
{
    public interface IAssetsProvider<TAsset, in TContainer>
    {
        public UniTask<TAsset> GetAsset(TContainer assetContainer, CancellationToken token);
    }
}