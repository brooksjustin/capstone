using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectBreak : MonoBehaviour
{
    private Animator anim;
    public LootTable thisLoot;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Break()
    {
        anim.SetBool("break", true);
        StartCoroutine(BreakCo());
        MakeLoot();
    }
    IEnumerator BreakCo()
    {
        yield return new WaitForSeconds(.3f);
        this.gameObject.SetActive(false);
    }
    private void MakeLoot()
    {
        if (thisLoot != null)
        {
            PowerUp current = thisLoot.LootPowerUp();
            if (current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }
}
