using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTargeter : MonoBehaviour
{

    public enum TARGET_COLOR
    {
        BLACK,
        RED,
        GREEN,
        BLUE,
    }

    public TARGET_COLOR targetColor = TARGET_COLOR.BLACK;
    public Material[] targetColorMaterial;

    private Renderer hintRenderer;
    private int prevColorIndex = -1;

    private void Start()
    {
        hintRenderer.transform.Find("Hint").GetComponent<Renderer>();
    }

    public void TargetingColor()
    {
        int curColorIdx;
        do
        {
            curColorIdx = Random.Range(0, targetColorMaterial.Length);
        }
        while (curColorIdx == prevColorIndex);
        prevColorIndex = curColorIdx;

        targetColor = (TARGET_COLOR)curColorIdx;
        hintRenderer.material = targetColorMaterial[curColorIdx];
    }
}
