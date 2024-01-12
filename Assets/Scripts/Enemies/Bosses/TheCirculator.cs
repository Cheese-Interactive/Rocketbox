using System.Collections.Generic;
using UnityEngine;

public class TheCirculator : Boss {
    [SerializeField] private int rotationSpeed;
    private GameObject turret;
    private Vector3 direction;
    private Vector3 direction2D;
    protected List<GameObject> weakpoints = new List<GameObject>();
    protected List<Collider2D> weakpointColliders = new List<Collider2D>();

    // Start is called before the first frame update
    override protected void Initialize() {
        for (int i = 0; i < transform.GetChild(0).childCount; i++) {
            GameObject currentObject = transform.GetChild(0).GetChild(i).gameObject;
            weakpoints.Add(currentObject);
            weakpointColliders.Add(currentObject.GetComponent<Collider2D>());
            print(weakpoints[i]);
        }
        resetWeakpoints();
    }

    // Update is called once per frame
    void Update() {
        healthCheck();
        transform.Rotate(new Vector3(0, 0, 1), rotationSpeed * Time.deltaTime);

        turret = gameObject.transform.GetChild(1).GetChild(0).gameObject;
        direction = player.transform.position - transform.position;
        direction2D = Vector3.ProjectOnPlane(-direction, Vector3.forward);
        turret.transform.rotation = Quaternion.FromToRotation(Vector3.right, direction2D);


        if (Input.GetKeyDown(KeyCode.Semicolon)) {
            resetWeakpoints();
            enableWeakpoints(Random.Range(3, 5));
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            StartCoroutine(die());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player"))
            player.GetComponent<PlayerController>().hitPlayer();

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
