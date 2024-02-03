using UnityEngine;

public class Rocket : BasicProjectile {
    [SerializeField] private float blastSize;
    [SerializeField] private ExplosionParticles explosionEffect;
    private void explode() {
        Instantiate(explosionEffect, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity).initialize(blastSize);
        RaycastHit2D[] collisions = Physics2D.BoxCastAll(transform.position, new Vector2(blastSize, blastSize), 0, Vector3.zero, 0);
        foreach (RaycastHit2D hit in collisions) {
            Collider2D collision = hit.collider;
            if (!shouldHitPlayer)
                if (collision.gameObject.CompareTag("Enemy"))
                    collision.gameObject.GetComponent<Enemy>().takeDamage(damage);
            if (shouldHitPlayer)
                if (collision.gameObject.CompareTag("Player"))
                    collision.gameObject.GetComponent<PlayerController>().hitPlayer();
        }

    }
    new protected void OnCollisionEnter2D(Collision2D collision) {
        explode();
        hasHitObject = true;
    }

    private void OnDrawGizmos() {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(blastSize, blastSize, 0));
    }
}
