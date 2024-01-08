using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private int health;
    private Rigidbody2D rb;
    private PlayerController player;
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        rb.gravityScale = 0f;
    }

    // Update is called once per frame
    void Update() {
        healthCheck();
    }

    protected void healthCheck() {
        if (health <= 0)
            Destroy(gameObject);
    }
    public void takeDamage(int damage) {
        health -= damage;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player"))
            player.hitPlayer();
    }

    protected void attack() {
        //The default enemy has no ai and therefore, no attack method
        //Only damages with collision
    }

    private void OnDestroy() {

    }
}
