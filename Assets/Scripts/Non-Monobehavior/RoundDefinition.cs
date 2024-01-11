using System.Collections.Generic;
using UnityEngine;

public class Round {
    private int index;                                        //round #
    private List<GameObject> objects = new List<GameObject>();//stuff to spawn
    private List<Vector3> locations = new List<Vector3>();    //locations of stuff, order corresponds with enemies list


    public Round(int index, List<GameObject> objects, List<Vector3> locations) {
        this.index = index;
        for (int i = 0; i < objects.Count; i++) {
            this.objects.Add(objects[i]);
            this.locations.Add(locations[i]);
        }
    }

    public List<GameObject> GetObjects() {
        return objects;
    }
    public List<Vector3> GetLocations() {
        return locations;
    }

    public int GetIndex() {
        return index;
    }





}
