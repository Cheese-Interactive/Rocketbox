using UnityEngine;

public class TheCirculatorBlaster : Weapon {
    [SerializeField] private float bloom;
    private float bloomAngle;
    [SerializeField] private int projectilesToShoot;

    //this is designed in a way that it only works with The Circulator
    override public float shoot(Vector3 pos, float degrees) {

        bloomAngle = bloom / projectilesToShoot;
        if (degrees == 0)
            for (int i = 0; i < projectilesToShoot; i++) {
                Instantiate(projectile, pos, Quaternion.identity).GetComponent<Projectile>().initialize(degrees + bloomAngle - (bloomAngle * i));
            }
        else if (degrees == 180)
            for (int i = 0; i < projectilesToShoot; i++) {
                Instantiate(projectile, pos, Quaternion.identity).GetComponent<Projectile>().initialize(degrees - bloomAngle + (bloomAngle * i));
            }



        bloomAngle = bloom / projectilesToShoot;
        return cooldown;
    }

    //partially ai generated
    //i do not know how to work with quaternions so i used ai for that
    override public float shoot(Vector3 pos, Quaternion angle) {
        bloomAngle = bloom / projectilesToShoot;
        Vector3 euler = angle.eulerAngles;
        if (euler.z == 0)
            for (int i = 0; i < projectilesToShoot; i++) {
                Instantiate(projectile, pos, Quaternion.identity).GetComponent<Projectile>().initialize(euler.z + bloom - bloomAngle * i);
            }
        else if (euler.z == 180)
            for (int i = 0; i < projectilesToShoot; i++) {
                euler.z += bloomAngle;
                Instantiate(projectile, pos, Quaternion.identity).GetComponent<Projectile>().initialize(euler.z - bloom + bloomAngle * i);
            }

        bloomAngle = bloom / projectilesToShoot;
        return cooldown;
    }
}
