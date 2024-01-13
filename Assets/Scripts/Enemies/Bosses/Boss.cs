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
    protected abstract void Initialize();

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        rb = GetComponent<Rigidbody2D>();
        Initialize();
    }

    public void getHit() {
        health--;
        print(health);
    }

    protected void healthCheck() {
        if (health <= 0) {
            die();
        }
    }

    protected void die() {  //this doesnt work, unsure why
        Vector2 launchVector = new Vector2(
               Mathf.Cos(Random.Range(onDieMinLaunchAngle, onDieMaxLaunchAngle)), 0);
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(launchVector * 5 * Random.Range(0.5f, 2.5f));
        rb.AddForce(Vector2.up * 20);
        StartCoroutine(fakeGravity());
        //rb.excludeLayers = (1 << 3) | (1 << 6);
        StartCoroutine(queueForDeletion());
    }

    private IEnumerator fakeGravity() {
        rb.gravityScale = 0f;
        float m = rb.mass;
        float a = -Physics.gravity.magnitude;
        rb.totalForce = new Vector2(0, 0);
        while (true) {
            rb.totalForce = new Vector3(0, m * a, 0);
            yield return null;
        }
    }
    private IEnumerator queueForDeletion() {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}
