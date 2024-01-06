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
    [SerializeField] private float travelTime = 0.2f;

    [Header("Bounds")]
    [SerializeField] private GameObject upperBound;
    [SerializeField] private GameObject lowerBound;
    [SerializeField] private GameObject leftBound;
    [SerializeField] private GameObject rightBound;



    // Start is called before the first frame update
    // Update is called once per frame

    private void Awake() {
        cam = GetComponent<Camera>();
    }
    void Update() {
        currentPos = new Vector3(transform.position.x, transform.position.y, layer);
        targetPos = new Vector3(target.transform.position.x, target.transform.position.y + verticalOffset, layer);
        //PURPOSE: cam follows player
        //cam has regions (defined by "bound" GameObjects) in which it does not move, two per axis
        //i found out later there is a way to do this with math but for now this works so ill keep it
        //
        //this first if statement (plus the else on the other 2) solves an issue
        //without it, the camera only moved horizontally when you were inside the area where the cam couldnt move veritcally (smth like that)

        if (inHorizontalBounds() && inVerticalBounds())
            transform.position = targetPos;
        else if (inHorizontalBounds())
            transform.position = new Vector3(targetPos.x, currentPos.y, layer);
        else if (inVerticalBounds())
            transform.position = new Vector3(currentPos.x, targetPos.y, layer);

        //TODO: make it lerp
        //wip code:
        /*
        if (inHorizontalBounds() && inVerticalBounds())
            transform.position = Vector3.Lerp(currentPos, targetPos, travelTime);
        else if (inHorizontalBounds())
            transform.position = Vector3.Lerp(currentPos, new Vector3(targetPos.x, currentPos.y, layer), travelTime);
        else if (inVerticalBounds())
            transform.position = Vector3.Lerp(currentPos, new Vector3(currentPos.x, targetPos.x, layer), travelTime);*/






        //print(transform.position);
    }

    private bool inHorizontalBounds() {
        return targetPos.x >= leftBound.transform.position.x
               && targetPos.x <= rightBound.transform.position.x;
    }
    private bool inVerticalBounds() {
        return targetPos.y >= lowerBound.transform.position.y
               && targetPos.y <= upperBound.transform.position.y;
    }
}
