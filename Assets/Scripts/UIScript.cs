using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public GameObject topCamera;
    public GameObject sideCamera;
    public Button btnTopCamera;
    public Button btnSideCamera;
    public Button btnStart;
    public Slider yslider;
    public Button buttonJump;
    public BallController ballcontroller;

    // Start is called before the first frame update
    void Start()
    {
        btnTopCamera.onClick.AddListener(TopView);
        btnSideCamera.onClick.AddListener(SideView);
        btnStart.onClick.AddListener(OnStart);
        buttonJump.onClick.AddListener(OnJump);
        yslider.onValueChanged.AddListener(ChangeY);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnStart()
    {
        ballcontroller.Run();
    }

    void TopView()
    {
        topCamera.SetActive(true);
        sideCamera.SetActive(false);
    }

    void SideView()
    {
        topCamera.SetActive(false);
        sideCamera.SetActive(true);
    }

    void ChangeY(float value)
    {
        ballcontroller.ChangeY(value);
    }

    void OnJump()
    {
        //ballcontroller.Jump();
    }
}
