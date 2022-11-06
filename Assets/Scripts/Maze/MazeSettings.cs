using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;

namespace EscapeMaze.Maze {

    public class MazeSettings : ScriptableObjectInstaller<MazeSettings> {

        public List<AssetReferenceT<MazePreset>> mazePresets;
        public int mazeSize;

        public const int MazeWallSize = 6;

        public override void InstallBindings() {
            Container.BindInstance(this).AsSingle();
        }

        public async UniTask<MazePreset> GetRandomMazePreset() {
            return await mazePresets.GetRandomElement().LoadAssetAsync();
        }
        
    }
}
