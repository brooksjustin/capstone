using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    key,
    enemy,
    button
}

public class Door : Interactable
{

    public DoorType thisDoorType;
    public bool open=false;
    public Inventory playerInventory;
    public SpriteRenderer doorSprite;
    public BoxCollider2D physicsCollider;

    public void Update()
    {
        if (Input.GetButtonDown("interact"))
        {
            if (dialogTrigger&&thisDoorType==DoorType.key)
            {
                //check for key, and open the door if yes,remove a key from inventory
                if (playerInventory.numberOfKeys > 0)
                {
                    playerInventory.numberOfKeys--;
                    Open();
                }
            }
        }
    }
    public void Open()
    {
        //turn off sprite,set open to true, turn off collider
        doorSprite.enabled = false;
        open = true;
        physicsCollider.enabled = false;
    }
    public void Close()
    {
        doorSprite.enabled = true;
        open = false;
        physicsCollider.enabled = true;
    }
}
