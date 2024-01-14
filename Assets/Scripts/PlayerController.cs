using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Health: Hits to die")]
    [SerializeField] private int health;
    [SerializeField] private float onHitImmunityTime;
    [SerializeField] private float postHitGracePeriod;
    [SerializeField] private float onHitMinLaunchAngle;
    [SerializeField] private float onHitMaxLaunchAngle;
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
    private Rigidbody2D rb;
    //i know [vvvvvv] is a weird implementation but idc
    private float hMultiplier;
    private float vMultiplier;
    private float forceAppMax;

    [Header("Projectiles")]
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private GameObject enemyToSpawn;
    private int currentWeapon = 0;

    [Header("Other")]
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private GameObject healthIndicatorSprite;
    [SerializeField] private GameObject simpleText; //temporary
    private SpriteRenderer healthShower;




    private bool canAct = true;
    private bool canShoot = true;


    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        maxHealth = health;
        forceAppMax = forceApp;
        healthShower = healthIndicatorSprite.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update() {
        playerSprite.transform.position = transform.position;
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
        StartCoroutine(canActCheck());
        switchWeapon();

        //get the player going (experimental)
        //todo: add something like this to make switching directions snappier
        /*  
         if (rb.totalForce.x <= forceApp && rb.totalForce.y <= forceApp)
              forceApp *= 10f;
          else
              forceApp = forceAppMax; 
           */

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
        Weapon current = weapons[currentWeapon].GetComponent<Weapon>();
        if (canShoot) {
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                StartCoroutine(shootCooldown(current.shoot(loc, 180)));
            }
            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                StartCoroutine(shootCooldown(current.shoot(loc, 0)));
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)) {
                StartCoroutine(shootCooldown(current.shoot(loc, 90)));
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) {
                StartCoroutine(shootCooldown(current.shoot(loc, 270)));
            }
        }

    }

    private void switchWeapon() {
        if (Input.GetKeyDown(KeyCode.R)) {
            if (currentWeapon == weapons.Length - 1) {
                currentWeapon = 0;
            }
            else
                currentWeapon++;
            StartCoroutine(shootCooldown(weapons[currentWeapon].GetComponent<Weapon>().getCooldown()));
        }
        simpleText.GetComponent<TextMeshProUGUI>().text = "Using: " + weapons[currentWeapon] + " (" + currentWeapon + ")";

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

    private IEnumerator playerImmuneFor(float time) {
        isImmune = true;                          //When player is immune, it cannot take damage              
        rb.excludeLayers = (1 << 8) | (1 << 7);   //I added ExcludeLayer later so maybe having both is redundant
        yield return new WaitForSeconds(time);
        rb.excludeLayers = 0;
        yield return new WaitForSeconds(postHitGracePeriod); //grace period. fixes some issues
        isImmune = false;
    }

    private IEnumerator canActCheck() {
        //cancel all forces
        //manually enact gravity
        float m = rb.mass;
        float a = -Physics.gravity.magnitude;
        if (!canAct)
            rb.totalForce = new Vector2(0, 0);
        while (!canAct) {
            rb.totalForce = new Vector3(0, m * a, 0);
            yield return null;
        }
    }

    private IEnumerator playHitAnimation(float duration) {
        canAct = false;
        updateHealthIndicator();
        Rigidbody2D spriteRb = playerSprite.GetComponent<Rigidbody2D>();
        float t = 0;
        float rotationTarget = ((int)duration + Random.Range(0, 2)) * 360; //cast to int so it only rotates in full circles, 1 per second
        while (t < duration) {
            t += Time.deltaTime;
            spriteRb.MoveRotation(Mathf.Lerp(0, rotationTarget, t / duration));
            healthIndicatorSprite.transform.rotation = playerSprite.transform.rotation;
            yield return null;
        }
        spriteRb.SetRotation(0); //should be unnecesary 
        canAct = true;
        checkHealth();

    }


    public void setPlayerHealth(int health) {
        health = this.health;
    }

    public void hitPlayer() {
        if (!isImmune) {
            Vector2 launchVector = new Vector2(
                Mathf.Cos(Random.Range(onHitMinLaunchAngle, onHitMaxLaunchAngle)), 0);
            //Vector2 launchVector = new Vector2(0, launchAngle);
            rb.velocity = new Vector2(0, 0);
            rb.AddForce(launchVector * forceApp * Random.Range(0.5f, 2.5f), ForceMode2D.Impulse);   //a lot of this could be combined into one big function but idk
            rb.AddForce(Vector2.up * forceApp * 2f, ForceMode2D.Impulse);
            StartCoroutine(playHitAnimation(onHitImmunityTime));
            StartCoroutine(playerImmuneFor(onHitImmunityTime));
            health--;
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
        updateHealthIndicator();
        if (health == 0) {
            print("Player has died!");
            //EditorApplication.isPlaying = false; 
            canAct = false;
        }
    }

    private void updateHealthIndicator() {
        if (health == maxHealth)
            healthShower.color = Color.green;
        else if (health < maxHealth && health > 1) //first hit, light yellow. hits after that, light red. killing blow, light turns off. (for action freeze)
            healthShower.color = Color.yellow;
        else
            healthShower.color = Color.red;
    }
}
