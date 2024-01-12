using System.Collections;
using UnityEngine;

public abstract class Boss : MonoBehaviour {

    //i love polymorphism
    [SerializeField] protected float speed;
    [SerializeField] protected int health;
    protected GameObject player;
    protected Rigidbody2D rb;
    [SerializeField] private float onDieMinLaunchAngle, onDieMaxLaunchAngle;
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
            StartCoroutine(die());
        }
    }

    protected IEnumerator die() {//this doesnt work, unsure why
        print("dead");
        Vector2 launchVector = new Vector2(
               Mathf.Cos(Random.Range(onDieMinLaunchAngle, onDieMaxLaunchAngle)), 0);
        rb.velocity = new Vector2(0, 0);
        rb.AddForce(launchVector * 5 * Random.Range(0.5f, 2.5f), ForceMode2D.Impulse);   //a lot of this could be combined into one big function but idk
        rb.AddForce(Vector2.up * 5 * 2f, ForceMode2D.Impulse);
        //rb.isKinematic = true;
        yield return new WaitForSeconds(6);
        Destroy(gameObject);
    }

}
