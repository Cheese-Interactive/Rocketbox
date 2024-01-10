using System.Collections;
using UnityEngine;

public class Shooter : Enemy {
    // enemy that doesnt move and shoots at player (a turret)
    // speed is used for its rotation/aim speed

    private GameObject turret;
    private Vector3 direction;
    private Vector3 direction2D;
    [SerializeField] private Projectile projectile;
    private bool canShoot = true;

    protected override void seekPlayer() {
        turret = gameObject.transform.GetChild(0).gameObject;
        direction = player.transform.position - transform.position;
        direction2D = Vector3.ProjectOnPlane(direction, Vector3.forward);
        turret.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction2D); //considering: add (abstract?) lookAtPlayer method in Enemy
                                                                                           //todo: PrecisionShooter: slowly lerps rotation instead of just pointing
                                                                                           //has laser pointer (2d light) and when that is on the player, it shoots a very fast projectile
                                                                                           //dodging the pointer instead of the actual projectile
    }
    protected override void attack() {
        Vector3 loc = new Vector3(turret.transform.position.x, turret.transform.position.y, 0);
        Quaternion rot = turret.transform.rotation;
        if (canShoot)
            StartCoroutine(shootCooldown(Instantiate(projectile, loc, turret.transform.rotation).GetComponent<Projectile>().initialize()));
    }

    private IEnumerator shootCooldown(float time) {
        canShoot = false;
        yield return new WaitForSeconds(time);
        canShoot = true;
    }
    //taken from PlayerController

}


