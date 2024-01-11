using System.Collections.Generic;
using UnityEngine;

public class RoundCreator : MonoBehaviour {

    /* What this is:
     * An interactive and simple way to create a predefined "round" for the game
     * An empty gameobject with this script on it is created (goes under RoundDefinitions gameobject for organization purposes)
     * Every RoundCreator object has a set of enemies under it
     * Those enemies are then used to create a RoundDefinition
     * RoundDefinition stores a list of enemies and their corresponding spawn locations as well as an index (round number)
     * Round number is also given in the RoundCreator object
     * GameManager (GameLogic) uses RoundDefinitions to start and end rounds (to be implemented)
     */
    [SerializeField] private int index;
    private List<GameObject> enemies = new List<GameObject>();
    private List<Vector3> locations = new List<Vector3>();
    private Round round;

    // Start is called before the first frame update
    void Start() {
        for (int i = 0; i < transform.childCount; i++) {
            GameObject current = transform.GetChild(i).gameObject.GetComponent<Enemy>().getPrefab();
            enemies.Add(current);
            locations.Add(current.transform.position);
        }
        round = new Round(index, enemies, locations);
        //Destroy(gameObject);

    }

    public Round getData() {
        return round;
    }
    // Update is called once per frame
    void Update() {

    }
}

