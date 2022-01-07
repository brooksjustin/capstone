using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magephase3 : Enemy
{
    private Animator anim;
    [SerializeField] protected float retreatDistance;
    public float fireDelay;
    private float fireDelaySecs;
    public bool canFire = true;
    [Header("magic burst stuff")]
    public GameObject magicBurstObject;
    [SerializeField] private int magicBurstNum;
    [SerializeField] public float magicBurstSpread;
    private Vector3 targetPosistion;
    [Header("projectilestuff")]
    public GameObject projectile;
    public float fireDelay2;
    private float fireDelaySecs2;
    public bool canFire2 = true;
    private void Start()
    {
        anim = GetComponent<Animator>();
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
    }
    private void Update()
    {
        if (canFire2 == false)
        {
            fireDelaySecs2 -= Time.deltaTime;
            if (fireDelaySecs2 <= 0)
            {
                canFire2 = true;
                fireDelaySecs2 = fireDelay2;
            }
        }
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
        if (Vector3.Distance(target.position, transform.position) < chaseRadius && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger && canFire)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
            }
        }
        else if (Vector3.Distance(target.position, transform.position) < attackRadius && Vector3.Distance(target.position, transform.position) > retreatDistance && canFire)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                transform.position = this.transform.position;
                ChangeState(EnemyState.walk);
                MagicBurst();
            }
        }
        else if (Vector3.Distance(target.position, transform.position) < retreatDistance)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger && canFire2)
            {
                transform.position = this.transform.position;
                ChangeState(EnemyState.walk);
                FireProjectile();
            }
        }
    }
    private void MagicBurst()
    {
        targetPosistion = target.transform.position - transform.position;
        float facingRotation = Mathf.Atan2(targetPosistion.y, targetPosistion.x) * Mathf.Rad2Deg;
        float startRotation = facingRotation + magicBurstSpread / 2f;
        float angleIncrease = magicBurstSpread / ((float)magicBurstNum - 1f);
        Projectile magic;
        for (int i = 0; i < magicBurstNum; i++)
        {
            float tempRot = startRotation - angleIncrease * i;
            GameObject newProjectile = Instantiate(magicBurstObject, transform.position, Quaternion.Euler(0f, 0f, facingRotation));
            magic = newProjectile.GetComponent<Projectile>();
            if (magic)
            {
                magic.Launch(new Vector2(Mathf.Cos(tempRot * Mathf.Deg2Rad), Mathf.Sin(tempRot * Mathf.Deg2Rad)));
            }
        }
        canFire = false;
    }
    private void FireProjectile()
    {
        Vector3 tempVector = target.transform.position - transform.position;
        GameObject current = Instantiate(projectile, transform.position, Quaternion.identity);
        current.GetComponent<Projectile>().Launch(tempVector);
        canFire2 = false;
    }
}

