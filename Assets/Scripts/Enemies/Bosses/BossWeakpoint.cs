
using UnityEngine;

public class BossWeakpoint : MonoBehaviour {

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private GameObject player;
    private Boss belongsTo;
    private bool vulnerable;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
        belongsTo = transform.parent.parent.gameObject.GetComponent<Boss>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void isVulnerable(bool vulnerable) {
        this.vulnerable = vulnerable;
        //vulnerable: can be hit
        //invulnerable: cannot be hit
        //todo: make the colors configurable
        if (vulnerable)
            sprite.color = Color.red;
        else
            sprite.color = Color.gray;

    }

    public bool getVulnerability() {
        return vulnerable;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player"))
            player.GetComponent<PlayerController>().hitPlayer();
        if (collision.gameObject.CompareTag("Projectile") && vulnerable) {
            vulnerable = false;
            belongsTo.getHit();
            sprite.color = Color.gray;
        }


    }


}
