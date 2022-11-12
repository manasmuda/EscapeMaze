using EscapeMaze.Game;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider))]
public class ShootingBotBullet : MonoBehaviour {

    [Inject] private SignalBus _signalBus;
    [Inject] private ShootingBotSettings _shootingBotSettings;
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            _signalBus.Fire(new BulletPlayerHitSignal(){ damage = _shootingBotSettings.bulletDamage});
            Destroy(gameObject);
        } else if(other.CompareTag("Maze")){
            Destroy(gameObject);
        }
    }
}
