using Cysharp.Threading.Tasks;
using EscapeMaze.StateMachine;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Random = UnityEngine.Random;

namespace EscapeMaze.Game.Bot {

    public class ShootingBot : MonoBehaviour, IStateMachine<ShootingBot.State, Transform> {

        public NavMeshAgent navMeshAgent;

        public SphereCollider patrolCollider, shootingCollider;

        public GameObject bulletPrefab;
        public Transform gunPos;
        
        private float _curShotTime;
        private Vector3 _destination;

        private int _shootingMaskLayer;

        private Transform _myTransform;
        private Transform _target;

        [Inject] private DiContainer _container;
        [Inject] private ShootingBotSettings _shootingBotSettings;

        public enum State {
            FindDest, // Finding random destination in Range
            Patrol, // Moving to random destination selected
            Chasing, //Chasing player 
            Aiming, //Aiming at player
            Shooting, // Shooting player
        }

        public State CurrentState { get; private set; }

        private void Awake() {
            _myTransform = transform;
            _shootingMaskLayer = LayerMask.GetMask("Player", "Maze");
        }

        private void Start() {
            InitializeBot();
        }

        private async void InitializeBot() {
            await UniTask.Delay(100);
            if (navMeshAgent.isOnNavMesh) {
                EnterState(State.FindDest, null);
            } else {
                Debug.LogError("Not on NavMesh");
            }
        }

        private void Update() {
            if (CurrentState < State.Chasing) {
                if (!navMeshAgent.pathPending) {
                    if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
                        if (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f) {
                            EnterState(State.FindDest, null);
                        }
                    }
                }
            }

            if (CurrentState == State.Shooting) {
                _curShotTime += Time.deltaTime;
                if (_curShotTime >= _shootingBotSettings.BulletDelay) {
                    Shoot();
                }
            }
        }

        public void EnterState(State state, Transform targetTransform) {
            SetState(state);
            _target = targetTransform;
            _destination = new Vector3(0, 100, 0);
            switch (state) {
                case State.FindDest:
                    EnterFindDestState();
                    break;
                case State.Patrol:
                    EnterPatrolState();
                    break;
                case State.Chasing:
                    EnterChaseState(targetTransform);
                    break;
                case State.Aiming:
                    EnterAimingState(targetTransform);
                    break;
                case State.Shooting:
                    EnterShootingState(targetTransform);
                    break;
            }
        }

        private void EnterFindDestState() {
            float radius = patrolCollider.radius;
            float randomX = Random.Range(-1f * radius, radius);
            float randomY = Random.Range(-1f * radius, radius);
            Vector3 destination = _myTransform.position + new Vector3(randomX, 0f, randomY);

            navMeshAgent.SetDestination(destination);

            EnterState(State.Patrol, null);
        }

        private void EnterPatrolState() {
            _destination = navMeshAgent.destination;
        }

        private void EnterChaseState(Transform target) {
            if (target != null) {
                _target = target;
                var targetPosition = _target.position;
                _destination = new Vector3(targetPosition.x, _myTransform.position.y, targetPosition.z);
                navMeshAgent.SetDestination(_destination);
            } else {
                EnterState(State.FindDest, null);
            }
        }

        private void EnterAimingState(Transform target) {
            if (target != null) {
                _target = target;
                navMeshAgent.SetDestination(_myTransform.position);
                _myTransform.rotation = Quaternion.LookRotation(_target.position - _myTransform.position);
                if (Physics.Raycast(transform.position + transform.up, transform.forward, out RaycastHit hit, shootingCollider.radius, _shootingMaskLayer)) {
                    if (hit.collider.CompareTag("Player")) {
                        EnterState(State.Shooting, _target);
                    } else {
                        EnterState(State.Chasing, _target);
                    }
                } else {
                    EnterState(State.Chasing, _target);
                }
            } else {
                EnterState(State.FindDest, null);
            }
        }

        private void EnterShootingState(Transform target) {
            if (target != null) {
                _target = target;
                navMeshAgent.SetDestination(_myTransform.position);
                Shoot();
            } else {
                EnterState(State.FindDest, null);
            }
        }

        private void Shoot() {
            _curShotTime = 0;
            if (Physics.Raycast(transform.position + transform.up, transform.forward, out RaycastHit hit,
                    shootingCollider.radius, _shootingMaskLayer)) {
                if (hit.collider.CompareTag("Player")) {
                    GameObject temp = _container.InstantiatePrefab(bulletPrefab, new GameObjectCreationParameters() {
                        Position = gunPos.position
                    });
                    temp.GetComponent<Rigidbody>().AddForce(gunPos.forward * (_shootingBotSettings.bulletForce), ForceMode.Impulse);
                    Destroy(temp, 1f);
                } else {
                    EnterState(State.Aiming, _target);
                }
            } else {
                EnterState(State.Aiming, _target);
            }
        }

        public void SetState(State state) {
            CurrentState = state;
        }

        public void Exit() {
            if (CurrentState == State.Chasing) {
                EnterState(State.FindDest, null);
            } else if (CurrentState == State.Aiming) {
                if (_target != null) {
                    EnterState(State.Chasing, _target);
                } else {
                    EnterState(State.FindDest, null);
                }
            } else if (CurrentState == State.Shooting) {
                if (_target != null) {
                    EnterState(State.Aiming, _target);
                } else {
                    EnterState(State.FindDest, null);
                }
            } else {
                EnterState(State.FindDest, null);
            }
        }
    }
}
