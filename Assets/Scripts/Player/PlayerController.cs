using UnityEngine;
using Zenject;

namespace EscapeMaze.Game.Player {

    public class PlayerController : MonoBehaviour {

        public Transform playerTransform;
        public Rigidbody playerRb;
        public Animator playerAnimator;

        private int _moveXId, _moveYId;

        [Inject] private PlayerMovementInputManager _playerMovementInputManager;
        [Inject] private PlayerSettings _playerSettings;

        private void Awake() {
            _moveXId = Animator.StringToHash("MoveX");
            _moveYId = Animator.StringToHash("MoveY");
        }

        void Update() {
            RotatePlayerByInput();
            MovePlayerByInput();
            AnimatePlayerByInput();
        }

        private void RotatePlayerByInput() {
            playerTransform.Rotate(Vector3.up * _playerMovementInputManager.MouseXAxis);
        }

        private void MovePlayerByInput() {
            float deltaMovementX = 0.75f * _playerMovementInputManager.JoyStickXAxis * _playerSettings.maxSpeed * Time.deltaTime;
            float deltaMovementY = _playerMovementInputManager.JoyStickYAxis * _playerSettings.maxSpeed * Time.deltaTime;
            if (deltaMovementY < 0) {
                deltaMovementY *= 0.75f;
            }

            playerRb.velocity = playerTransform.right * deltaMovementX + playerTransform.forward * deltaMovementY;
        }

        private void AnimatePlayerByInput() {

            playerAnimator.SetFloat(_moveXId, _playerMovementInputManager.JoyStickXAxis);
            playerAnimator.SetFloat(_moveYId, _playerMovementInputManager.JoyStickYAxis);
        }
    }
}
