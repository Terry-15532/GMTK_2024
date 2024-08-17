using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPos : MonoBehaviour
{
    private static Vector3 startingPos;
    private static bool isDefault = true;
    private static Vector3 defaultPos;
    // public Gravity gravity;
    private static bool flipped;

    public void resetStartingPos()
    {
        isDefault = true;
        startingPos = defaultPos;
    }
    public void setNewStartingPos(Vector3 pos, bool flip)
    {
        isDefault = false;
        startingPos = pos;
        flipped = flip;
    }

    public bool getIsDefault()
    {
        return isDefault;
    }

    public Vector3 getStartingPos()
    {
        return startingPos;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isDefault) {
            transform.position = startingPos;
        } else {
            startingPos = transform.position;
            defaultPos = transform.position;
        }
        if (flipped)
        {
            // gravity.Flip();
        }
    }
}
