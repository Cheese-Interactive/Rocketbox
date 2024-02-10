using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheCirculator : Boss {

    #region Variables
    [Header("Stats")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float strongWeaponCooldown;
    [SerializeField] private float weakWeaponCooldown;
    [SerializeField] private float vulnerableTime;
    [SerializeField] private float attackingTime;
    [Header("GameObjects")]
    [SerializeField] private GameObject turret;
    [SerializeField] private List<GameObject> weakpoints = new List<GameObject>();
    [SerializeField] private GameObject strongWeapon;
    [SerializeField] private GameObject weakWeapon;
    [SerializeField] private GameObject projectileOrigin;
    protected List<Collider2D> weakpointColliders = new List<Collider2D>();
    private bool shouldRotate;
    private bool shouldPointTurret;
    private float curActionTime;
    private bool hasPlayedSpawnTaunt;
    private float vMovementBound = 21f;
    private Vector3 direction;
    private Vector3 direction2D;
    private bool canShoot;
    private int weakpointsActive = 0;

    #endregion

    #region behavior/phases

    // Start is called before the first frame update
    override protected void Initialize() {
        foreach (GameObject weakpoint in weakpoints) {
            weakpointColliders.Add(((GameObject)weakpoint).GetComponent<Collider2D>());
            print(weakpoint);
        }
        hasPlayedSpawnTaunt = false;
        resetWeakpoints();
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


        if (health == 0) {
            StopAllCoroutines();
            shouldRotate = false;
            shouldPointTurret = false;
        }


        if (curActionTime > 0f)
            curActionTime -= Time.deltaTime;

    }

    private IEnumerator spawnTaunt() {
        resetWeakpoints();
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
        float shootAngle;
        if (transform.position.x > 0) {
            turretDir = Quaternion.Euler(0, 0, 0);
            shootAngle = 180;
        }
        else {
            turretDir = Quaternion.Euler(0, 0, 180);
            shootAngle = 0;
        }



        //stick to player Y


        Vector3 target = transform.position;
        Vector3 smoothPosition = transform.position;
        float followSpeed = 2f;
        float delay = 0.5f;
        shouldRotate = true;
        canShoot = true;

        curActionTime = attackingTime;
        while (curActionTime > 0) {
            StartCoroutine(lockTurret(turretDir));
            if (canShoot)
                StartCoroutine(shootStrongBlaster(shootAngle));
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
        StartCoroutine(moveToVulnerablePhase());

    }

    private IEnumerator moveToVulnerablePhase() {
        //pick a side
        shouldPointTurret = true;
        float t = 0;
        float duration = 0.8f;
        Vector3 startPosition = transform.position;

        while (t < duration) {
            transform.position = Vector3.Lerp(startPosition, Vector3.zero, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = Vector3.zero;

        StartCoroutine(vulnerablePhase());

    }

    private IEnumerator vulnerablePhase() {
        curActionTime = vulnerableTime;
        resetWeakpoints();
        enableWeakpoints(Random.Range(2, 5));
        while (curActionTime > 0) {
            if (canShoot)
                StartCoroutine(shootWeakBlaster());
            if (weakpointsActive == 0) {
                weakpointsActive = -1;
                curActionTime = 0.2f;
            }
            yield return null;
        }

        resetWeakpoints();
        StartCoroutine(moveToAttackingPhase());
    }


    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player"))
            player.GetComponent<PlayerController>().hitPlayer();

    }

    #endregion

    #region Actions

    private IEnumerator lockTurret(Quaternion direction) {
        turret.transform.rotation = direction;
        turret.transform.Rotate(new Vector3(0, 0, 1), -rotationSpeed * Time.deltaTime);   //todo: smoothly rotate the turret instead of snapping it into place here
        yield return new WaitForEndOfFrame();
    }

    private IEnumerator shootStrongBlaster(float angle) {
        canShoot = false;
        yield return new WaitForSeconds(strongWeaponCooldown);
        strongWeapon.GetComponent<Weapon>().shoot(projectileOrigin.transform.position, angle);
        canShoot = true;
    }

    private IEnumerator shootWeakBlaster() {
        canShoot = false;
        Quaternion rot = turret.transform.rotation.normalized;
        yield return new WaitForSeconds(weakWeaponCooldown);
        weakWeapon.GetComponent<Weapon>().shoot(projectileOrigin.transform.position, rot);
        canShoot = true;
    }


    private void resetWeakpoints() {
        weakpointsActive = 0;
        foreach (GameObject weakpoint in weakpoints)
            weakpoint.GetComponent<BossWeakpoint>().isVulnerable(false);
    }

    private void enableWeakpoints(int count) {
        if (count > weakpoints.Count)
            count = weakpoints.Count;
        for (int i = 0; i < count; i++) {
            weakpoints[Random.Range(0, weakpoints.Count)].GetComponent<BossWeakpoint>().isVulnerable(true);
            weakpointsActive++;
        }
    }

    new public void getHit() {
        health--;
        weakpointsActive--;
        print(health);
    }

    #endregion

}
