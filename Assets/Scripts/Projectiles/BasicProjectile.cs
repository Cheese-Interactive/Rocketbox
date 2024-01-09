using System.Collections;
using UnityEngine;

public class BasicProjectile : Projectile {

    protected override IEnumerator travel(float direction) {
        transform.forward = new Vector3(0, 0, direction);
        rb.SetRotation(direction);
        rb.velocity = getDirectionVector(direction);

        while (!hasHitObject)
            yield return null;
        Destroy(gameObject);
    }

    /* protected override IEnumerator travel(Vector3 direction) {
        //transform.forward = direction;
        rb.velocity = direction * speed * 1000000000;

        while (!hasHitObject)
            yield return null;
        Destroy(gameObject);
    }*/
}
