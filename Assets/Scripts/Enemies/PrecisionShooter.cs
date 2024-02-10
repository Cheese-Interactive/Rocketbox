using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PrecisionShooter : Enemy {
    // enemy that doesnt move and shoots at player (a turret)
    // speed is used for its rotation/aim speed

    private GameObject turret;
    [SerializeField] private GameObject weapon;
    private bool shouldRotate = true;
    [SerializeField] private float lockOnTime;
    [SerializeField] private float cooldownTime;

    private Vector3 shootLoc;
    private Quaternion shootRot;

    [SerializeField] private GameObject lightObject;
    private Light2D light;
    private float lightMaxBright;
    //does not use hasAttackedOnce
    //does not use its weapon reference, kind of just a lazy thing from me, whatever
    //there was a bug where the projectile would shoot in the wrong direction so yea

    protected override void seekPlayer() {
        light = lightObject.GetComponent<Light2D>();
        turret = gameObject.transform.GetChild(0).gameObject;
        if (shouldRotate)
            turret.transform.Rotate(new Vector3(0, 0, speed * Time.deltaTime));

        //considering: add (abstract?) lookAtPlayer method in Enemy
        //todo: PrecisionShooter: slowly lerps rotation instead of just pointing
        //has laser pointer (2d light) and when that is on the player, it shoots a very fast projectile
        //dodging the pointer instead of the actual projectile
    }
    protected override void attack() {
        //in focusPlayer
    }

    public void recieveTrigger() {
        if (shouldRotate)
            StartCoroutine(focusPlayer(lockOnTime));
    }

    private IEnumerator focusPlayer(float time) {
        shouldRotate = false;
        //turret = gameObject.transform.GetChild(0).gameObject;
        Vector3 direction = player.transform.position - transform.position;
        Vector3 direction2D = Vector3.ProjectOnPlane(direction, Vector3.forward);
        while (time > 0f) {
            direction = player.transform.position - transform.position;
            direction2D = Vector3.ProjectOnPlane(direction, Vector3.forward);
            turret.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction2D);
            time -= Time.deltaTime;
            yield return null;
        }
        player.hitPlayer();
        lightMaxBright = light.intensity;
        yield return new WaitForSeconds(0.1f);
        light.intensity = 0;
        yield return new WaitForSeconds(cooldownTime);
        shouldRotate = true;
        light.intensity = lightMaxBright;
        speed = -speed;
    }


    //taken from PlayerController

}


