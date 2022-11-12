using System;
using UnityEngine;
using Zenject;

namespace EscapeMaze.Game {

    [RequireComponent(typeof(Collider))]
    public class Key : MonoBehaviour {

        [Inject] private SignalBus _signalBus;

        private Transform _myTransform;

        private void Awake() {
            _myTransform = transform;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Player")) {
                _signalBus.Fire(new KeyCollectedSignal(){ key = this});
                HideKey();
            }
        }

        public void SpawnKey(Vector3 position) {
            gameObject.SetActive(true);
            position.y = _myTransform.position.y;
            _myTransform.position = position;
        }

        public void HideKey() {
            gameObject.SetActive(false);
        }
    }
    
    
}