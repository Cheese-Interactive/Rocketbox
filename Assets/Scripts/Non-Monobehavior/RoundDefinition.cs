using System.Collections.Generic;
using UnityEngine;

public class Round {
    private int index;                                        //round #
    private List<Enemy> enemies = new List<Enemy>();          //enemies to spawn
    private List<Vector3> locations = new List<Vector3>();    //locations of enemies, order corresponds with enemy list


    public Round(int index, List<Enemy> enemies, List<Vector3> locations) {
        this.index = index;
        for (int i = 0; i < enemies.Count; i++) {
            this.enemies.Add(enemies[i]);
            this.locations.Add(locations[i]);
        }
    }

    public List<Enemy> GetEnemies() {
        return enemies;
    }
    public List<Vector3> GetLocations() {
        return locations;
    }

    public int GetIndex() {
        return index;
    }





}
