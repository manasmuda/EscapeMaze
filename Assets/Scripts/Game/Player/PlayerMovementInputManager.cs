using UnityEngine;


namespace EscapeMaze.Game.Player {

    public class PlayerMovementInputManager : MonoBehaviour {

        public float JoyStickXAxis { get; private set; }
        public float JoyStickYAxis { get; private set; }

        public Vector2 JoystickVector => new Vector2(JoyStickXAxis, JoyStickYAxis);

        private const float MouseSensitivity = 2f;

        public float MouseXAxis { get; private set; }
        public float MouseYAxis { get; private set; }

        void Update() {
            TakeInputAndUpdateJoystickAxisValues();
            AdjustJoystickAxisValues();

            TakeInputAndUpdateMouseAxisValues();
        }

#if UNITY_STANDALONE || UNITY_EDITOR

        private void TakeInputAndUpdateJoystickAxisValues() {
            if (Input.GetKey(KeyCode.W)) {
                JoyStickYAxis = 1f;
            } else if (Input.GetKey(KeyCode.S)) {
                JoyStickYAxis = -1f;
            } else {
                JoyStickYAxis = 0f;
            }

            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) {
                JoyStickXAxis = 0f;
            } else if (Input.GetKey(KeyCode.D)) {
                JoyStickXAxis = 1f;
            } else if (Input.GetKey(KeyCode.A)) {
                JoyStickXAxis = -1f;
            } else {
                JoyStickXAxis = 0f;
            }

            if (!(JoyStickXAxis == 0f && JoyStickYAxis == 0f)) {
                float magnitude = 1 / (Mathf.Sqrt(Mathf.Pow(JoyStickXAxis, 2) + Mathf.Pow(JoyStickYAxis, 2)));

                JoyStickXAxis *= magnitude;
                JoyStickYAxis *= magnitude;
            }
        }

#else

    private void TakeInputAndUpdateJoystickAxisValues() {
        
    }

#endif

#if UNITY_STANDALONE || UNITY_EDITOR

        private void TakeInputAndUpdateMouseAxisValues() {
            MouseXAxis = Input.GetAxis("Mouse X") * MouseSensitivity;
            MouseYAxis = Input.GetAxis("Mouse Y") * MouseSensitivity;
        }

#else

    private void TakeInputAndUpdateMouseAxisValues() {
        
    }

#endif

        private void AdjustJoystickAxisValues() {
            JoyStickXAxis = Mathf.Clamp(JoyStickXAxis, -1f, 1f);
            JoyStickYAxis = Mathf.Clamp(JoyStickYAxis, -1f, 1f);
        }

    }
}
