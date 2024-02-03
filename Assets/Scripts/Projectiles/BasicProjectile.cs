using System.Collections;
using UnityEngine;

public class BasicProjectile : Projectile {

    protected override IEnumerator travel(float direction) {
        transform.forward = new Vector3(0, 0, direction);
        rb.SetRotation(-direction);
        transform.eulerAngles = new Vector3(0, 0, -direction);
        if (direction == 180)
            transform.eulerAngles = new Vector3(0, 0, 0);
        if (direction == 0)
            transform.eulerAngles = new Vector3(0, 0, 180);
        rb.velocity = getDirectionVector(direction);

        while (!hasHitObject)
            yield return null;
        Destroy(gameObject);
    }

    protected override IEnumerator travel() { //uses the Quaternion passed in Initialize
        //lookAtThing(GameObject.Find("Player"));
        rb.velocity = getDirectionVector(GameObject.Find("Player"));

        while (!hasHitObject)
            yield return null;
        StartCoroutine(queueForDeletion());
    }

    /* protected override IEnumerator travel(Vector3 direction) {
        //transform.forward = direction;
        rb.velocity = direction * speed * 1000000000;

        while (!hasHitObject)
            yield return null;
        Destroy(gameObject);
    }*/
}
