public class ShootingBotSettings : SelfScriptableObjectInstaller<ShootingBotSettings>
{
    public int bulletDamage; //HP
    public int bulletForce; // in unit/second2
    public float bulletFrequency; //per second

    public float BulletDelay => 1f / (float)bulletFrequency;
}
