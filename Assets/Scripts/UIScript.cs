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
    public Button btnExit;
    public Slider yslider;
    public Button buttonJump;
    public BallController ballcontroller;
    public bool isPause=false;

    // Start is called before the first frame update
    void Start()
    {
        btnTopCamera.onClick.AddListener(TopView);
        btnSideCamera.onClick.AddListener(SideView);
        btnStart.onClick.AddListener(OnStart);
        btnExit.onClick.AddListener(OnExit);
        buttonJump.onClick.AddListener(OnJump);
        yslider.onValueChanged.AddListener(ChangeY);
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            yslider.value += (yslider.maxValue - yslider.minValue) * 0.01f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            yslider.value -= (yslider.maxValue - yslider.minValue) * 0.01f;
        }
    }
        // Update is called once per frame
    void Update()
    {
       
    }

    void OnStart()
    {
        Text name = btnStart.transform.GetChild(0).GetComponent<Text>();
        if (name.text=="开始" || name.text=="暂停")
        {
            ballcontroller.RunDynmics();
            name.text = "运行";
            btnStart.targetGraphic.color = Color.green;
        }
        else
        {
            ballcontroller.StopDynmics();
            name.text = "暂停";
            btnStart.targetGraphic.color = Color.red;
        }
    }
    void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
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
