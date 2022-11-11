using EscapeMaze.Game.Player;
using EscapeMaze.Maze;
using UnityEngine;
using Zenject;

namespace EscapeMaze.Game {

    public class GameInstaller : MonoInstaller {

        public MazeController mazeController;

        public override void InstallBindings() {

            Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();

            Container.BindInstance(mazeController).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerDataManager>().AsSingle();

            Container.DeclareSignal<PlayerDiedSignal>();
            Container.DeclareSignal<BulletPlayerHitSignal>();
            Container.DeclareSignal<KeyCollectedSignal>();
            
            Container.DeclareSignal<PlayerDataUpdatedSignal>();

            Debug.Log("Game Installed");
        }
    }

    public struct PlayerDataUpdatedSignal { }
    
    public struct PlayerDiedSignal { }

    public struct BulletPlayerHitSignal {
        public int damage;
    }

    public struct KeyCollectedSignal {
        public Key key;
    }
}
