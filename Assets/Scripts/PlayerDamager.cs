using UnityEngine;

public class PlayerDamager : MonoBehaviour {
    [SerializeField] private bool isTriggerOnly;
    private PlayerController player;

    private void Start() {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!isTriggerOnly)
            if (collision.gameObject.CompareTag("Player"))
                player.hitPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag.Equals("Player"))
            player.hitPlayer();
    }
}

