using System.Collections;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    #region Variables + abstracted methods
    [SerializeField] private int health;
    [SerializeField] protected float speed;
    protected Rigidbody2D rb;
    protected PlayerController player;
    private bool isDecoy = false;

    protected abstract void attack();
    protected abstract void seekPlayer();
    #endregion

    #region Start/Update

    void Start() {
        //StartCoroutine(checkFix());

        if (transform.parent != null)                                                           //does the enemy have a parent?
            isDecoy = gameObject.transform.parent.gameObject.GetComponent<RoundCreator>();      //if so, is it a RoundCreator? (more details in RoundCreator.cs)
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        rb.gravityScale = 0f;
        if (isDecoy) {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            Destroy(gameObject.GetComponent<Collider2D>());
            Destroy(gameObject.GetComponent<SpriteRenderer>());
            gameObject.SetActive(false);
        }
    }

    private IEnumerator checkFix() {
        yield return new WaitForEndOfFrame();
        if (isDecoy) {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            Destroy(gameObject.GetComponent<Collider2D>());
            //Destroy(gameObject.GetComponent<SpriteRenderer>());
            //gameObject.SetActive(false);
        }
        else {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.Find("Player").GetComponent<PlayerController>();
            rb.gravityScale = 0f;
        }
    }

    void Update() {
        if (!isDecoy) {
            seekPlayer();
            attack();
            healthCheck();
        }
    }
    #endregion

    #region Behavior

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
        // to be implemented
    }

    #endregion

}
