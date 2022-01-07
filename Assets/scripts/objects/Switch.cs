using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool active;
    public BoolValue storedValue;
    [SerializeField] private Sprite activeSprite;
    private SpriteRenderer mySprite;
    [SerializeField] private Door thisDoor;
    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        active = storedValue.RunTimeValue;
        if (active)
        {
            ActivateSwitch();
        }
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ActivateSwitch();
        }
    }
    public void ActivateSwitch()
    {
        active = true;
        storedValue.RunTimeValue = active;
        thisDoor.Open();
        mySprite.sprite = activeSprite;
    }
}
