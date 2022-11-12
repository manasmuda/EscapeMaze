using DG.Tweening;
using EscapeMaze.Game.Player;
using UnityEngine;
using Zenject;

namespace EscapeMaze.Game {

    public class Gate : MonoBehaviour {
        
        [Inject] private PlayerDataManager _playerDataManager;
        [Inject] private GameManager _gameManager;

        public Transform gateTransform;

        private Vector3 orgPos;

        private void Start() {
            orgPos = gateTransform.position;
        }

        public void OnPlayerEntered(DetectorType type) {
            if (type == DetectorType.Entry) {
                if (_playerDataManager.HasCollectedAllKeys()) {
                    OpenGate();
                }
            } else if(type == DetectorType.Exit){
                _gameManager.EndMatch(GameResult.Victory);
            }
        }

        private void OpenGate() {
            gateTransform.DOMove(gateTransform.position - gateTransform.forward * 5, 1f);
        }

        private void CloseGate() {
            gateTransform.DOMove(orgPos, 1f);
        }

        public void OnPlayerExited(DetectorType type) {
            if (type == DetectorType.Entry) {
                CloseGate();
            } 
        }
        
        public enum DetectorType {
            Entry,
            Exit
        }
    }
}
