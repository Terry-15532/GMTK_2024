using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    public Rigidbody rb;
    public ShadowChecker checker;
    public float speed = 5;
    public static Character instance;
    public bool jumpKeyDown;

    public MeshRenderer model;

    public bool grounded, canJump = true;

    public bool shadowMode;

    public Vector3 initPos, initScale;

    public SpriteRenderer sr;

    public Animator animator;

    public static readonly int falling = Animator.StringToHash("Falling");

    private static readonly int running = Animator.StringToHash("Running");

    private static readonly int jumping = Animator.StringToHash("Jumping");

    public void Reset()
    {
        transform.position = initPos;
        rb.linearVelocity = Vector3.zero;
    }

    public void Awake()
    {
        instance = this;
        initPos = transform.position;
        rb = GetComponent<Rigidbody>();
        checker = GetComponent<ShadowChecker>();
        animator = GetComponent<Animator>();
        // sr = GetComponent<SpriteRenderer>();
        canJump = true;
        model = GetComponentInChildren<MeshRenderer>();
        initScale = model.transform.localScale;
    }


    public void Start()
    {
        Stage.instance.resetStage.AddListener(Reset);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.contacts.Any(contact => contact.point.y < transform.position.y - 0.35f))
        {
            canJump = true;
            grounded = true;
        }
    }

    public void OnCollisionExit(Collision other)
    {
        grounded = false;
    }

    public void UpdateModel()
    {
        if (shadowMode)
        {
            model.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            var lightPos = Stage.instance.currLight.position;
            var cameraPos = GameInfo.mainCamera.transform.position;
            Ray wallRay = new Ray(transform.position, transform.position - cameraPos);
            Debug.DrawRay(wallRay.origin, wallRay.direction, Color.cyan);
            Physics.Raycast(wallRay, out RaycastHit hit, 1000, Stage.wallLayer);
            var wallPos = hit.point;
            Ray ray = new Ray(wallPos, lightPos - wallPos);
            float distance = (Stage.instance.platformZ - ray.origin.z) / ray.direction.z;
            float lightRatio = (lightPos.z - Stage.instance.platformZ) / (lightPos.z - ray.origin.z);
            float cameraRatio = (cameraPos.z - ray.origin.z) / (cameraPos.z - Stage.instance.platformZ);
            Vector3 pos = ray.GetPoint(distance);
            model.transform.position = pos;
            model.transform.localScale = initScale * (lightRatio * cameraRatio);
        }
        else
        {
            model.shadowCastingMode = ShadowCastingMode.Off;
            model.transform.localPosition = Vector3.zero;
            model.transform.localScale = initScale;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpKeyDown = true;
        }
    }

    public void FixedUpdate()
    {
        var top = checker.HitTop(out float topDist);
        var bottom = checker.HitBottom(out float bottomDist);
        var left = checker.HitLeft(out float leftDist);
        var right = checker.HitRight(out float rightDist);


        var finalV = new Vector3(Input.GetAxis("Horizontal") * speed, rb.linearVelocity.y, 0);

        finalV.y += -9.8f * Time.fixedDeltaTime; //改为手动设置重力，否则即使设置速度y分量为零仍然会向下动

        if (jumpKeyDown)
        {
            jumpKeyDown = false;
            if (canJump)
            {

                finalV.y = 5;
                canJump = false;
                StopAllCoroutines();
                animator.SetBool(jumping, true);
                Tools.CallDelayed(() => { animator.SetBool(jumping, false); }, 0.25f);
            }
        }

        if (bottom)
        {
            canJump = true;
            if (finalV.y <= 0)
            {
                finalV.y = top ? 3 : 0;
                transform.position += new Vector3(0, bottomDist, 0);
            }
        }

        else if (top && finalV.y > 0)
        {
            finalV.y = 0;
            transform.position -= new Vector3(0, topDist, 0);
        }

        if (left && finalV.x < 0)
        {
            finalV.x = 0;
            transform.position += new Vector3(leftDist, 0, 0);
        }

        if (right && finalV.x > 0)
        {
            finalV.x = 0;
            transform.position -= new Vector3(rightDist, 0, 0);
        }

        if (bottom || left || right)
        {
            shadowMode = true;
        }
        else
        {
            shadowMode = false;
        }
        // if (finalV.x > 0){
        // 	sr.flipX = false;
        // }
        // else if (finalV.x < 0){
        // 	sr.flipX = true;
        // }

        if (bottom || grounded)
        {
            animator.SetBool(running, Mathf.Abs(finalV.x) > 0);
            animator.SetBool(falling, false);
        }
        else
        {
            animator.SetBool(falling, true);
        }

        UpdateModel();

        rb.linearVelocity = finalV;
    }
}