using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class calldll : MonoBehaviour
{
    public Text text;

    [DllImport("testdll")] //������ǵ��õ�dll����
    public static extern int Add(int p1, int p2);


    void Start()
    {
        int sum = Add(1, 2);
        text.text = sum + "";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
