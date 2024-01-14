using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] RoundCreator[] roundCreators;
    [SerializeField] int startAtRound;
    [SerializeField] float timeBetweenRounds;
    [SerializeField] private List<GameObject> bosses = new List<GameObject>();
    [SerializeField] private GameObject spawnIndicator;
    [SerializeField] private GameObject bossSpawnIndicator;
    private List<Round> rounds = new List<Round>();
    private bool started = false;
    private int round;
    private int enemiesLeft;

    void Start() {
        round = startAtRound;

        foreach (RoundCreator roundcreator in roundCreators) {
            rounds.Add(roundcreator.getData());
            print("Round Initialized! (" + rounds.Count + ")");
            //Destroy(roundcreator);
        }
        //rounds = rounds.OrderBy(r => r.getIndex()).ToList();
        /*for (int i = 0; i < rounds.Count; i++) {
            List<Enemy> current = rounds[i].GetEnemies();
            print((current[i].getPrefab()));
        }*/
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            print("Game has started!");
            started = true;
        }
        if (started && enemiesLeft == 0) {
            StartCoroutine(roundCooldown(timeBetweenRounds));
            enemiesLeft = -1; //fixes an issue
        }

    }

    public void startRound(int r) {
        enemiesLeft = 0;
        Round currentRound = null;
        if (round % 5 != 0) {
            for (int i = 0; i < rounds.Count; i++)
                if (rounds[i].getIndex() == r)
                    currentRound = rounds[i];
            if (currentRound == null) {
                print("ERROR: Round not initialized (check RoundCreator indexes)");
                return;
            }


            List<Enemy> currentEnemies = currentRound.getEnemies();
            List<Vector3> currentLocations = currentRound.getLocations();
            for (int i = 0; i < currentEnemies.Count; i++) {
                print(currentEnemies.Count + " enemies will be spawned! Using round index: " + currentRound.getIndex());
            }
            for (int i = 0; i < currentEnemies.Count; i++) {
                Instantiate(spawnIndicator, currentLocations[i], Quaternion.identity);
            }
            StartCoroutine(waitThenSpawnEnemies(timeBetweenRounds, currentEnemies, currentLocations));

            enemiesLeft = currentEnemies.Count;
        }
        else {
            Instantiate(bossSpawnIndicator, Vector3.zero, Quaternion.identity);
            StartCoroutine(waitThenSpawnBoss(timeBetweenRounds * 1.6f, bosses[Random.Range(0, bosses.Count)]));

            enemiesLeft = 1;
        }
        round++;
    }

    private IEnumerator waitThenSpawnEnemies(float seconds, List<Enemy> currentEnemies, List<Vector3> currentLocations) {
        yield return new WaitForSeconds(seconds);
        for (int i = 0; i < currentEnemies.Count; i++) {
            Instantiate(currentEnemies[i], currentLocations[i], Quaternion.identity).gameObject.SetActive(true);
            print("Spawned " + currentEnemies[i] + " at " + currentLocations[i] + " (" + (currentEnemies.Count - i - 1) + " thing(s) to go)");
        }
    }
    private IEnumerator waitThenSpawnBoss(float seconds, GameObject boss) {
        yield return new WaitForSeconds(seconds);
        Instantiate(boss, Vector3.zero, Quaternion.identity);
    }

    public void enemyKilled() {
        if (started)
            enemiesLeft--;
    }

    private IEnumerator roundCooldown(float seconds) {
        yield return new WaitForSeconds(seconds);
        startRound(round);
    }


}
