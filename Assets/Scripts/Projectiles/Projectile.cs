using System.Collections;
using UnityEngine;

public abstract class Projectile : MonoBehaviour {

    #region Variables + abstracted methods
    [Header("Stats")]
    [SerializeField] protected float speed;
    [SerializeField] protected int damage;
    [SerializeField] protected float cooldown; //time between shots
    [SerializeField] protected float hitForce;
    [SerializeField] protected float gravityModifier;
    [SerializeField] protected bool shouldHitPlayer;
    protected Enemy targetHit;
    protected bool hasHitObject = false;
    protected Rigidbody2D rb;


    protected abstract IEnumerator travel(float direction);
    #endregion

    #region Initializers
    //to create a projectile:
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
    //can initialize with angle in degrees, vector2, or vector3
    //i dont feel like learning what a quaternion is 
    public float initialize(float direction) {
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
        return cooldown;
    }
    public float initialize(Vector2 direction) {
        return initialize(getAngleFromVector2(direction));
    }
    public float initialize(Vector3 direction) {
        return initialize(getAngleFromVector3(direction));
    }
    public float initialize(Quaternion direction) {
        return initialize(getAngleFromVector3(direction.eulerAngles));
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

    #region Angle Format Conversion Math
    //NOTE: everything below is ai generated
    //i have no idea what any of this does nor would i have been able to make any of this myself
    //better(?) (and 9999x harder) solution: learn quaternions
    protected Vector2 getDirectionVector(float direction) {
        float Radians = Mathf.Deg2Rad * direction;
        return new Vector2(Mathf.Cos(Radians) * speed, Mathf.Sin(Radians) * speed);
    }

    protected float getAngleFromVector3(Vector3 vector) {
        Vector3 reference = new Vector3(0, 1, 0);
        float angle = Vector3.Angle(reference, vector);
        Vector3 cross = Vector3.Cross(reference, vector);
        if (cross.z > 0) {
            angle = 360 - angle;
        }
        return angle;
    }

    protected float getAngleFromVector2(Vector2 vector) {
        Vector2 reference = new Vector2(0, 1);
        float angle = Vector2.Angle(reference, vector);
        Vector3 cross = Vector3.Cross(reference, vector);
        if (cross.z > 0) {
            angle = 360 - angle;
        }
        return angle;
    }

    protected float getAngleFromQuaternion(Quaternion quaternion) {
        Vector3 forward = Vector3.forward;
        Vector3 up = Vector3.up;
        Vector3 rotatedForward = quaternion * forward;
        Vector3 rotatedUp = quaternion * up;
        float angle = Mathf.Atan2(rotatedUp.y, rotatedUp.x) * Mathf.Rad2Deg;
        return angle;
    }

    #endregion

}
