using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickToMoveUI : MonoBehaviour
{
    Vector2 ImagePos;
    private Camera mainCamera;
    public GameObject LineBoldSelect,LineBold, LineNormal, LineThin;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetMouseButtonDown(1) && !Input.GetMouseButton(0)) ImagePos = Input.mousePosition;
        if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
        {
            LineBoldSelect.transform.position = ImagePos;
            LineBold.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            LineNormal.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            LineThin.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);

        }
        if (Input.GetMouseButtonUp(1) && !Input.GetMouseButton(0))
        {
            LineBold.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            LineNormal.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            LineThin.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        }
    }
}
