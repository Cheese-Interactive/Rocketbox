using System.Collections.Generic;
using UnityEngine;

public class Round {
    private int index;                                        //round #
    private List<Enemy> enemies = new List<Enemy>();//stuff to spawn
    private List<Vector3> locations = new List<Vector3>();    //locations of stuff, order corresponds with enemies list


    public Round(int index, List<Enemy> objects, List<Vector3> locations) {
        this.index = index;
        for (int i = 0; i < objects.Count; i++) {
            this.enemies.Add(objects[i]);
            this.locations.Add(locations[i]);
        }
    }

    public List<Enemy> getEnemies() {
        return enemies;
    }
    public List<Vector3> getLocations() {
        return locations;
    }

    public int getIndex() {
        return index;
    }





}
