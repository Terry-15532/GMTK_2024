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
    public float jumpSpeed = 5;
    public static Character instance;
    public bool jumpKeyDown;

    public MeshRenderer model;

    public bool wallGrounded;
    public bool shadowGrounded;
    public bool canMoveLight = true;

    public bool shadowMode;

    public Vector3 initPos, initScale;

    public SpriteRenderer sr;

    public Animator animator;

    public static readonly int falling = Animator.StringToHash("Falling");

    private static readonly int running = Animator.StringToHash("Running");

    private static readonly int jumping = Animator.StringToHash("Jumping");
    private int collisionCount;

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
        model = GetComponentInChildren<MeshRenderer>();
        initScale = model.transform.localScale;
    }


    public void Start()
    {
        Stage.instance.resetStage.AddListener(Reset);
    }

    public void OnCollisionEnter(Collision other)
    {
        collisionCount++;
        shadowMode = false;
        if (other.contacts.Any(contact => contact.point.y < transform.position.y - 0.55f))
        {
            // Debug.Log("collision enter ground");
            StopAllCoroutines();
            wallGrounded = true;
            canMoveLight = true;
            foreach (var l in Stage.instance.currLights)
            {
                var lamp = l.GetComponent<MovableLamp>();
                if (lamp != null && lamp.mouseOn)
                {
                    lamp.outline.enabled = true;
                }
            }
        }
    }

    public void OnCollisionExit(Collision other)
    {
        collisionCount--;
        if (collisionCount <= 0)
        {
            wallGrounded = false;
            Tools.CallDelayed(() => { wallGrounded = false; }, 0.1f);
            canMoveLight = false;
            foreach (var l in Stage.instance.currLights)
            {
                var lamp = l.GetComponent<MovableLamp>();
                if (lamp)
                {
                    lamp.isDragging = false;
                    lamp.outline.enabled = false;
                }
            }
        }
    }

    public void UpdateModel()
    {
        if (shadowMode)
        {
            model.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            var cameraPos = GameInfo.mainCamera.transform.position;
            Ray wallRay = new Ray(transform.position, transform.position - cameraPos);
            Debug.DrawRay(wallRay.origin, wallRay.direction, Color.cyan);
            Physics.Raycast(wallRay, out RaycastHit hit, 1000, Stage.wallLayer);
            var wallPos = hit.point;

            if (Stage.instance.currLights.Length > 0)
            {

                Transform inLight = Stage.instance.currLights[0];
                foreach (var l in Stage.instance.currLights)
                {
                    Ray r = new Ray(wallPos, l.position - wallPos);
                    if (checker.InLight(r, l))
                    {
                        inLight = l;
                        break;
                    }
                }

                var lightPos = inLight.position;
                Ray ray = new Ray(wallPos, lightPos - wallPos);

                if (!checker.InAnyLight(ray))
                {
                    shadowMode = false;
                }

                float distance = (Stage.instance.platformZ - ray.origin.z) / ray.direction.z;
                float lightRatio = (lightPos.z - Stage.instance.platformZ) / (lightPos.z - ray.origin.z);
                float cameraRatio = (cameraPos.z - ray.origin.z) / (cameraPos.z - Stage.instance.platformZ);
                Vector3 pos = ray.GetPoint(distance);
                model.transform.position = pos;
                model.transform.localScale = initScale * (lightRatio * cameraRatio);
            }
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
            jumpKeyPressedTime = Time.unscaledTime;
        }
    }

    public float jumpKeyPressedTime;

    public void FixedUpdate()
    {
        var top = checker.HitTop(out float topDist);
        var bottom = checker.HitBottom(out float bottomDist);
        var left = checker.HitLeft(out float leftDist);
        var right = checker.HitRight(out float rightDist);


        var finalV = new Vector3(Input.GetAxis("Horizontal") * speed, rb.linearVelocity.y, 0);

        finalV.y += -9.8f * Time.fixedDeltaTime; //改为手动设置重力，否则即使设置速度y分量为零仍然会向下动

        // if(canJump){
        //     Debug.Log("canjump");
        // }

        if ((shadowGrounded || wallGrounded) && jumpKeyDown)
        {
            finalV.y = jumpSpeed;
            jumpKeyDown = false;
            shadowGrounded = false;
            jumpKeyDown = false;
            StopAllCoroutines();
            animator.SetBool(jumping, true);
            Tools.CallDelayed(() => { animator.SetBool(jumping, false); }, 0.25f);
        }

        if (Time.unscaledTime > jumpKeyPressedTime + 0.1f)
        {
            jumpKeyDown = false;
        }

        if (bottom)
        {
            StopAllCoroutines();
            shadowGrounded = true;
            shadowMode = true;
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

        if (!bottom)
        {
            Tools.CallDelayed(() => { shadowGrounded = false; }, 0.1f);
        }

        // else{
        // 	shadowMode = false;
        // }
        // if (finalV.x > 0){
        // 	sr.flipX = false;
        // }
        // else if (finalV.x < 0){
        // 	sr.flipX = true;
        // }

        if (bottom || wallGrounded)
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