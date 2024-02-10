using System.Collections;
using UnityEngine;

public class PhysicsProjectile : Projectile {
    [SerializeField] private float forceRandomnessModifier;
    [SerializeField] private float rightAngleMotionModifier;
    [SerializeField] private float rotSpeed;
    // Start is called before the first frame update
    override protected IEnumerator travel(float direction) {
        float forceToApply = 0;
        speed *= 1000;
        forceToApply = Random.Range(speed / forceRandomnessModifier, speed * forceRandomnessModifier);
        if (direction == 0)
            rb.AddForce(new Vector2(forceToApply, forceToApply * rightAngleMotionModifier));
        if (direction == 180)
            rb.AddForce(new Vector2(-forceToApply, forceToApply * rightAngleMotionModifier));
        if (direction == 270)
            rb.AddForce(new Vector2(forceToApply * rightAngleMotionModifier * Random.Range(-1.0f, 1f), -forceToApply));
        if (direction == 90)
            rb.AddForce(new Vector2(forceToApply * rightAngleMotionModifier * Random.Range(-1.0f, 1f), forceToApply));
        while (!hasHitObject) {
            transform.Rotate(new Vector3(0, 0, 1), rotSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);

    }
    override protected IEnumerator travel() {
        yield return null;
        //not implemented
    }
}
