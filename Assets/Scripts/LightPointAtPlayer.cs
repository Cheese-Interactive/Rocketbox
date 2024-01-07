using UnityEngine;

public class LightPointAtPlayer : MonoBehaviour {
    //necessariness of this script: 3/10
    private GameObject player;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update() {
        //bing ai made this code. i dont honestly know what its doing
        Vector3 direction = player.transform.position - transform.position;
        Vector3 direction2D = Vector3.ProjectOnPlane(direction, Vector3.forward);
        transform.rotation = Quaternion.FromToRotation(Vector3.right, direction2D);
        //i added this part myself cuz it thinks the player is further to the right than it actually is for some reason 
        //apparently it is exactly 90 degrees off
        transform.Rotate(0, 0, -90f);
    }
}
