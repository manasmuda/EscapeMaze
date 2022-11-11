using EscapeMaze.Game.Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace EscapeMaze.Game.UI {

    public class KeyCollectionPanel : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI KeysText;

        [Inject] private SignalBus _signalBus;
        [Inject] private GameSettings _gameSettings;
        [Inject] private PlayerDataManager _playerDataManager;

        private void Start() {
            _signalBus.Subscribe<PlayerDataUpdatedSignal>(RenderPanel);

            RenderPanel();
        }

        private void RenderPanel() {
            var keys = _playerDataManager.GetCollectedKeys();
            KeysText.text = $"KEYS: {keys.Count}/{_gameSettings.totalKeys}";
        }
    }
}
