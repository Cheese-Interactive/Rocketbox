using UnityEngine;

public abstract class Weapon : MonoBehaviour {
    //Weapons are be used to handle the creation of Projectiles
    //originally, you just made a projectile directly
    //but i thought this would make things complicated for multiprojectile attacks, and came up with this


    // Start is called before the first frame update
    [SerializeField] protected float cooldown; //time between shots
    [SerializeField] protected GameObject projectile;
    public abstract float shoot(Vector3 pos, float degrees);
    public abstract float shoot(Vector3 pos, Quaternion angle);
}
