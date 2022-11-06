using Cysharp.Threading.Tasks;
using EscapeMaze.Game.Player;
using EscapeMaze.Maze;
using UnityEngine;
using Zenject;

namespace EscapeMaze.Game {

    public class GameManager : IInitializable {

        [Inject] private MazeInitializer _mazeInitializer;
        [Inject] private DiContainer _container;

        private AssetAddressReference<GameObject> _playerEnvPrefabReference;
        private AssetAddressReference<GameObject> _shootingBotPrefabReference;

        public void Initialize() {
            _shootingBotPrefabReference = new AssetAddressReference<GameObject>("ShootingBot");
            _playerEnvPrefabReference = new AssetAddressReference<GameObject>("PlayerEnvironmentPrefab");
            
            InitializeGame();
        }

        private async void InitializeGame() {
            await _mazeInitializer.InitializeMaze();
            await SpawnPlayer();
            await SpawnBots();
            Debug.Log("Game Initialized");
        }

        private async UniTask SpawnPlayer() {
            Vector3 position = _mazeInitializer.GetRandomMazePosition(MazeDirection.South);
            GameObject playerEnvObj = await _playerEnvPrefabReference.TryLoadAndInstantiateGameObject(null, _container);
            PlayerEnvController playerEnvController = playerEnvObj.GetComponent<PlayerEnvController>();
            playerEnvController.SetPlayerPosition(position);
        }

        private async UniTask SpawnBots() {
            GameObject botPrefab = await _shootingBotPrefabReference.TryGetOrLoadObjectAsync();
            float botPosY = botPrefab.transform.position.y;
            for (int i = 0; i < 4; i++) {
                Vector3 position = _mazeInitializer.GetRandomMazePosition();
                position.y = botPosY;
                GameObject bot = _container.InstantiatePrefab(botPrefab);
                bot.transform.position = position;
            }
        }

        private async UniTask SetExitGate() {
            
        }

    }

}
