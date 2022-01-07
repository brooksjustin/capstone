using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitScript : MonoBehaviour
{
    public float force;
    public float knockTime;
    public float damage;
    public FloatValue playerDamage;
    [SerializeField] private string targetTag;
    [SerializeField] private bool canBreak;
    [SerializeField] private bool canHit;
    public void Start()
    {
        if (this.gameObject.CompareTag("playerhitbox"))
        {
            damage = playerDamage.RunTimeValue;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("hittable") && canHit == true)
         {
         collision.GetComponent<objectHitLoop>().HitLoop();
         }

        if (collision.gameObject.CompareTag("breakable") && canBreak==true)
         {
            collision.GetComponent<objectBreak>().Break();
         }
        if (collision.gameObject.CompareTag(targetTag))
        {
            Rigidbody2D hit = collision.GetComponent<Rigidbody2D>();
            if (hit != null)
            {
                Vector2 diff = hit.transform.position - transform.position;
                diff = diff.normalized * force;
                hit.AddForce(diff, ForceMode2D.Impulse);
                if (collision.gameObject.CompareTag("mob") && collision.isTrigger)
                {
                    hit.GetComponent<Enemy>().currentState = EnemyState.stagger;
                    collision.GetComponent<Enemy>().Knock(hit, knockTime, damage, collision.GetComponent<Enemy>().isDead);
                }
                if (collision.gameObject.CompareTag("Player")&& collision.GetComponent<playerMovement>().currentState != PlayerState.stagger&&collision.isTrigger)
                {
                    hit.GetComponent<playerMovement>().currentState = PlayerState.stagger;
                    collision.GetComponent<playerMovement>().Knock(knockTime, damage);                   
                }
            }
        }
    }
}
