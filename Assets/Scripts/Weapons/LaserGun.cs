using UnityEngine;

public class LaserGun : Weapon {


    override public float shoot(Vector3 pos, float degrees) {
        Instantiate(projectile, pos, Quaternion.identity).GetComponent<Projectile>().initialize(degrees);
        return cooldown;
    }

    override public float shoot(Vector3 pos, Quaternion angle) {
        Instantiate(projectile, pos, angle).GetComponent<Projectile>().initialize();
        return cooldown;
    }
}
