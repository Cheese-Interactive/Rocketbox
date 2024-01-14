using System.Collections;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {

    #region Variables + abstracted methods
    [Header("Stats")]
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [SerializeField] protected float hitForce;
    [SerializeField] protected float gravityModifier;
    [SerializeField] protected bool shouldHitPlayer;
    protected Enemy targetHit;
    protected bool hasHitObject = false;
    protected Rigidbody2D rb;


    protected abstract IEnumerator travel(float direction);
    protected abstract IEnumerator travel();
    #endregion

    #region Initializers
    //to create a projectile:
    //IGNORE ALL THIS, ITS ALL CAP AND HAS BEEN CHANGED A LOT


    /* must have a shootCooldown coroutine in the class of the thing trying to instantiate a projectile
     * when instantiating, start the coroutine, passing in Instantiate(...).Initialize(...) as the cooldown
     * this both instantiates the projectile and returns its cooldown (as well as whatever other stuff it needs to do)
     * i moved everything normally in Start() to there but idk if it makes a difference
     * 
     * this is made this way because i originally made the projectile script forgetting that i had the intention of making enemies with projectiles\
     * as a result, cooldowns are stored in the projectile prefab itself
     * this could be a good thing? either way its kinda weird
     * 
     */
    //can initialize with angle in degrees 
    //or, pass quaternion into instantiate, and pass no angle into initialize
    //i do NOT like the way this is coded but i dont want to go back and redo it rn
    //i dont feel like learning what a quaternion is 
    public void initialize(float direction) {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityModifier;
        StartCoroutine(travel(direction));
        rb.mass = hitForce;
        if (!shouldHitPlayer)
            rb.excludeLayers = (1 << 3) | (1 << 7);
        else {
            damage = 0;
            rb.excludeLayers = (1 << 8) | (1 << 7);
        }
    }

    public void initialize() {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityModifier;
        StartCoroutine(travel());
        rb.mass = hitForce;
        if (!shouldHitPlayer)
            rb.excludeLayers = (1 << 3) | (1 << 7);
        else {
            damage = 0;
            rb.excludeLayers = (1 << 8) | (1 << 7);
        }
    }
    #endregion

    #region Behavior
    //Projectile starts moving once initialize is called (coroutine travel())
    protected void OnCollisionEnter2D(Collision2D collision) {
        if (!shouldHitPlayer)
            if (collision.gameObject.CompareTag("Enemy"))
                collision.gameObject.GetComponent<Enemy>().takeDamage(damage);
        if (shouldHitPlayer)
            if (collision.gameObject.CompareTag("Player"))
                collision.gameObject.GetComponent<PlayerController>().hitPlayer();
        hasHitObject = true;
    }

    #endregion

    #region Angle Stuff

    //trigonometry baby
    protected Vector2 getDirectionVector(float direction) {
        float Radians = Mathf.Deg2Rad * direction;
        return new Vector2(Mathf.Cos(Radians) * speed, Mathf.Sin(Radians) * speed);
    }

    //ai generated (vvvv)
    protected Vector3 getDirectionVector(GameObject other) {
        Vector3 direction = other.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Vector3 directionVector = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * speed, Mathf.Sin(angle * Mathf.Deg2Rad) * speed, 0);
        return directionVector;
    }


    #endregion

}
