using System.Collections;
using UnityEngine;

public class Laserer : Enemy {
    // Start is called before the first frame update
    private bool readyToTurn = true;
    [SerializeField] private bool leftLaserActive;
    [SerializeField] private bool rightLaserActive;
    [SerializeField] private bool upLaserActive;
    [SerializeField] private bool downLaserActive;
    [SerializeField] private GameObject leftLaser;
    [SerializeField] private GameObject rightLaser;
    [SerializeField] private GameObject upLaser;
    [SerializeField] private GameObject downLaser;
    [SerializeField] private Vector2 moveDirection;
    override protected void attack() {
        if (upLaserActive)
            upLaser.SetActive(true);
        if (downLaserActive)
            downLaser.SetActive(true);
        if (leftLaserActive)
            leftLaser.SetActive(true);
        if (rightLaserActive)
            rightLaser.SetActive(true);
    }

    override protected void seekPlayer() {
        rb.velocity = moveDirection.normalized * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player"))
            player.hitPlayer();
        if (collision.gameObject.CompareTag("Walls"))
            StartCoroutine(flipDirection());
        if (collision.gameObject.CompareTag("Enemy") && collision.gameObject.GetComponent<Enemy>() is Laserer)  //bounces on walls and other laserers
            StartCoroutine(flipDirection());
    }

    private IEnumerator flipDirection() {
        if (readyToTurn) {
            moveDirection = -moveDirection.normalized;
            readyToTurn = false;
            yield return new WaitForSeconds(5 / speed);
            readyToTurn = true;
        }
        yield return new WaitForEndOfFrame();
    }
}
