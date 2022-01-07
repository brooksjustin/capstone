using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : Enemy
{
    private Animator anim;
    public SpriteRenderer spriteRenderer;
    Color orginalColor;
    [SerializeField] private float flashTime;
    private void Start()
    {
        orginalColor = spriteRenderer.color;
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
    }
    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) <= chaseRadius
    && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                anim.SetBool("fall", true);
                anim.SetBool("rise", false);
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
            }
        }
        if (Vector3.Distance(target.position, transform.position) < attackRadius)
        {
            anim.SetBool("rise", true);
            anim.SetBool("fall", false);
            Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            myRigidbody.MovePosition(temp);
            ChangeState(EnemyState.walk);
        }
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (!isDead)
        {
            StartCoroutine(FlashCo(flashTime));
        }
    }
    private IEnumerator FlashCo(float flashTime)
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.color = orginalColor;
    }
}
