using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerSpotlight : MonoBehaviour {
    //necessariness of this script: 5/10
    private GameObject player;
    private Light2D spotlight;
    [SerializeField] private float colorChangeSpeed;


    // Start is called before the first frame update
    void Start() {
        player = GameObject.Find("Player");
        spotlight = gameObject.GetComponent<Light2D>();
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

    private IEnumerator lerpColor(Color start, Color end) {
        float t = 0;
        spotlight.color = start;
        while (t < colorChangeSpeed) {
            spotlight.color = Color.Lerp(start, end, t / colorChangeSpeed);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
        spotlight.color = end;
    }

    public void changeColor(Color start, Color end) {
        StartCoroutine(lerpColor(start, end));
    }
}
