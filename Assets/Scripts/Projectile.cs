using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [Header("Stats")]
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float cooldown; //time between shots
    private bool hasHitObject = false;
    private Rigidbody2D rb;

    public float initialize(float direction) {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        StartCoroutine(travel(direction));
        return cooldown;
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player") &&
        collision.gameObject.layer != LayerMask.NameToLayer("Projectile")) {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                collision.otherCollider.GetComponentInParent<Enemy>(true).enemyHit(damage); //not working, enemy does not take damage
            hasHitObject = true;
        }
    }


    protected IEnumerator travel(float direction) {
        transform.forward = new Vector3(0, 0, direction);
        rb.SetRotation(direction);
        if (direction == 0) //right
            rb.velocity = new Vector2(speed, 0);
        if (direction == 180) //left
            rb.velocity = new Vector2(-speed, 0);
        if (direction == 90) //up
            rb.velocity = new Vector2(0, speed);
        if (direction == 270) //down
            rb.velocity = new Vector2(0, -speed);

        while (!hasHitObject)
            yield return null;
        Destroy(gameObject);
    }

    private void OnDestroy() {

    }
}
