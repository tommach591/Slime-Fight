using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainMist : Projectile
{
    [SerializeField] protected PoisonMist poisonMist;
    protected PoisonMist poisonMistCopy;

    new public void DestroyProjectile()
    {   
        poisonMistCopy = Instantiate(poisonMist, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
