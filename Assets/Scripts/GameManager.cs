using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] RoundCreator[] roundCreators;
    [SerializeField] int startAtRound;
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
        print(enemiesLeft);
        if (Input.GetKeyDown(KeyCode.P)) {
            print("Game has started!");
            started = true;
        }
        if (started && enemiesLeft == 0)
            startRound(round);

    }

    public void startRound(int r) {
        enemiesLeft = 0;
        Round currentRound = null;
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
            print(currentEnemies.Count + " enemies will be spawned! Using index: " + currentRound.getIndex());
        }

        for (int i = 0; i < currentEnemies.Count; i++) {
            Instantiate(currentEnemies[i], currentLocations[i], Quaternion.identity).gameObject.SetActive(true);
            print("Spawned " + currentEnemies[i] + " at " + currentLocations[i] + " (" + (currentEnemies.Count - i - 1) + " thing(s) to go)");
        }
        enemiesLeft = currentEnemies.Count;
        round++;
    }

    public void enemyKilled() {
        if (started)
            enemiesLeft--;
    }
}
