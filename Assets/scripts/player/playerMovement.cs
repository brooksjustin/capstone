using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
//for state machine
public enum PlayerState
{
    idle,
    walk,
    attack,
    interact,
    stagger
}
public class playerMovement : MonoBehaviour
{
    [Header("player stats")]
    [SerializeField] protected float speed;
    public FloatValue currentHealth;
    public signal playerHealthSignal;
    [Header("arrow stuff")]
    public GameObject projectile;
    public float fireDelay;
    private float fireDelaySecs;
    public bool canFire = true;
    [Header("magic burst stuff")]
    public GameObject magicBurstObject;
    public signal reduceResource;
    [SerializeField] private int magicBurstNum;
    [SerializeField] public float magicBurstSpread;
    [Header("Dash Stuff")]
    [SerializeField]private float dashForce;
    [SerializeField] private float dashDuration;
    [SerializeField] private int dashCost;
    [Header("item Stuff")]
    public Item bow;
    public Item magicDevice;
    public Item dashBoots;
    [Header("state")]
    public PlayerState currentState;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator anim;
    public VectorValue startingPosistion;
    public Inventory playerInventory;
    public SpriteRenderer recievedItemSprite;
    private Vector2 facingDirection = Vector2.down;


    // Start is called before the first frame update
    void Start()
    {
        currentHealth.RunTimeValue = currentHealth.initialValue;
        currentState = PlayerState.walk;
        //refrences charachter rigidbody
        myRigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetFloat("movex", 0);
        anim.SetFloat("movey", -1);
        transform.position = startingPosistion.initialValue;
    }

    // Update is called once per frame
    void Update()
    {  //is player interacting?
        if (currentState == PlayerState.interact)
        {
            return;
        }
        // gets input from keyboard and cases change
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        change.y = Input.GetAxisRaw("Vertical");
        //arrow fire delay
        if (canFire == false)
        {
            fireDelaySecs -= Time.deltaTime;
            if (fireDelaySecs <= 0)
            {
                canFire = true;
                fireDelaySecs = fireDelay;
            }
        }
        //attack section
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }
        else if (Input.GetButtonDown("shoot") && currentState != PlayerState.attack && currentState != PlayerState.stagger && canFire)
        {
            if (playerInventory.CheckForItem(bow))
            {
                StartCoroutine(ArrowAttackCo());
            }
        }
        else if (Input.GetButtonDown("magic") && currentState != PlayerState.attack && currentState != PlayerState.stagger && canFire)
        {
            if (playerInventory.CheckForItem(magicDevice))
            {
                StartCoroutine(MagicBurstCo());
            }
        }
        else if (Input.GetButtonDown("dash") && currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            if (playerInventory.CheckForItem(dashBoots))
            {
                StartCoroutine(DashCo());
            }
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpdateAnimationAndMove();
        }
    }

    private IEnumerator AttackCo()
    {
        anim.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        anim.SetBool("attacking", false);
        yield return new WaitForSeconds(.21f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }
    private IEnumerator ArrowAttackCo()
    {
        currentState = PlayerState.attack;
        yield return null;
        MakeArrow();
        canFire = false;
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }
    private IEnumerator MagicBurstCo()
    {
        currentState = PlayerState.attack;
        yield return null;
        MagicBurst();
        canFire = false;
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }
    private IEnumerator DashCo()
    {
        currentState = PlayerState.walk;
        yield return null;
        UseDash(facingDirection);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }

    void MoveCharacter()
    {
        change.Normalize();
        //formula for the charachter movement 
        myRigidbody.MovePosition(transform.position + change * speed * Time.deltaTime);
    }
    void UpdateAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            change.x = Mathf.Round(change.x);
            change.y = Mathf.Round(change.y);
            anim.SetFloat("movex", change.x);
            anim.SetFloat("movey", change.y);
            anim.SetBool("moving", true);
            facingDirection = change;
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }
    public void Knock(float knockTime, float damage)
    {
        currentHealth.RunTimeValue -= damage;
        playerHealthSignal.Raise();
        if (currentHealth.RunTimeValue > 0)
        {
            StartCoroutine(KnockCo(knockTime));
        }
        else
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene("Game Over");
        }
    }
    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            currentState = PlayerState.idle;
            myRigidbody.velocity = Vector2.zero;
        }
    }
    public void RaiseItem()
    {
        if (playerInventory.currentItem != null)
        {
            if (currentState != PlayerState.interact)
            {
                anim.SetBool("getitem", true);
                currentState = PlayerState.interact;
                recievedItemSprite.sprite = playerInventory.currentItem.itemSprite;
            }
            else
            {
                anim.SetBool("getitem", false);
                currentState = PlayerState.idle;
                recievedItemSprite.sprite = null;
                playerInventory.currentItem = null;
            }
        }
    }
    private void MakeArrow()
    {
        if (playerInventory.currentResource >= 2)
        {
            Vector2 temp = new Vector2(anim.GetFloat("movex"), anim.GetFloat("movey"));
            Arrow arrow = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.SetUp(temp, ChooseArrowDirection());
            playerInventory.ReduceResource(arrow.magicCost);
            reduceResource.Raise();
        }
    }
    private void MagicBurst()
    {

        if (playerInventory.currentResource >= 4)
        {
            float facingRotation = Mathf.Atan2(anim.GetFloat("movey"), anim.GetFloat("movex")) * Mathf.Rad2Deg;
            float startRotation = facingRotation + magicBurstSpread / 2f;
            float angleIncrease = magicBurstSpread / ((float)magicBurstNum - 1f);
            MagicBurstProjectile magic;
            for (int i = 0; i < magicBurstNum; i++)
            {
                float tempRot = startRotation - angleIncrease * i;
                GameObject newProjectile = Instantiate(magicBurstObject, transform.position, Quaternion.Euler(0f, 0f, facingRotation));
                magic = newProjectile.GetComponent<MagicBurstProjectile>();
                if (magic)
                {
                    magic.SetUp(new Vector2(Mathf.Cos(tempRot * Mathf.Deg2Rad), Mathf.Sin(tempRot * Mathf.Deg2Rad)));
                }
                playerInventory.ReduceResource(magic.magicCost);
                reduceResource.Raise();
            }
        }
    }
    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(anim.GetFloat("movey"), anim.GetFloat("movex")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }
    private void UseDash(Vector2 facingDirection)
    {
        if (playerInventory.currentResource >= 1)
        {
            playerInventory.currentResource -= dashCost;
            reduceResource.Raise();
            Vector3 dashVector = myRigidbody.transform.position + (Vector3)facingDirection.normalized * dashForce;
            myRigidbody.DOMove(dashVector, dashDuration);
        }
        else { return; }
    }
}



