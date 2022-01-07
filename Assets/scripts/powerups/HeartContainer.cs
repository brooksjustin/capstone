using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartContainer : PowerUp
{
    public FloatValue heartContainers;
    public FloatValue playerHealth;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag ("Player"))
        {
            heartContainers.RunTimeValue += 1;
            heartContainers.initialValue += 1;
            playerHealth.initialValue = heartContainers.RunTimeValue * 2;
            playerHealth.RunTimeValue = heartContainers.RunTimeValue * 2;
            powerUpSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
