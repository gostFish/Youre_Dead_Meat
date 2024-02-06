using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using TMPro;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    private GameObject GameController;
    private PlayerCommon cmn;
    private PlayerFlags flags;

    private PlayerInteract Interact;
    private PlayerManager pManager;

    private Rigidbody2D rb2D;
    private Animator walkAnimation;

    private RawImage[] controlWidgets;
    private bool controlsReverted;

    private Vector2 direction;
    private Vector2 lastMove;


    // Start is called before the first frame update
    void Awake()
    {
        GameController = GameObject.FindGameObjectWithTag("GameController");

        cmn = GameController.GetComponent<PlayerCommon>();
        flags = transform.GetComponent<PlayerFlags>();

        Interact = transform.GetChild(0).GetComponent<PlayerInteract>();
        pManager = transform.GetComponent<PlayerManager>();
        //ToDo, Move to player Manager
        flags.canMeele = true;
        flags.canMove = true;
        flags.canUseItem = true;
        flags.facingRight = true;
        flags.canJump = true;
        flags.grounded = 0;

        //walkAnimation = cmn.playerWalkAnimation[pManager.playerNumber];
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        walkAnimation = gameObject.GetComponent<Animator>();
        controlsReverted = false;

        //Assign Control Widgets
        controlWidgets = cmn.controlIcons[pManager.playerNumber].transform.GetComponentsInChildren<RawImage>();

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //ToDo: Add in Recieve Direction
        if(this.isActiveAndEnabled && flags.canMove)
        {
            transform.position = new Vector2(transform.position.x + (direction.x * cmn.speed), transform.position.y);
        }
    }


    public void RecieveDirection(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
        
        if (context.canceled || direction == Vector2.zero || flags.hitStunned)
        {
            flags.animateMoving = false;
            walkAnimation.SetBool("IsWalking", flags.animateMoving);
            return;
        }

        if(context.performed)
        {
            if (direction.y == 0)
            {
                flags.animateMoving = true;
                walkAnimation.SetBool("IsWalking", flags.animateMoving);
            }
        }
        
        if (flags.canMove)
        {
            if(direction.x == 1)
            {
                flags.facingRight = true;
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if(direction.x == -1)
            {
                flags.facingRight = false;
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }
        

        if ( direction.y > 0 && flags.canJump && flags.canMove && flags.grounded < 2 && transform.position.y > -35)
        {
            flags.canUseItem = false;
            rb2D.velocity = Vector2.zero;
            rb2D.AddForce(transform.up * cmn.jumpForce, ForceMode2D.Impulse);
            flags.grounded += 1;
            StartCoroutine(JumpCooldown());            
        }
        lastMove = direction;
    }
    
    public void StartHitStunned()
    {
        StartCoroutine(HitStunned());
    }
    public IEnumerator HitStunned()
    {
        flags.hitStunned = true;
        yield return new WaitForSeconds(1f);
        flags.hitStunned = false;

    }

    public IEnumerator SwapControls()
    {
        transform.GetChild(0).transform.GetComponent<PlayerInteract>().PlayPillowsound();
        Texture temp = controlWidgets[1].texture;
        controlWidgets[1].texture = controlWidgets[2].texture;
        controlWidgets[2].texture = temp;

        cmn.speed = cmn.speed * -1;

        controlsReverted = true;
        yield return new WaitForSeconds(5f);

        //Swap back
        controlsReverted = false;

        cmn.speed = cmn.speed * -1;

        temp = controlWidgets[2].texture;
        controlWidgets[2].texture = controlWidgets[1].texture;
        controlWidgets[1].texture = temp;
    }

    

    void OnCollisionEnter2D(Collision2D collision2D)
    {

        flags.canUseItem = true;
        flags.grounded = 0;

        if(collision2D.transform.gameObject.layer == 9)
        {
            //Player hit the out of bounds
            pManager.PlayerKilled();
        }
    }

    IEnumerator JumpCooldown()
    {
        flags.canJump = false;
        yield return new WaitForSeconds(0.05f);
        flags.canJump = true;
    }

}
