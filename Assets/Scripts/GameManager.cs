using System;
using Cysharp.Threading.Tasks;
using EscapeMaze.Game.Player;
using EscapeMaze.Maze;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace EscapeMaze.Game {

    public class GameManager : IInitializable, IDisposable {

        [Inject] private MazeController _mazeController;
        [Inject] private DiContainer _container;

        [Inject] private GameSettings _gameSettings;
        [Inject] private SignalBus _signalBus;

        [Inject] private GameTimer _gameTimer;

        [Inject] private PlayerDataManager _playerDataManager;
        [Inject] private MatchResultPage _matchResultPage;
        
        private PlayerEnvController _playerEnvController;
        
        #region Asset References

        private readonly AssetAddressReference<GameObject> _playerEnvPrefabReference = new ("PlayerEnvironmentPrefab");
        private readonly AssetAddressReference<GameObject> _shootingBotPrefabReference = new ("ShootingBot");
        private readonly AssetAddressReference<GameObject> _gatePrefabReference = new("GatePrefab");
        private readonly AssetAddressReference<GameObject> _keyPrefabReference = new("KeyPrefab");

        #endregion

        public void Initialize() {
            SubscribeSignals();
            
            InitializeGame();
        }

        private void SubscribeSignals() {
            _signalBus.Subscribe<PlayerDiedSignal>(OnPlayerDied);
        }

        #region Game Initialization

        private async void InitializeGame() {
            await _mazeController.InitializeMaze();
            await UniTask.WhenAll(SpawnPlayer(), SpawnBots(), SetExitGate(), SpawnKeys());
            _gameTimer.StartTimer();
            Debug.Log("Game Initialized");
        }

        private async UniTask SpawnPlayer() {
            Vector3 position = _mazeController.GetRandomMazePosition(MazeDirection.South);
            GameObject playerEnvObj = await _playerEnvPrefabReference.TryLoadAndInstantiateGameObject(null, _container);
            _playerEnvController = playerEnvObj.GetComponent<PlayerEnvController>();
            _playerEnvController.SetPlayerPosition(position);
            _playerDataManager.ResetPlayerData();
        }

        private async UniTask SpawnBots() {
            GameObject botPrefab = await _shootingBotPrefabReference.TryGetOrLoadObjectAsync();
            float botPosY = botPrefab.transform.position.y;
            for (int i = 0; i < _gameSettings.totalShootingBots; i++) {
                Vector3 position = _mazeController.GetRandomMazePosition();
                position.y = botPosY;
                _container.InstantiatePrefab(botPrefab,  new GameObjectCreationParameters() {
                    Position = position
                });
            }
        }

        private async UniTask SetExitGate() {
            GameObject gatePrefab = await _gatePrefabReference.TryGetOrLoadObjectAsync();
            MazeCell.Wall wall = _mazeController.GetRandomMazeBorderWall(MazeDirection.North);
            Transform wallObjTransform = wall.GetWallObj().transform;
            Vector3 position = wallObjTransform.position;
            position.y = gatePrefab.transform.position.y;
            Vector3 rotation = Vector3.zero;
            rotation.y = 180 + ((int) wall.direction - 1) * 90;
            _container.InstantiatePrefab(gatePrefab, new GameObjectCreationParameters() {
                Position = position,
                Rotation = Quaternion.Euler(rotation)
            });
            Object.Destroy(wall.GetWallObj());
        }

        private async UniTask SpawnKeys() {
            GameObject keyPrefab = await _keyPrefabReference.TryGetOrLoadObjectAsync();
            for (int i = 0; i < _gameSettings.totalKeys; i++) {
                Vector3 position = _mazeController.GetRandomMazePosition();
                Key key = _container.InstantiatePrefab(keyPrefab).GetComponent<Key>();
                key.SpawnKey(position);
            }
        }
        
        #endregion

        private void OnPlayerDied() {
            Vector3 playerPosition = _mazeController.GetRandomMazePosition(MazeDirection.South);
            _playerEnvController.SetPlayerPosition(playerPosition);
            var collectedKeys = _playerDataManager.GetCollectedKeys();
            foreach (var key in collectedKeys) {
                Vector3 position = _mazeController.GetRandomMazePosition();
                key.SpawnKey(position);
            }
            _playerDataManager.ResetPlayerData();
        }

        public void EndMatch(GameResult result) {
            _gameTimer.PauseTimer();
            Object.Destroy(_playerEnvController.gameObject);
            _matchResultPage.ShowMatchResult(result);
        }
        
        private void UnloadAssets() {
            _shootingBotPrefabReference.Unload();
            _playerEnvPrefabReference.Unload();
            _keyPrefabReference.Unload();
            _gatePrefabReference.Unload();
        }

        private void UnsubscribeSignals() {
            _signalBus.TryUnsubscribe<PlayerDiedSignal>(OnPlayerDied);
        }
        
        public void Dispose() {
            UnsubscribeSignals();
            
            UnloadAssets();
        }

    }

}
