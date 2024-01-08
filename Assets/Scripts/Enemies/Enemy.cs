using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    [SerializeField] private int health;
    [SerializeField] protected float speed;
    protected Rigidbody2D rb;
    protected PlayerController player;

    protected abstract void attack();
    protected abstract void seekPlayer();
    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        rb.gravityScale = 0f;
    }

    // Update is called once per frame
    void Update() {
        healthCheck();
        attack();
        seekPlayer();
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

    private void OnDestroy() {

    }

}
