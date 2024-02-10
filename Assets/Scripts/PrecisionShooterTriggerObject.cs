using UnityEngine;

public class PrecisionShooterTriggerObject : MonoBehaviour {

    [SerializeField] private GameObject reciever;
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals("Player"))
            reciever.GetComponent<PrecisionShooter>().recieveTrigger();
    }

}

