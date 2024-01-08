using UnityEngine;

public class Sweeper : Enemy {
    //Spins long melee weapon and moves to enemy

    [SerializeField] private float angularSpeed;

    protected override void attack() {
        //StartCoroutine(spinToWin(angularSpeed));
        transform.Rotate(new Vector3(0, 0, 1), angularSpeed * Time.deltaTime);
    }
    protected override void seekPlayer() {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

}
