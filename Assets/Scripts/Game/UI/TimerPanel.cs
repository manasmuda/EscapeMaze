using TMPro;
using UnityEngine;
using Zenject;

namespace EscapeMaze.Game.UI {
    
    public class TimerPanel : MonoBehaviour {

        public TextMeshProUGUI timerText;

        [Inject] private GameTimer _gameTimer;

        private void Update() {
            if (_gameTimer.IsTimerRunning) {
                RenderTimer((int)_gameTimer.TimeLeft);
            }
        }

        private void RenderTimer(int timeLeft) {
            if (timeLeft > 0) {
                int minutes = timeLeft / 60;
                int seconds = timeLeft % 60;

                timerText.text = $"{minutes}:{seconds}";
            }
        }
    }
}