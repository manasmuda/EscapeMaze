using UnityEngine;

namespace EscapeMaze {

    public class FollowObject : MonoBehaviour {

        public Vector3 positionOffset, rotationOffset;

        public Transform followTransform;
        private Transform _transform;

        void Awake() {
            _transform = transform;
        }

        void Update() {
            _transform.position = followTransform.position + positionOffset;
            Vector3 rotationAngle = followTransform.rotation.eulerAngles + rotationOffset;
            _transform.rotation = Quaternion.Euler(rotationAngle);
        }
    }
}
