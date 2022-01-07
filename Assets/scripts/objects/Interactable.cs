using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool dialogTrigger;
    public signal context;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&!collision.isTrigger)
        {
            context.Raise();
            dialogTrigger = true;          
        }
    }
    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&!collision.isTrigger)
        {
            context.Raise();
            dialogTrigger = false;
        }
    }
}
