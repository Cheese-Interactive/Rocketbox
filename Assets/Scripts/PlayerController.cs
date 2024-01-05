using UnityEngine;

public class PlayerController : MonoBehaviour {

    [Header("Speed and multipliers")]
    [SerializeField] private float thrust;
    [Header("")]
    [SerializeField] private float upMultiplier = 1;
    [SerializeField] private float downMultiplier = 1;
    [SerializeField] private float leftMultiplier = 1;
    [SerializeField] private float rightMultiplier = 1;

    private Rigidbody2D rb;
    //i know [vvvvvv] is a weird implementation but idc
    private float hMultiplier;
    private float vMultiplier;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        //get keys down
        //again, idk how to do it not sus
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

        rb.AddForce(transform.up * thrust * vMultiplier);
        rb.AddForce(transform.right * thrust * hMultiplier);
    }

}
