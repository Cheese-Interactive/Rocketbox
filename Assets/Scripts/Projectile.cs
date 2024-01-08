using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour {

    [Header("Stats")]
    [SerializeField] private float speed;
    [SerializeField] private int damage;
    [SerializeField] private float cooldown; //time between shots
    [SerializeField] private float hitForce;
    private Enemy targetHit;
    private bool hasHitObject = false;
    private Rigidbody2D rb;

    public float initialize(float direction) {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        StartCoroutine(travel(direction));
        return cooldown;
        rb.mass = hitForce;
    }

    protected void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Projectile")) {
            if (collision.gameObject.CompareTag("Enemy"))
                collision.gameObject.GetComponent<Enemy>().takeDamage(damage);
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

    public int getDamage() {
        return damage;
    }

    private void OnDestroy() {

    }
}
