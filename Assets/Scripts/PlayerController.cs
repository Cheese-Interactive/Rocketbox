using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Health: Hits to die")]
    [SerializeField] private int health;

    [Header("Speed and multipliers")]
    [SerializeField] private float forceApp;
    [SerializeField] private float maxForce;
    //[SerializeField] private float hoverLaunchForce;
    //[SerializeField] private float hoverGravityCounterAcceleration;
    [Header("")]
    [SerializeField] private float upMultiplier = 1;
    [SerializeField] private float downMultiplier = 1;
    [SerializeField] private float leftMultiplier = 1;
    [SerializeField] private float rightMultiplier = 1;

    private Rigidbody2D rb;
    //i know [vvvvvv] is a weird implementation but idc
    private float hMultiplier;
    private float vMultiplier;
    private bool canAct = true;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        //get keys down
        //again, idk how to do it not sus
        //1 is positive input -1 is negative input
        if (canAct) {
            vMultiplier = Input.GetAxisRaw("Vertical");
            if (vMultiplier == 1)
                vMultiplier *= upMultiplier;
            if (vMultiplier == -1)
                vMultiplier *= downMultiplier;
            hMultiplier = Input.GetAxisRaw("Horizontal");
            if (hMultiplier == 1)
                hMultiplier *= rightMultiplier;
            if (hMultiplier == -1)
                hMultiplier *= leftMultiplier;
        }

        rb.AddForce(transform.up * forceApp * vMultiplier + transform.right * forceApp * hMultiplier);

        //print(System.Math.Abs(rb.totalForce.x) + " " + System.Math.Abs(rb.totalForce.y));
        //janky way of adding a max force. it doesnt work super cleanly but it works
        if (System.Math.Abs(rb.totalForce.x) > maxForce)                           //cant go left or right too fast
            rb.AddForce(-transform.right * forceApp * hMultiplier);
        if (rb.totalForce.y > maxForce)                                            //cant go up too fast
            rb.AddForce(-transform.up * forceApp * vMultiplier);
        if (rb.totalForce.y < -maxForce && Input.GetAxisRaw("Vertical") != 0)      //falling has no limit, thrusting down has a limit
            rb.AddForce(-transform.up * forceApp * vMultiplier);                   //not sure how big of a difference this makes and also has potential issues



    }


}
