using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    //Controls camera following the player and other features

    private int layer = -50;
    //[^^^^^] is a result of me figuring out how to "layer" gameobjects in unity2d
    private Camera cam;
    Vector3 targetPos = new Vector3(0, 0, 0);
    Vector3 currentPos = new Vector3(0, 0, 0);

    [SerializeField] private GameObject target;
    [SerializeField] private float verticalOffset;
    [SerializeField] private float travelTime;

    [Header("Bounds")]
    [SerializeField] private GameObject upperBound;
    [SerializeField] private GameObject lowerBound;
    [SerializeField] private GameObject leftBound;
    [SerializeField] private GameObject rightBound;

    [Header("Full Scene View")]
    //when changing sizes, remember to adjust cam bounds
    [SerializeField] private float defaultCamSize;
    [SerializeField] private float zoomCamSize;
    [SerializeField] private float zoomTime;
    [SerializeField] private Vector3 zoomCamPos; //not sure if this is needed anymore, was at one point
    private bool isPosLerping = false;
    private bool isFovLerping = false;
    private bool isZoomedOut = false;



    // Start is called before the first frame update
    // Update is called once per frame

    private void Awake() {
        cam = GetComponent<Camera>();
        cam.orthographicSize = defaultCamSize;
    }
    void Update() {
        currentPos = new Vector3(transform.position.x, transform.position.y, layer);
        targetPos = new Vector3(target.transform.position.x, target.transform.position.y + verticalOffset, layer);

        ZoomOut();
        //cam follows player
        //cam has regions (defined by "bound" GameObjects) in which it does not move, two per axis
        //i found out later there is a way to do this with math but for now this works so ill keep it
        //
        //this first if statement (plus the else on the other 2) solves an issue
        //without it, the camera only moved horizontally when you were inside the area where the cam couldnt move veritcally (smth like that)

        /*  old code: no lerping
         *  if (inHorizontalBounds() && inVerticalBounds() && !camFrozen)
              transform.position = targetPos;
          else if (inHorizontalBounds() && !camFrozen)
              transform.position = new Vector3(targetPos.x, currentPos.y, layer);
          else if (inVerticalBounds() && !camFrozen)
              transform.position = new Vector3(currentPos.x, targetPos.y, layer); */

        if (inHorizontalBounds() && inVerticalBounds() && !isPosLerping)
            StartCoroutine(lerpToTarget(targetPos, travelTime));
        else if (inHorizontalBounds() && !isPosLerping)
            StartCoroutine(lerpToTarget(new Vector3(targetPos.x, currentPos.y, layer), travelTime));
        else if (inVerticalBounds() && !isPosLerping)
            StartCoroutine(lerpToTarget(new Vector3(currentPos.x, targetPos.y, layer), travelTime));


    }

    private void UpdateCamPos() {


    }

    private bool inHorizontalBounds() {
        return targetPos.x >= leftBound.transform.position.x
               && targetPos.x <= rightBound.transform.position.x;
    }
    private bool inVerticalBounds() {
        return targetPos.y >= lowerBound.transform.position.y
               && targetPos.y <= upperBound.transform.position.y;
    }

    private void ZoomOut() {
        //BUG: when exiting full scene view while player is out of camera bounds, it takes a second for the camera to sync back up
        if (Input.GetKeyDown(KeyCode.Space) && !isFovLerping && !isZoomedOut) {
            StartCoroutine(lerpCamSize(zoomCamSize, zoomTime));
            transform.position = zoomCamPos;
            isZoomedOut = true;
        }
        if (!Input.GetKey(KeyCode.Space) && isZoomedOut && !isFovLerping) {
            //reset
            StartCoroutine(lerpCamSize(defaultCamSize, zoomTime));
            transform.position = currentPos;
            isZoomedOut = false;
        }
    }

    //various lerping formulas and their curves
    //https://chicounity3d.wordpress.com/2014/05/23/how-to-lerp-like-a-pro/
    private IEnumerator lerpToTarget(Vector3 target, float duration) {
        isPosLerping = true;
        float t = 0;
        //no it doesnt say TEASE its just the variable time for the easing curve
        float tEase = t / duration;
        while (t < duration) {
            tEase = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(currentPos, target, tEase);
            t += Time.deltaTime;
            yield return null;
        }
        isPosLerping = false;
    }

    private IEnumerator lerpCamSize(float target, float duration) {
        isFovLerping = true;
        float t = 0;
        float tEase = t / duration;
        while (t < duration) {
            tEase = t * t * t * (t * (6f * t - 15f) + 10f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, target, tEase);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(duration / 10);
        isFovLerping = false;
        print("Ready");
    }

}
