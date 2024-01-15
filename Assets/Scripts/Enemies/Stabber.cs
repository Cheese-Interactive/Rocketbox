using System.Collections;
using UnityEngine;

public class Stabber : Enemy {
    //Dashes in the direction of the player every few seconds
    //speed acts as launch force

    [SerializeField] private float dashCooldown;
    private bool shouldDash = true;
    private Vector3 direction;
    private Vector3 direction2D;

    protected override void attack() {
        if (shouldDash && hasAttackedOnce) {
            StartCoroutine(dashWait(dashCooldown));
            rb.AddForce(transform.right * speed * rb.mass);
        }
        else if (!hasAttackedOnce) {
            StartCoroutine(dashWait(dashCooldown * Random.Range(0.2f, 0.4f)));
            hasAttackedOnce = true;
        }
    }

    protected override void seekPlayer() {
        direction = player.transform.position - transform.position;
        direction2D = Vector3.ProjectOnPlane(direction, Vector3.forward);
        transform.rotation = Quaternion.FromToRotation(Vector3.right, direction2D);
        //this code is taken directly from LightPointAtPlayer, more explanation in there
        //there are some minor differences that dont effect functionality
    }


    protected IEnumerator dashWait(float seconds) {
        shouldDash = false;
        yield return new WaitForSeconds(seconds);
        rb.totalForce = new Vector2(0, 0);   //dont want it to be sliding around trying to lock onto player
        rb.velocity = new Vector2(0, 0);     //makes dashing snappier, otherwise its more like a slip than a dash
        shouldDash = true;
    }


}
