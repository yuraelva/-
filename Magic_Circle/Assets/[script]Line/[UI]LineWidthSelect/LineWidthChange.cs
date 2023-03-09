using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineWidthChange : MonoBehaviour
{
    public DrawLine drawline;
    public Image bold,normal,thin;
    public Sprite boldDef, boldSel,normalDef,normalSel, thinDef, thinSel;
    Vector2 ImagePos;
    private Camera mainCamera;
    bool OnBold = false, OnNormal = false, OnThin = false;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        bold = GetComponent<Image>();
        normal = GetComponent<Image>();
        thin = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1) && OnBold)
        {
            drawline.lineWidth = 1;
            Debug.Log("[LineWidth] Bold");
        }
        if (Input.GetMouseButtonUp(1) && OnNormal)
        {
            drawline.lineWidth = 2;
            Debug.Log("[LineWidth] Normal");
        }
        if (Input.GetMouseButtonUp(1) && OnThin)
        {
            drawline.lineWidth = 3;
            Debug.Log("[LineWidth] Thin");
        }
    }
    public void EnterBold()
    {
        bold.sprite = boldSel;
        OnBold = true;
    }
    public void ExitBold()
    {
        bold.sprite = boldDef;
        OnBold = false;
    }
    public void EnterNormal()
    {
        normal.sprite = normalSel;
        OnNormal = true;
    }
    public void ExitNormal()
    {
        normal.sprite = normalDef;
        OnNormal = false;
    }
    public void EnterThin()
    {
        thin.sprite = thinSel;
        OnThin = true;
    }
    public void ExitThin()
    {
        thin.sprite = thinDef;
        OnThin = false;
    }
}
