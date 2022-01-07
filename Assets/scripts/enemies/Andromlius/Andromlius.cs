using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Andromlius : Enemy
{
    public Animator anim;
    void Start()
    {
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }
    public override void CheckDistance()
    {

        if (Vector3.Distance(target.position, transform.position) <= chaseRadius
            && Vector3.Distance(target.position,transform.position)>attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk&& currentState!=EnemyState.stagger)
            {
                anim.SetBool("wakeup", true);
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
            }
        }
        else
        {
            anim.SetBool("wakeup", false);
        }
    }
}
