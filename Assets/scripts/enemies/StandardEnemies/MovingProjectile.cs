using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingProjectile : Enemy
{
    [SerializeField] protected float retreatDistance;
    public GameObject projectile;
    public float fireDelay;
    private float fireDelaySecs;
    public bool canFire = true;
    private void Update()
    {
        if (canFire == false)
        {
            fireDelaySecs -= Time.deltaTime;
            if (fireDelaySecs <= 0)
            {
                canFire = true;
                fireDelaySecs = fireDelay;
            }
        }
    }
    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) < chaseRadius&& Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger && canFire)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                ChangeState(EnemyState.walk);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) < attackRadius&& Vector3.Distance(target.position, transform.position)>retreatDistance&&canFire)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                transform.position = this.transform.position;
                ChangeState(EnemyState.walk);
                FireProjectile();
            }
        }
        else if (Vector3.Distance(target.position, transform.position) < retreatDistance)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger&&canFire)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, -moveSpeed * Time.deltaTime);
                ChangeState(EnemyState.walk);
            }
        }
    }
    private void FireProjectile()
    {
        Vector3 tempVector = target.transform.position - transform.position;
        GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
        current.GetComponent<Projectile>().Launch(tempVector);
        canFire = false;
    }
}
  

