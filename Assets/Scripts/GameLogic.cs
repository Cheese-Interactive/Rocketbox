using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    // Start is called before the first frame update
    [SerializeField] RoundCreator[] roundCreators;
    private List<Round> rounds = new List<Round>();
    private bool started = false;
    private int round = 0;
    void Start() {
        foreach (RoundCreator roundcreator in roundCreators) {
            rounds.Add(roundcreator.getData());
            Destroy(roundcreator);
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            print("Game has started!");
            started = true;
        }
        if (started == true) {
            startRound(round);
            started = false;//REMOVE THIS i think this spawn thing needs to be an IEnumerator or smth idk
        }
    }

    public void startRound(int round) {
        Round currentRound = rounds[round];
        List<GameObject> currentEnemies = currentRound.GetObjects();
        List<Vector3> currentLocations = currentRound.GetLocations();
        for (int i = 0; i < currentEnemies.Count; i++) {
            //todo: spawn enemies BASED ON TYPE
            print("Enemy " + i + " is a " + currentEnemies[i] + " which will be spawned at " + currentLocations[i]);
        }
        started = false; //REMOVE THIS i think this spawn thing needs to be an IEnumerator or smth idk
    }
}
