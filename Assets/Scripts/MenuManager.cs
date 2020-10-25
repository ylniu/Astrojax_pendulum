using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // 参数设置
    public Button btnSetting;
    // 参数设置窗口
    public GameObject settingPanel;

    // 显示图表
    public Button btnShowChart;
    public GameObject chartsPanel;

    // Start is called before the first frame update
    void Start()
    {
        btnSetting.onClick.AddListener(ShowSettingPanel);
        btnShowChart.onClick.AddListener(ShowChartsPanel);
    }
    void ShowSettingPanel()
    {
        //----------------------------------------------------------------------
        // 报错：GameObject.active is obsolete. 
        // Use GameObject.SetActive(), GameObject.activeSelf
        // or GameObject.activeInHierarchy
        //
        //settingPanel.SetActive(!settingPanel.active);
        settingPanel.SetActive(!settingPanel.activeSelf);
    }

    void ShowChartsPanel()
    {
        //chartsPanel.SetActive(!chartsPanel.active);
        chartsPanel.SetActive(!chartsPanel.activeSelf);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
