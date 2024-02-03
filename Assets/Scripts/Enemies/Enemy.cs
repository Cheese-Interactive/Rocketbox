using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    #region Variables + abstracted methods
    [Header("Basic Stats")]
    [SerializeField] private int health;
    [SerializeField] protected float speed;
    [Header("Type Specific Stats")]
    [SerializeField] private GameObject prefab;
    protected Rigidbody2D rb;
    protected PlayerController player;
    private bool isDecoy = false;
    protected bool hasAttackedOnce = false;

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
        if (isDecoy)
            gameObject.SetActive(false);
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
        if (health <= 0) {
            GameObject.FindAnyObjectByType<GameManager>().enemyKilled();
            Destroy(gameObject);
        }
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

    public GameObject getPrefab() {
        return prefab;
    }

    public void selfDestruct() {
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        Destroy(gameObject.GetComponent<Collider2D>());
        Destroy(gameObject.GetComponent<SpriteRenderer>());
    }

    #endregion

}
