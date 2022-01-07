using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("movement stats")]
    [SerializeField] public float moveSpeed;
    public Vector2 directionToMove;
    [Header("duration")]
    [SerializeField] private float lifeTime;
    private float lifeTimeSecs;
    public Rigidbody2D myRigidbody;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        lifeTimeSecs = lifeTime;
    }

    // Update is called once per frame
   private void Update()
    {
        lifeTimeSecs=lifeTimeSecs-Time.deltaTime;
      if (lifeTimeSecs <= 0)
      {
        Destroy(this.gameObject);
      }
    }
    public void Launch(Vector2 initialVelocity)
    {
        myRigidbody.velocity = initialVelocity.normalized * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        Destroy(this.gameObject);
    }
}
