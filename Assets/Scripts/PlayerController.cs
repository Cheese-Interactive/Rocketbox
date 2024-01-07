using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Health: Hits to die")]
    [SerializeField] private int health;
    [SerializeField] private float onHitImmunityTime;
    private bool isImmune = false;
    private int maxHealth;

    [Header("Speed and multipliers")]
    [SerializeField] private float forceApp;
    [SerializeField] private float maxForce;
    //[SerializeField] private float hoverLaunchForce;
    //[SerializeField] private float hoverGravityCounterAcceleration;
    [SerializeField] private float upMultiplier = 1;
    [SerializeField] private float downMultiplier = 1;
    [SerializeField] private float leftMultiplier = 1;
    [SerializeField] private float rightMultiplier = 1;

    [Header("Projectiles")]
    [SerializeField] private GameObject[] projectiles;
    [SerializeField] private GameObject enemyToSpawn;
    private int currentProjectile = 0;

    private Rigidbody2D rb;
    //i know [vvvvvv] is a weird implementation but idc
    private float hMultiplier;
    private float vMultiplier;

    private bool canAct = true;
    private bool canShoot = true;

    [SerializeField] private GameObject simpleText;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        maxHealth = health;
    }

    // Update is called once per frame
    void Update() {
        //get keys down
        //again, idk how to do it not sus
        //1 is positive input -1 is negative input
        if (canAct) {
            vMultiplier = Input.GetAxisRaw("Vertical");
            if (vMultiplier == 1)
                vMultiplier *= upMultiplier;
            if (vMultiplier == -1)
                vMultiplier *= downMultiplier;
            hMultiplier = Input.GetAxisRaw("Horizontal");
            if (hMultiplier == 1)
                hMultiplier *= rightMultiplier;
            if (hMultiplier == -1)
                hMultiplier *= leftMultiplier;
            shoot();
        }
        switchWeapon();

        rb.AddForce(transform.up.normalized * forceApp * vMultiplier + transform.right.normalized * forceApp * hMultiplier);

        //print(System.Math.Abs(rb.totalForce.x) + " " + System.Math.Abs(rb.totalForce.y));
        //janky way of adding a max force. it doesnt work super cleanly but it works
        if (System.Math.Abs(rb.totalForce.x) > maxForce)                           //cant go left or right too fast
            rb.AddForce(-transform.right * forceApp * hMultiplier);
        if (rb.totalForce.y > maxForce)                                            //cant go up too fast
            rb.AddForce(-transform.up * forceApp * vMultiplier);
        if (rb.totalForce.y < -maxForce && Input.GetAxisRaw("Vertical") != 0)      //falling has no limit, thrusting down has a limit
            rb.AddForce(-transform.up * forceApp * vMultiplier);                   //not sure how big of a difference this makes and also has potential issues



    }

    private void shoot() {
        //idk how to condense this
        Vector3 loc = new Vector3(transform.position.x, transform.position.y, 0);
        Quaternion rot = Quaternion.identity;
        GameObject current = projectiles[currentProjectile];
        if (canShoot) {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                StartCoroutine(shootCooldown(Instantiate(current, loc, rot).GetComponent<Projectile>().initialize(180)));
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                StartCoroutine(shootCooldown(Instantiate(current, loc, rot).GetComponent<Projectile>().initialize(0)));
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                StartCoroutine(shootCooldown(Instantiate(current, loc, rot).GetComponent<Projectile>().initialize(90)));
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                StartCoroutine(shootCooldown(Instantiate(current, loc, rot).GetComponent<Projectile>().initialize(270)));
            }
        }

    }

    private void switchWeapon() {
        if (Input.GetKeyDown(KeyCode.R)) {
            if (currentProjectile == projectiles.Length - 1) {
                currentProjectile = 0;
            }
            else
                currentProjectile++;
            //exploit with this switch weapons to skip cooldowns
            canShoot = true;
        }
        simpleText.GetComponent<TextMeshProUGUI>().text = "Using: " + projectiles[currentProjectile] + " (" + currentProjectile + ")";

        //leaving this here for now
        if (Input.GetKeyDown(KeyCode.J)) {
            Instantiate(enemyToSpawn, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    private IEnumerator shootCooldown(float time) {
        canShoot = false;
        yield return new WaitForSeconds(time);
        canShoot = true;
    }

    private IEnumerator playerImmuneFor(float time, bool disableActions) {
        canAct = !disableActions;
        isImmune = true;
        yield return new WaitForSeconds(time);
        isImmune = false;
        if (canAct == false)
            canAct = true;
    }

    public void setPlayerHealth(int health) {
        health = this.health;
    }

    public void hitPlayer() {
        if (!isImmune) {
            rb.totalForce = new Vector2(-rb.totalForce.x, -rb.totalForce.y);
            rb.velocity = new Vector2(0, 0);
            StartCoroutine(playerImmuneFor(onHitImmunityTime, true));
            rb.AddForce(transform.up * forceApp * 2f, ForceMode2D.Impulse);
            health--;
            checkHealth();
        }
        else
            print("Player is immune!");

    }

    public void healPlayer(int heal) {
        health += heal;
        if (health >= maxHealth)
            health = maxHealth;

    }

    public int getPlayerHealth() {
        return health;
    }

    private void checkHealth() {
        //to be implemented
        print(health);
    }
}
