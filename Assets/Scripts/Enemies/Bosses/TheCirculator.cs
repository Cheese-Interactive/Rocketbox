using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCirculator : Boss {
    [Header("Stats")]
    [SerializeField] private int rotationSpeed;
    [Header("GameObjects")]
    [SerializeField] private GameObject turret;
    [SerializeField] private List<GameObject> weakpoints = new List<GameObject>();
    [SerializeField] private Projectile projectile;
    [SerializeField] private GameObject projectileOrigin;
    protected List<Collider2D> weakpointColliders = new List<Collider2D>();
    private bool shouldRotate;
    private bool shouldPointTurret;
    private float curActionTime;
    private bool hasPlayedSpawnTaunt;
    private float vMovementBound = 19.5f;
    private Vector3 direction;
    private Vector3 direction2D;


    // Start is called before the first frame update
    override protected void Initialize() {
        foreach (GameObject weakpoint in weakpoints) {
            weakpointColliders.Add(((GameObject)weakpoint).GetComponent<Collider2D>());
            print(weakpoint);
        }
        hasPlayedSpawnTaunt = false;
    }

    // Update is called once per frame
    void Update() {
        healthCheck();
        if (shouldRotate)
            sprite.transform.Rotate(new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);
        else if (!hasPlayedSpawnTaunt) {
            StartCoroutine(spawnTaunt());
            hasPlayedSpawnTaunt = true;
        }
        if (shouldPointTurret) {
            direction = player.transform.position - transform.position;
            direction2D = Vector3.ProjectOnPlane(-direction, Vector3.forward);
            turret.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction2D);
        }


        if (Input.GetKeyDown(KeyCode.Semicolon)) {
            resetWeakpoints();
            enableWeakpoints(Random.Range(3, 5));
        }

        if (curActionTime > 0f)
            curActionTime = curActionTime - (Time.deltaTime);

    }

    private IEnumerator spawnTaunt() {                       //from PlayerController hitAnimation
        curActionTime = 4;
        while (curActionTime > 0) {
            sprite.transform.Rotate(new Vector3(0, 0, 360 * Time.deltaTime));
            turret.transform.Rotate(new Vector3(0, 0, -720 * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
        shouldRotate = true;
        StartCoroutine(moveToAttackingPhase());
    }



    //i decided to hard code a bunch of stuff in here
    private IEnumerator moveToAttackingPhase() {
        //pick a side
        Vector3 pos = transform.position;
        if (Random.Range(1, 3) == 1)  //this is weird but 1 = left, 2 = right (the 3 is exclusive)
            pos = new Vector3(-53, 0);
        else
            pos = new Vector3(53, 0);

        //move to pos

        shouldPointTurret = false;
        float t = 0;
        float duration = 1.2f;
        Vector3 startPosition = transform.position;

        while (t < duration) {
            transform.position = Vector3.Lerp(startPosition, pos, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = pos;

        StartCoroutine(attackingPhase());

    }

    private IEnumerator attackingPhase() {
        //determine where turret should point
        Quaternion turretDir = Quaternion.identity;
        if (transform.position.x > 0)
            turretDir = Quaternion.Euler(0, 0, 180);
        else
            turretDir = Quaternion.Euler(0, 0, -180);


        //stick to player Y


        Vector3 target = transform.position;
        Vector3 smoothPosition = transform.position;
        float followSpeed = 2f;
        float delay = 0.5f;
        shouldRotate = true;

        curActionTime = 15f;
        while (curActionTime > 0) {
            StartCoroutine(lockTurret(turretDir));
            StartCoroutine(shoot(turretDir));
            if (player.transform.position.y < vMovementBound && player.transform.position.y > -vMovementBound)
                target = new Vector3(transform.position.x, player.transform.position.y, 0);
            else if (player.transform.position.y > vMovementBound)
                target = new Vector3(transform.position.x, vMovementBound, 0);
            else if (player.transform.position.y < -vMovementBound)
                target = new Vector3(transform.position.x, -vMovementBound, 0);
            smoothPosition = Vector3.Lerp(smoothPosition, target, followSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, smoothPosition, delay);
            yield return null;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player"))
            player.GetComponent<PlayerController>().hitPlayer();

    }

    private IEnumerator lockTurret(Quaternion direction) {
        turret.transform.rotation = direction;
        turret.transform.Rotate(new Vector3(0, 0, 1), -rotationSpeed * Time.deltaTime);   //todo: smoothly rotate the turret instead of snapping it into place here
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator shoot(Quaternion direction) {
        yield return new WaitForSeconds(1);
        // Instantiate(projectile, transform.position, direction).initialize();  //todo: shoot multiple projectiles out
        //todo: add bosses into collision logic
        //todo: update projectile script more (again)
    }

    private void resetWeakpoints() {
        foreach (GameObject weakpoint in weakpoints)
            weakpoint.GetComponent<BossWeakpoint>().isVulnerable(false);
    }

    private void enableWeakpoints(int count) {
        if (count > weakpoints.Count)
            count = weakpoints.Count;
        for (int i = 0; i < count; i++)
            weakpoints[Random.Range(0, weakpoints.Count)].GetComponent<BossWeakpoint>().isVulnerable(true);
    }

    private List<int> generateNums(int count) {
        List<int> nums = new List<int>();
        for (int i = 0; i < count; i++) {                        //TODO: Implement this (generates 3 UNIQUE random nums)
            nums.Add(Random.Range(0, weakpoints.Count));         //is it even worth it?
        }
        return null;
    }
}
