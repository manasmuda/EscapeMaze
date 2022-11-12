using UnityEngine;

namespace EscapeMaze.Game {
    
    [RequireComponent(typeof(Collider))]
    public class GateEntryDetector : MonoBehaviour {

        public Gate gate;
        public Gate.DetectorType detectorType;

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                gate.OnPlayerEntered(detectorType);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.CompareTag("Player")) {
                gate.OnPlayerExited(detectorType);
            }
        }
    }
    
}
