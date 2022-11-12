using System;
using System.Collections.Generic;
using Zenject;

namespace EscapeMaze.Game.Player {

    public class PlayerDataManager : IInitializable, IDisposable {

        [Inject] private SignalBus _signalBus;
        [Inject] private PlayerSettings _playerSettings;
        [Inject] private GameSettings _gameSettings;

        private int _health;
        private List<Key> _collectedKeys = new List<Key>();

        public void Initialize() {
            _signalBus.Subscribe<BulletPlayerHitSignal>(OnBulletHit);
            _signalBus.Subscribe<KeyCollectedSignal>(OnKeyCollected);
        }

        private void OnBulletHit(BulletPlayerHitSignal signal) {
            if (signal.damage > 0) {
                _health -= signal.damage;
                
                OnPlayerDataUpdated();

                if (_health <= 0) {
                    _signalBus.Fire<PlayerDiedSignal>();
                }
            }
        }

        public int GetHealth() {
            return _health;
        }

        private void OnKeyCollected(KeyCollectedSignal signal) {
            if (signal.key != null) {
                _collectedKeys.Add(signal.key);
                
                OnPlayerDataUpdated();
            }
        }

        public List<Key> GetCollectedKeys() {
            return _collectedKeys;
        }

        public bool HasCollectedAllKeys() {
            return _collectedKeys.Count == _gameSettings.totalKeys;
        }

        public void ResetPlayerData() {
            _health = _playerSettings.health;
            _collectedKeys.Clear();
            OnPlayerDataUpdated();
        }

        private void OnPlayerDataUpdated() {
            _signalBus.Fire<PlayerDataUpdatedSignal>();
        }
        
        public void Dispose() {
            _signalBus.TryUnsubscribe<BulletPlayerHitSignal>(OnBulletHit);
        }
    }

}
