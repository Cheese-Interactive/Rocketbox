using UnityEngine;

public class DualLaser : Weapon {
    [SerializeField]
    private float gap;

    override public float shoot(Vector3 pos, float degrees) {
        if (degrees == 0 || degrees == 180) {
            pos = new Vector3(pos.x, pos.y + gap, pos.z);
            Instantiate(projectile, pos, Quaternion.identity).GetComponent<Projectile>().initialize(degrees);
            pos = new Vector3(pos.x, pos.y - gap, pos.z);
            Instantiate(projectile, pos, Quaternion.identity).GetComponent<Projectile>().initialize(degrees);
        }
        else if (degrees == 90 || degrees == 270) {
            pos = new Vector3(pos.x + gap, pos.y, pos.z);
            Instantiate(projectile, pos, Quaternion.identity).GetComponent<Projectile>().initialize(degrees);
            pos = new Vector3(pos.x - gap, pos.y, pos.z);
            Instantiate(projectile, pos, Quaternion.identity).GetComponent<Projectile>().initialize(degrees);
        }
        return cooldown;
    }

    override public float shoot(Vector3 pos, Quaternion angle) {
        //unsupported
        //Instantiate(projectile, pos, angle).GetComponent<Projectile>().initialize();
        return cooldown;
    }
}

