using UnityEngine;
using System;
using UnityEngine.UIElements;
using UnityEngine.InputSystem.XR.Haptics;

[RequireComponent(typeof(Outline))]
public class MovableLamp : MonoBehaviour
{
    [Header("最大Z坐标")] public float maxZ;
    [Header("最小Z坐标")] public float minZ;
    [Header("滚轮移动Z坐标的速度")] public float scrollSpeed;
    [Header("始终显示描边")] public bool showOutlineAlways = true;

    [HideInInspector] public float margin = 0.1f;

    [HideInInspector] public Vector3 initPos;
    [HideInInspector] public bool isDragging;

    [HideInInspector] public Outline outline;
    public Character character;
    public bool mouseOn = false;

    public MeshRenderer mr;

    public static Color blue = new(0, 0.6f, 1), red = new(1, 0.25f, 0);

    public static int colorAttrib = Shader.PropertyToID("_Color");


    public void ResetPos()
    {
        transform.position = initPos;
    }

    public void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
        character = Character.instance;
        initPos = transform.position;
        Stage.instance.resetStage.AddListener(ResetPos);
        Stage.instance.showOutline.AddListener(ShowOutline);

        mr = GetComponentInChildren<MeshRenderer>();
    }

    public void ShowOutline()
    {
        outline.enabled = true;
        Tools.CallDelayed(() => { outline.enabled = false; }, 1);
    }

    public void OnMouseDown()
    {
        if (character.canMoveLight)
        {
            isDragging = !isDragging;
        }
    }

    public void Update()
    {
        if (showOutlineAlways && !outline.enabled)
        {
            outline.enabled = true;
        }
        if (isDragging)
        {
            character.movingLight = true;
            var mousePos = Input.mousePosition;
            mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width * (1 - margin));
            mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height * (1 - margin));
            Ray ray = GameInfo.mainCamera.ScreenPointToRay(mousePos);
            var pos = ray.GetPoint((transform.position - GameInfo.mainCamera.transform.position).magnitude);
            var z = Mathf.Clamp(transform.position.z + Input.GetAxis("Mouse ScrollWheel") * scrollSpeed, minZ, maxZ);
            transform.position = new Vector3(pos.x, pos.y, z);
        }
        else
        {
            character.movingLight = false;
        }

        if (character.canMoveLight)
        {
            mr.materials[0].SetColor(colorAttrib, blue);
        }
        else
        {
            mr.materials[0].SetColor(colorAttrib, red);
        }
    }

    public void OnMouseEnter()
    {
        mouseOn = true;
        if (character.canMoveLight)
        {
            outline.enabled = true;
        }
    }

    public void OnMouseExit()
    {
        mouseOn = false;
        outline.enabled = false;
    }
}