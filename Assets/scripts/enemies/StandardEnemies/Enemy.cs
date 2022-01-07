using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    idle,
    walk,
    attack,
    stagger
}

public class Enemy : MonoBehaviour
{
    [Header("enemy stats")]
    public EnemyState currentState;
    public FloatValue maxHealth;
    public float health;
    [SerializeField]protected string enemyName;
    [SerializeField] protected int baseAttack;
    [SerializeField] protected float moveSpeed;
    [Header("Loot")]
    [SerializeField] protected LootTable thisLoot;
    [Header("aggro info")]
    [SerializeField] protected float chaseRadius;
    [SerializeField] protected float attackRadius;
    [Header("targeting ")]
    [SerializeField] protected Vector2 home;
    [SerializeField] protected Transform target;
    [Header("death stuff")]
    public signal roomSignal;
    public bool isDead;
    [Header("declarations")]
    [SerializeField] protected Rigidbody2D myRigidbody;

    void Start()
    {
        currentState = EnemyState.idle;
        myRigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
    }
    void FixedUpdate()
    {
        CheckDistance();
    }
    private void Awake()
    {
        home = transform.position;
        health = maxHealth.initialValue;
    }
    private void OnEnable()
    {
        transform.position = home;
        health = maxHealth.initialValue;
        currentState = EnemyState.idle;
    }
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            MakeLoot();
            roomSignal.Raise();
            this.gameObject.SetActive(false);
            isDead = true;
        }
    }
    public void Knock(Rigidbody2D myRigidBody, float knockTime,float damage,bool isDead)
    {
        if (isDead)
        {
            return;
        }      
        StartCoroutine(KnockCo(myRigidBody, knockTime));
        TakeDamage(damage);    
    }
    private IEnumerator KnockCo(Rigidbody2D myRigidbody2D,float knockTime)
    {
        if ( myRigidbody2D!= null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody2D.velocity = Vector2.zero;
            currentState= EnemyState.idle;
            myRigidbody2D.velocity = Vector2.zero;
        }
    }
    private void MakeLoot()
    {
        if (thisLoot != null)
        {
            PowerUp current = thisLoot.LootPowerUp();
            if (current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }
    public void ChangeState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
        }
    }
    public virtual void CheckDistance()
    {
    if (Vector3.Distance(target.position, transform.position) <= chaseRadius
     && Vector3.Distance(target.position, transform.position) > attackRadius)
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                myRigidbody.MovePosition(temp);
                ChangeState(EnemyState.walk);
            }
        }
    }
}

