using UnityEngine;
using Zenject;

namespace EscapeMaze.Game {

    public class GameTimer : ITickable {

        [Inject] private GameSettings _gameSettings;
        [Inject] private GameManager _gameManager;
        
        private bool _pauseTimer = true;

        public float TimeCompleted { get; private set; }

        public void StartTimer() {
            UnPauseTimer();
            TimeCompleted = 0;
        }

        public void Tick() {
            if (IsTimerRunning) {
                TimeCompleted += Time.deltaTime;
                CheckAndFinishTimer();
            }
        }

        private void CheckAndFinishTimer() {
            if (IsTimerCompleted) {
                StopTimer();
                _gameManager.EndMatch(GameResult.Defeat);
            }
        }

        public float TimeLeft => _gameSettings.totalTime - TimeCompleted;
        
        private bool IsTimerCompleted => TimeCompleted >= _gameSettings.totalTime;

        public bool IsTimerRunning => !_pauseTimer;

        public void PauseTimer() {
            _pauseTimer = true;
        }

        public void UnPauseTimer() {
            _pauseTimer = false;
        }

        private void StopTimer() {
            PauseTimer();
            TimeCompleted = 0;
        }
    }
}
