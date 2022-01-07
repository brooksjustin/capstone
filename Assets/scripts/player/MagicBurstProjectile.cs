using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBurstProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D myRigidBody;
    [SerializeField] private float lifeTime;
    private float lifeTimeSecs;
    public float magicCost;

    // Start is called before the first frame update
    void Start()
    {
        lifeTimeSecs = lifeTime;
    }
    private void Update()
    {
        lifeTimeSecs = lifeTimeSecs - Time.deltaTime;
        if (lifeTimeSecs <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public void SetUp(Vector2 moveDirection)
    {
        myRigidBody.velocity = moveDirection.normalized * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("mob") || collision.gameObject.CompareTag("breakable") || collision.gameObject.CompareTag("object"))
            Destroy(this.gameObject);
    }
}
