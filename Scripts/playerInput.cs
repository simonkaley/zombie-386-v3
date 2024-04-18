using UnityEngine;
using UnityEngine.InputSystem;

public class playerInput : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D jumpingBody, movingBody;
    [SerializeField]
    bool useForce = true;
    [SerializeField]
    float jumpPower = 6f, moveSpeed = 2.3f;
    float sprintMultiplier = 1f;
    float physicsModifier = 100f;

    private bool isSprinting;

    private Animator animator;
    private SpriteRenderer sr;

    Vector2 moveDir = Vector2.zero;

    void Start()
    {
        animator = movingBody.GetComponent<Animator>(); 
        sr = movingBody.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if(movingBody) 
        {
            if(useForce) 
                movingBody.AddForce(moveDir * moveSpeed * Time.deltaTime * physicsModifier, ForceMode2D.Force);
            else
                movingBody.MovePosition(movingBody.position + (moveDir * moveSpeed * sprintMultiplier * Time.deltaTime));
        }
    }

    //This function will provide movement using the new input system
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDir = context.ReadValue<Vector2>() * moveSpeed;

        if (animator != null)
        {
            // Set the "Run" parameter in the Animator to true if the player is moving
            animator.SetBool("Run", moveDir.magnitude > 0.1f);
        }

        // Flip the sprite horizontally if moving left
        if (moveDir.x < 0)
        {
            sr.flipX = true;
        }
        // Flip the sprite horizontally if moving right
        else if (moveDir.x > 0)
        {
            sr.flipX = false;
        }
    }
    
    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValueAsButton();

        if (isSprinting)
        {
            //start sprinting 
            sprintMultiplier = 2f;
        }
        else
        {
            //stop sprinting
            sprintMultiplier = 1f;
        }
    }

    void OnJump()
    {
        if(jumpingBody) 
            jumpingBody.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        // Debug.Log("jump");
    }
}
