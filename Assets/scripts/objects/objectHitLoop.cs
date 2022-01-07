using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectHitLoop : MonoBehaviour
{

    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HitLoop()
    {
        StartCoroutine(HitLoopCo());
    }
    public IEnumerator HitLoopCo()
    {
        anim.SetBool("hit", true);
        yield return new WaitForSeconds(.4f);
        anim.SetBool("hit", false);
    }
}
