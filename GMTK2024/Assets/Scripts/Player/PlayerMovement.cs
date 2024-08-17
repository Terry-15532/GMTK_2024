
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D controller;
    public Rigidbody body;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;
    private bool canMove = true;
    public Animator animator;
    private int jumpPredict = 0;
    // public Gravity gravity;
    public float jumpGrav;
    public float fallGrav;
    private bool isFlipped = false;

    void Start()
    {
        body = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(body.velocity);
        float yVelocity = isFlipped ? -body.linearVelocity.y : body.linearVelocity.y;
        if (yVelocity >= -0.1)
        {
            // gravity.setGravity(jumpGrav);
            animator.SetBool("IsFalling", false);
        }
        else
        {
            // gravity.setGravity(fallGrav);
            animator.SetBool("IsFalling", true);
            // animator.SetBool("IsJumping", true);
        }

        if (jumpPredict > 0)
        {
            jumpPredict--;
            //Debug.Log(jumpPredict);
        }
        if (canMove)
        {
            animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal") * runSpeed));

            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                if (animator.GetBool("IsJumping"))
                {
                    //Debug.Log("Setting jumpPredict");
                    jumpPredict = 100;
                }
                jump = true;
                animator.SetBool("IsJumping", true);
            }
            // if(Input.GetButtonDown("Crouch"))
            // {
            //     crouch = true;
            // }else if(Input.GetButtonUp("Crouch")) {
            //     crouch = false;
            // }
        }
        else
        {
            animator.SetFloat("Speed", 0);
            horizontalMove = 0;
            jump = false;
        }

    }

    public void OnLanding()
    {
        animator.SetBool("IsFalling", false);
        // Debug.Log("Landed");
        if (jumpPredict > 0)
        {
            // Debug.Log("Jump executed from prediction");
            Vector3 vel = body.linearVelocity;
            if (!body.isKinematic)
            {
                body.linearVelocity = new Vector3(vel.x, 0, vel.z);
            }
            controller.Jump(isFlipped);
        }
        else
        {
            // Debug.Log("Normal landing");
            Vector3 vel = body.linearVelocity;
            if (!body.isKinematic)
            {
                body.linearVelocity = new Vector3(vel.x, 0, vel.z);
            }
            animator.SetBool("IsJumping", false);
        }
    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, isFlipped);
        jump = false;
    }

    public void setCanMove(bool move)
    {
        canMove = move;
    }

    public void setFlipped(bool flip)
    {
        isFlipped = flip;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(collision.transform);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            transform.SetParent(null);
        }
    }


    public void setAttached(bool attached)
    {
        animator.SetBool("isAttached", attached);
    }

    public void setAnimJumping(bool jump)
    {
        animator.SetBool("IsJumping", jump);
    }





    public void Attach(GameObject obj)
    {
        transform.SetParent(obj.transform);
        canMove = false;
        // body.isKinematic = true;
        body.freezeRotation = false;
        // gravity.setIsEnabled(false);
        GetComponent<SphereCollider>().enabled = false;
        transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y - 0.1f, obj.transform.position.z);
        transform.localScale = new Vector3(1, 0.125f, 1);
        transform.rotation = obj.transform.rotation;

    }

    public void Release()
    {
        transform.SetParent(null);
        canMove = true;
        // body.isKinematic = false;
        // gravity.setIsEnabled(true);
        GetComponent<SphereCollider>().enabled = true;
        transform.localScale = new Vector3(1, 1, 1);
        transform.rotation = Quaternion.identity;
        body.freezeRotation = true;
    }
}
