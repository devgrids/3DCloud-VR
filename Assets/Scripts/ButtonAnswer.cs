using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAnswer : MonoBehaviour
{
    public Color colorEnter;
    public Color colorExit;

    Image imgButton;

    private void Awake()
    {
        imgButton = GetComponent<Image>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnPointerEnter()
    {
        imgButton.color = colorEnter;
        Debug.Log("Enter");
    }

    public void OnPointerExit()
    {
        imgButton.color = colorExit;
        Debug.Log("Exit");
    }
}
