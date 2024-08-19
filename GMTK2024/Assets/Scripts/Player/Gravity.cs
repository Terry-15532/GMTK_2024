using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Gravity : MonoBehaviour
{
    // Gravity Scale editable on the inspector
    // providing a gravity scale per object

    private PlayerMovement playerMovement;
    public float gravityScale = 1.0f;
    public int ammo = 2;

    // Global Gravity doesn't appear in the inspector. Modify it here in the code
    // (or via scripting) to define a different default gravity for all objects.

    public float globalGravity = -9.81f;
    private bool isEnabled = true;
    // private bool canFlip = true;

    Rigidbody m_rb;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void OnEnable()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.useGravity = false;
    }

    public void setIsEnabled(bool enable)
    {
        isEnabled = enable;
    }

    void FixedUpdate()
    {
        if (isEnabled)
        {
            Vector3 gravity = globalGravity * gravityScale * Vector3.up;
            m_rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }

    public void setGravity(float grav)
    {
        gravityScale = grav;
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Minus) && ammo > 0 && canFlip)
        // {
        //     GetComponent<AudioSource>().Play();
        //     Flip();
        //     ammo--;
        // }
    }

    public void Flip()
    {
        globalGravity *= -1;
        playerMovement.setFlipped(globalGravity > 0);
        playerMovement.setAnimJumping(true);
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x, -scale.y, scale.z);

    }

    public int getAmmo()
    {
        return ammo;
    }

    public void setAmmo(int newAmmo)
    {
        ammo = newAmmo;
    }

    // public void setCanFlip(bool flip)
    // {
    //     canFlip = flip;
    // }

}


