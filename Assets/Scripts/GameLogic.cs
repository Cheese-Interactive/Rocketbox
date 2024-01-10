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
        }
    }

    public void startRound(int round) {
        Round currentRound = rounds[round];
        //for(int i = 0; i< round. i++) {}
    }
}
