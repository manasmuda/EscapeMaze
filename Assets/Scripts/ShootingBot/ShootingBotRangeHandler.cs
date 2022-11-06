using EscapeMaze.StateMachine;
using UnityEngine;
using Zenject;

namespace EscapeMaze.Game.Bot {


    [RequireComponent(typeof(Collider))]
    public class ShootingBotRangeHandler : MonoBehaviour {

        public bool enterTrigger;
        public ShootingBot.State enterState;
        public bool exitTrigger;

        [Inject] private IStateMachine<ShootingBot.State, Transform> _stateMachine;

        private void OnTriggerEnter(Collider other) {
            //Debug.LogError("Trigger:"+other.tag+","+other.gameObject.name);
            if (enterTrigger) {
                if (other.CompareTag("Player")) {
                    _stateMachine.EnterState(enterState, other.transform);
                }
            }
        }

        private void OnTriggerExit(Collider other) {
            if (exitTrigger) {
                if (other.CompareTag("Player")) {
                    _stateMachine.Exit();
                }
            }
        }
    }
}