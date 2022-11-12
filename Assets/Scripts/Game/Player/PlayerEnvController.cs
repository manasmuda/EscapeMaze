using UnityEngine;
using Zenject;

namespace EscapeMaze.Game.Player {
    public class PlayerEnvController : MonoBehaviour {
        public GameObject player;
        private Transform _playerTransform;

        private void Awake() {
            _playerTransform = player.transform;
        }

        public void SetPlayerPosition(Vector3 position) {
            _playerTransform.position = new Vector3(position.x, _playerTransform.position.y, position.z);
        }
    }
}


