using System.Collections;
using UnityEngine;

public abstract class Boss : MonoBehaviour {

    //i love polymorphism
    [SerializeField] protected float speed;
    [SerializeField] protected int health;
    protected GameObject player;
    protected Rigidbody2D rb;
    [SerializeField] private float onDieMinLaunchAngle, onDieMaxLaunchAngle;
    [SerializeField] protected GameObject sprite;
    private GameObject gameManager;
    protected abstract void Initialize();
    private bool hasBeenKilled;

    // Start is called before the first frame update
    void Start() {
        gameManager = GameObject.Find("GameManager");
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        Initialize();
    }

    public void getHit() {
        health--;
        print(health);
    }

    protected void healthCheck() {
        if (health <= 0 && !hasBeenKilled || Input.GetKeyDown(KeyCode.V)) {
            die();
            hasBeenKilled = true;
        }
    }

    protected void die() {  //this doesnt work, unsure why
        rb.gravityScale = 1;
        rb.mass = 1;
        rb.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
        //StartCoroutine(fakeGravity());
        GameObject.FindAnyObjectByType<GameManager>().enemyKilled();

        rb.excludeLayers = (1 << 3) | (1 << 6);
        StartCoroutine(queueForDeletion());
    }

    private IEnumerator fakeGravity() {
        rb.gravityScale = 0f;
        float m = rb.mass;
        float a = -Physics.gravity.magnitude;
        rb.totalForce = new Vector2(0, 0);
        while (rb.gravityScale == 0) {
            rb.totalForce = new Vector3(0, m * a, 0);
            yield return null;
        }
    }
    private IEnumerator queueForDeletion() {
        yield return new WaitForSeconds(5);
        rb.gravityScale = 1;
        gameManager.GetComponent<GameManager>().enemyKilled();
        Destroy(gameObject);
    }
}
