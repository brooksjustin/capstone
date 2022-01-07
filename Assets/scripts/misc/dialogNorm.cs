using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class dialogNorm : Interactable
{
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("interact") && dialogTrigger)
        {
            if (dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(false);
            }
            else
            {
                dialogBox.SetActive(true);
                dialogText.text = dialog;
            }
        }
    }
    public override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&& !collision.isTrigger)
        {
            dialogTrigger = false;
            dialogBox.SetActive(false);
            context.Raise();
        }
    }
}

