using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    public float speed = 75;
    public float jumpForce = 75;
    public Transform groundCheck;
    public LayerMask groundLabel;
    public int possibleJumps = 1;
    public float arialJumpFraction = 0.75f;

    private Rigidbody2D rb;
    private float horizontalComponent;
    private bool facingRight = true;
    private bool isGrounded;
    private int jumpsLeft;
    private float jumpComponent;

    private Crafting crafting;
    private Inventory inv;

    private Vitality vit;

    private TriggerTouch triggerTouch;

    public string playerSwitchScriptTag;
    private PlayerSwitch playerSwitch;

    public Animator animator;

    public GameObject nestPrefab;

    private LevelManager levelManager;

    public PlayerSound playerSound;

    private bool active = true;
    private bool isNested = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpsLeft = possibleJumps;

        crafting = GetComponent<Crafting>();
        inv = GetComponent<Inventory>();
        vit = GetComponent<Vitality>();

        triggerTouch = GetComponent<TriggerTouch>();

        playerSwitch = GameObject.FindGameObjectWithTag(playerSwitchScriptTag).GetComponent<PlayerSwitch>();
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }

    private void Update() {
        // Horizontal Movement
        if (active)
            horizontalComponent = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed_Abs", Mathf.Abs(horizontalComponent));
        if ((horizontalComponent > 0 && !facingRight) || (horizontalComponent < 0 && facingRight))
            Flip();


        // Jump
        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, groundLabel);
        animator.SetBool("Grounded", isGrounded);
        if (isGrounded)
            jumpsLeft = possibleJumps;
        
        jumpComponent = rb.velocity.y;
        if (Input.GetKeyDown("space") && active)
        {
            if (isGrounded)
            {
                jumpComponent = jumpForce;
                isGrounded = false;

                animator.SetBool("Grounded", false);
                animator.SetTrigger("Jump");
                playerSound.Flap();
            }
            else if (jumpsLeft > 0)
            {            
                jumpComponent = jumpForce * arialJumpFraction;
                animator.SetTrigger("Jump");
                playerSound.Flap();
            }

            jumpsLeft--;
        }
        rb.velocity = new Vector2(horizontalComponent * speed, jumpComponent);

        // Build Nest
        if (Input.GetKeyDown("e") && active) {
            if (crafting.BuildItem(inv, "Nest")) {
                GameObject clone = Instantiate(nestPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                clone.transform.position = gameObject.transform.position;
            }
        }

        // Lay Egg
        if (Input.GetKeyDown("s") && active) {
            if (vit.GetVitality(Vitality.Stat.Satiety) > vit.GetMaxVitality(Vitality.Stat.Satiety) * .75) {
                if (triggerTouch.GetTargetsTouched() > 0) {
                    playerSwitch.NestPlayer(gameObject);
                }
            }
        }

        // Check fall offscreen
        if (transform.position.y < levelManager.GetBottomOfMap())
            GameOver();
    }

    void Flip()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void ResetPlayer() {
        active = true;
        isNested = false;
        
        // Reset inventory, vitality, animator
        inv.ResetInventory();
        vit.ResetVitality();
        animator.SetBool("Nested", false);
    }

    public void GameOver() {
        // animator.SetTrigger("Die");
        SetActive(false);
        levelManager.GameOver(gameObject);
    }
    
    public bool IsWalking() {
        return isGrounded && Mathf.Abs(rb.velocity.x) > 0f;
    }

    public void SetActive(bool active) {
        this.active = active;
    }

    public bool IsActive() {
        return active;
    }

    public void SetNested(bool nested) {
        isNested = nested;
    }

    public bool IsNested() {
        return isNested;
    }
}
