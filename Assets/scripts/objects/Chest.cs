using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : Interactable
{
    public Inventory playerInventory;
    public Item contents;
    public bool isOpen;
    public signal getItem;
    public GameObject dialogBox;
    public Text dialogText;
    private Animator anim;
    public BoolValue storedOpen;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        isOpen = storedOpen.RunTimeValue;
        if (isOpen)
        {
            anim.SetBool("opened", true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("interact") && 
            dialogTrigger)
        {
            if (!isOpen)
            {
                //open the chest
                OpenChest();
            }
            else
            {
                // chest is already open
                OpenAlready();
            }
        }
    }
    public void OpenChest()
    {
        //dialog window on, text=content text, add contents to inventory, signal player to animate,set chest to open,raise context clue
        dialogBox.SetActive(true);
        dialogText.text = contents.itemDesc;
        playerInventory.AddItem(contents);
        playerInventory.currentItem = contents;
        getItem.Raise();
        context.Raise();
        isOpen = true;
        anim.SetBool("opened", true);
        storedOpen.RunTimeValue = isOpen;
    }
    public void OpenAlready()
    {       
        //dialog off, set current time to null, raise the signal to the player to stop animating
        dialogBox.SetActive(false);          
        getItem.Raise();
        dialogTrigger = false;
        
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpen)
        {
            context.Raise();
            dialogTrigger = true;
        }
    }
    public override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpen)
        {
            context.Raise();
            dialogTrigger = false;
        }
    }
}
