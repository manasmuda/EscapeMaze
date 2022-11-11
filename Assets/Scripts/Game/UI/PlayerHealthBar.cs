using EscapeMaze.Game.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace EscapeMaze.Game.UI {

    public class PlayerHealthBar : MonoBehaviour {
        
        [SerializeField] private TextMeshProUGUI HealthText;
        [SerializeField] private Image HealthBarFill;

        [Inject] private SignalBus _signalBus;
        [Inject] private PlayerSettings _playerSettings;
        [Inject] private PlayerDataManager _playerDataManager;

        private void Start() {
            _signalBus.Subscribe<PlayerDataUpdatedSignal>(RenderPanel);

            RenderPanel();
        }

        private void RenderPanel() {
            HealthText.text = $"{_playerDataManager.GetHealth()}/{_playerSettings.health}";
            HealthBarFill.fillAmount = (float)_playerDataManager.GetHealth() / (float)_playerSettings.health;
        }
        
    }

}
