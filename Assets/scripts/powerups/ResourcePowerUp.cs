using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePowerUp : PowerUp
{
    public Inventory playerInventory;
    public float resourceValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInventory.currentResource += resourceValue;
            if (playerInventory.currentResource > playerInventory.maxResource)
            {
                playerInventory.currentResource = playerInventory.maxResource;
            }
            powerUpSignal.Raise();
            Destroy(this.gameObject);
        }
    }
}
