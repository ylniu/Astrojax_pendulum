﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
//using System.Diagnostics;

public class BallController : MonoBehaviour
{
    // 图表组件
    public SpeedChart chart;
    private int nball;
    private int ndyn;
    private int RunType;
    private float dt;
    private float k;
    private float damping;
    private float ks;
    private float m;
    private float g;
    private float L0;
    private float L1;
    private float L2;
    private float dL;
    private float t;
    private float driver_f;
    private float driver_r;
    private float driver_a;
    private float driver_p;
    private float L;
    private float Fsr;
    private float radius;
    private float cradius;
    private float diameter;
    private float cdiameter;
    private float amp;
    private float force_radius;
    private Vector3[] X;
    private Vector3[] V;
    private Vector3[] F;
    private Vector3 Fs2;
    private Vector3 Fs1;
    private Vector3 Ft2;
    private Vector3 Ft1;
    private Vector3 ZOrigin;
    private Vector3[,] F_pair;
    private Vector3[] a;
    private Vector3 e0;
    private Vector3 e1;
    private Vector3 e;
    private Vector3 dX;
    private Vector3 dX1;
    private Vector3 origin;
    private Vector3 CX0;
    private Vector3 CX1;
    private float[,] Fr;
    private float[,] D;
    private float[] mass;
    private List<GameObject> balls;
    private List<MoveController> ballscripts;
    private List<GameObject> strings;
    private List<MoveString> stringscripts;
    public GameObject Ballpref;
    public GameObject Stringpref;
    private string path;

    // Start is called before the first frame update
    void Start()
    {
        Initiate_params();

        for (int i = 0; i < nball; i++)
        {
            GameObject newBall = Instantiate(Ballpref);
            newBall.transform.localScale = new Vector3(diameter, diameter, diameter);
            MoveController ballscript = newBall.GetComponent<MoveController>();
            balls.Add(newBall.gameObject);
            ballscripts.Add(ballscript);
        }

        for (int i = 0; i < 2; i++)
        {
            GameObject newString = Instantiate(Stringpref);
            newString.transform.localScale = new Vector3(10.0f, 10.0f, 10.0f);
            MoveString stringscript = newString.GetComponent<MoveString>();
            strings.Add(newString.gameObject);
            stringscripts.Add(stringscript);
        }

        Initiate_xv();
        Get_xv();
        //Distance();
        UpdatePos();
    }

    public void Run()
    {
        string cx = Convert.ToString(ballscripts[0].x);
        string appendText = "This is extra text 2  " + cx + Environment.NewLine;
        File.AppendAllText(path, appendText);
        Get_xv();
        cx = Convert.ToString(ballscripts[0].x);
        appendText = "This is extra text 3  " + cx + Environment.NewLine;
        File.AppendAllText(path, appendText);
        Distance();
        cx = Convert.ToString(ballscripts[0].x);
        appendText = "This is extra text 4  " + cx + Environment.NewLine;
        File.AppendAllText(path, appendText);
        //L0 = L;
        Get_bond();

        InvokeRepeating("UpdatePos", 0f, 0.001f);
    }

    void UpdatePos()
    {

        Get_xv();
        Update_xv(nball, X, V, dt, ndyn, 1);
        Set_xv();
        Update_position();
        //---------------------------------------------------------------------
        //
        //FileStream fs = new FileStream("D:\\ak.txt", FileMode.Create);
        //获得字节数组
        //float len = D[0, 1] + D[1, 2];
        //byte[] data = System.Text.Encoding.Default.GetBytes("len = "+len);
        //fs.Write(data, 0, data.Length);
        //byte[] data01 = System.Text.Encoding.Default.GetBytes("X1x = " + X[1].x + "\n");
        //fs.Write(data01, 0, data01.Length);
        //byte[] data = System.Text.Encoding.Default.GetBytes("X1y = " + X[1].y);
        //fs.Write(data, 0, data.Length);
        //byte[] data = System.Text.Encoding.Default.GetBytes("X1z = " + X[1].z);
        //fs.Write(data, 0, data.Length);
        //byte[] data = System.Text.Encoding.Default.GetBytes("V1x = " + V[1].x);
        //fs.Write(data, 0, data.Length);
        //byte[] data = System.Text.Encoding.Default.GetBytes("V1y = " + V[1].y);
        //fs.Write(data, 0, data.Length);
        //byte[] data = System.Text.Encoding.Default.GetBytes("V1z = " + V[1].z);
        //fs.Write(data, 0, data.Length);
        //byte[] data = System.Text.Encoding.Default.GetBytes("X2x = " + X[2].x);
        //fs.Write(data, 0, data.Length);
        //byte[] data = System.Text.Encoding.Default.GetBytes("X2y = " + X[2].y);
        //fs.Write(data, 0, data.Length);
        //byte[] data = System.Text.Encoding.Default.GetBytes("X2z = " + X[2].z);
        //fs.Write(data, 0, data.Length);
        //byte[] data = System.Text.Encoding.Default.GetBytes("V2x = " + V[2].x);
        //fs.Write(data, 0, data.Length);
        //byte[] data = System.Text.Encoding.Default.GetBytes("V2y = " + V[2].y);
        //fs.Write(data, 0, data.Length);
        //byte[] data = System.Text.Encoding.Default.GetBytes("V2z = " + V[2].z);
        //fs.Write(data, 0, data.Length);
        //清空缓冲区、关闭流
        //fs.Flush();
        //fs.Close();
        //
        //---------------------------------------------------------------------
        // 更新图表
        chart.AddData("",V[2].x);

        if (Input.GetKey(KeyCode.Escape))
        {
            Screen.fullScreen = false;  //退出全屏         
        }
        //按A全屏
        if (Input.GetKey(KeyCode.A))
        {
            Screen.SetResolution(1920, 1080, true);
            Screen.fullScreen = true;  //设置成全屏
        }

    }
    //-------------------------------------------------------------------------
    public void ChangeSomething(string key, string value)
    {
        origin = new Vector3(0.0f, 0.0f, 0.0f);
        if (key=="RunType")
        {
            RunType=int.Parse(value);
        }
        else if(key == "Damping")
        {
            damping = float.Parse(value);
        }
        else if (key == "L0")
        {
            L0 = float.Parse(value);
        }
        else if (key == "g")
        {
            g = float.Parse(value);
        }
        else if (key == "ks")
        {
            ks = float.Parse(value);
        }
        else if (key == "dt")
        {
            dt = float.Parse(value);
        }
        else if (key == "driverF")
        {
            driver_f = float.Parse(value);
        }
        else if (key == "M0")
        {
            mass[0] = float.Parse(value);
        }
        else if (key == "M1")
        {
            mass[1] = float.Parse(value);
        }
        else if (key == "M2")
        {
            mass[2] = float.Parse(value);
        }
        else if (key == "X0x")
        {
            ballscripts[0].x.x = float.Parse(value) + origin.x;
        }
        else if (key == "X0y")
        {
            ballscripts[0].x.y = float.Parse(value) + origin.y;
        }
        else if (key == "X0z")
        {
            ballscripts[0].x.z = float.Parse(value) + origin.z;
        }
        else if (key == "X1x")
        {
            ballscripts[1].x.x = float.Parse(value) + origin.x;
        }
        else if (key == "X1y")
        {
            ballscripts[1].x.y = float.Parse(value) + origin.y;
        }
        else if (key == "X1z")
        {
            ballscripts[1].x.z = float.Parse(value) + origin.z;
        }
        else if (key == "X2x")
        {
            ballscripts[2].x.x = float.Parse(value) + origin.x;
        }
        else if (key == "X2y")
        {
            ballscripts[2].x.y = float.Parse(value) + origin.y;
        }
        else if (key == "X2z")
        {
            ballscripts[2].x.z = float.Parse(value) + origin.z;
        }
        else if (key == "V0x")
        {
            ballscripts[0].v.x = float.Parse(value);
        }
        else if (key == "V0y")
        {
            ballscripts[0].v.y = float.Parse(value);
        }
        else if (key == "V0z")
        {
            ballscripts[0].v.z = float.Parse(value);
        }
        else if (key == "V1x")
        {
            ballscripts[1].v.x = float.Parse(value);
        }
        else if (key == "V1y")
        {
            ballscripts[1].v.y = float.Parse(value);
        }
        else if (key == "V1z")
        {
            ballscripts[1].v.z = float.Parse(value);
        }
        else if (key == "V2x")
        {
            ballscripts[2].v.x = float.Parse(value);
        }
        else if (key == "V2y")
        {
            ballscripts[2].v.y = float.Parse(value);
        }
        else if (key == "V2z")
        {
            ballscripts[2].v.z = float.Parse(value);
        }
        
        Get_xv();
        Distance();
        Get_bond();
        Update_position();
        string cx = Convert.ToString(ballscripts[0].x);
        string appendText = "This is extra text 1  " + cx + Environment.NewLine;
        File.AppendAllText(path, appendText);
        //if (strDamping != "") damping = float.Parse(strDamping);
        //if (strL0      != "") L0      = float.Parse(strL0     );
    }
    //-------------------------------------------------------------------------
    public void ChangeY(float value)
    {
        ballscripts[0].x.y = value * 0.1f;
    }
    public void ChangeXZ(float xratio, float yratio)
    {
        float x0 = -0.2f;
        float z0 = -0.2f;
        float wid = 0.4f;
        float hei = 0.4f;
        ballscripts[0].x.x = x0 + wid * xratio;
        ballscripts[0].x.z = z0 + hei * yratio;
    }
    

    public void Jump(Vector3 pos)
    {
        //X[0].y += 5;
        ballscripts[0].x.y = pos.y * 0.1f;

    }

    //--------------------------------------------------------------------------
    void Initiate_params()
    {
        //----------------------------------------------------------------------
        // Initiation
        //
        nball      = 3;
        ndyn       = 20;
        RunType    = 0;
        dt         = 0.0001f;
        radius     = 0.01f;
        cradius    = radius / 5.0f;
        diameter   = radius * 2.0f;
        cdiameter  = cradius * 2.0f;
        damping    = 0.005f;
        //driver_f = 0.1f;
        driver_f   = 1.0f;
        //driver_r = 2.0f;
        driver_p   = 0.0f;
        //amp = 0.1f;
        t = 0.0f;
        //damping = 0.000f;
        //driver_f = 0.0f;
        //driver_r = 0.0f;
        k = 280.0f;
        ks = 280.0f;
        m = 31.88f;
        m = m / 1000.0f;
        g = 9.8f;
        force_radius = 2.0f * radius;
        L0 = 0.61f;
        //
        balls         = new List<GameObject>();
        ballscripts   = new List<MoveController>();
        strings       = new List<GameObject>();
        stringscripts = new List<MoveString>();

        e       = new Vector3();
        origin  = new Vector3();
        dX      = new Vector3();
        dX1     = new Vector3();
        e0      = new Vector3();
        e1      = new Vector3();
        CX0     = new Vector3();
        CX1     = new Vector3();
        Fs2     = new Vector3();
        Fs1     = new Vector3();
        Ft2     = new Vector3();
        Ft1     = new Vector3();
        ZOrigin = new Vector3();
        X = new Vector3[nball];
        V = new Vector3[nball];
        a = new Vector3[nball];
        F = new Vector3[nball];
        F_pair = new Vector3[nball, nball];
        D      = new float  [nball, nball];
        Fr     = new float  [nball, nball];
        mass   = new float  [nball];


        path = Application.dataPath + "/log.txt";

        // This text is added only once to the file.
        if (!File.Exists(path))
        {
            // Create a file to write to.
            string createText = "Hello and Welcome" + Environment.NewLine;
            File.WriteAllText(path, createText);
        }

        // This text is always added, making the file longer over time
        // if it is not deleted.
        string appendText = "This is extra text" + Environment.NewLine;
        File.AppendAllText(path, appendText);

    }
    void Initiate_xv()
    {
        //----------------------------------------------------------------------
        origin = new Vector3(0.0f, 0.0f, 0.0f);
        //----------------------------------------------------------------------
        ballscripts[0].x = new Vector3(0.0f + driver_r, 0.0f, 0.0f);
        ballscripts[0].v = new Vector3(0.0f, 0.0f, 0.0f);
        //----------------------------------------------------------------------
        ballscripts[1].x = new Vector3(0.0f, -0.3f, 0.0f);
        ballscripts[1].v = new Vector3(-1.0f, -1.0f, 0.0f);
        //----------------------------------------------------------------------
        ballscripts[2].x = new Vector3(0.0f, -0.61f, 0.0f);
        ballscripts[2].v = new Vector3(3.0f, 0.0f, 0.0f);
        ////----------------------------------------------------------------------
    }
    void Get_xv()
    {
        for (int i = 0; i < nball; i++)
        {
            X[i] = ballscripts[i].x;
            V[i] = ballscripts[i].v;
        }
    }
    void Set_xv()
    {
        for (int i = 0; i < nball; i++)
        {
            ballscripts[i].x = X[i];
            ballscripts[i].v = V[i];
        }
    }
    void Update_position()
    {
        for (int i = 0; i < nball; i++)
        {
            balls[i].transform.position = ballscripts[i].x;
        }
    }
    void Distance()
    {
        for (int i = 0; i < nball; i++)
        {
            for (int j = i+1; j < nball; j++)
            {
                if (i == j)
                {
                    D[i, j] = 0.0f;
                }
                else
                {
                    D[i, j] = (X[i] - X[j]).magnitude;
                    D[j, i] = D[i, j];
                }
            }
        }
        L1 = D[0, 1];
        L2 = D[1, 2];
        L = L1 + L2;
    }
    void Get_bond()
    {
        //----------------------------------------------------------------------
        strings[0].transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        strings[1].transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        strings[0].transform.localScale = new Vector3(cdiameter, L1/2.0f, cdiameter);
        strings[1].transform.localScale = new Vector3(cdiameter, L2/2.0f, cdiameter);
        //----------------------------------------------------------------------
        CX0 = (X[0]+X[1])/2.0f;
        CX1 = (X[1]+X[2])/2.0f;
        //----------------------------------------------------------------------
        strings[0].transform.position = CX0;
        strings[1].transform.position = CX1;
        //----------------------------------------------------------------------
        e = new Vector3(0.0f, 1.0f, 0.0f);
        e0 = (X[1] - X[0]) / (X[1] - X[0]).magnitude;
        e1 = (X[2] - X[1]) / (X[2] - X[1]).magnitude;
        strings[0].transform.rotation = Quaternion.FromToRotation(e, e0);
        strings[1].transform.rotation = Quaternion.FromToRotation(e, e1);
        //----------------------------------------------------------------------
    }
    void Cal_force_pair()
    {
        for (int i = 0; i < nball - 1; i++)
        {
            for (int j = i + 1; j < nball; j++)
            {
                if (D[i, j] < force_radius)
                {
                    float dr = force_radius - D[i, j];
                    Fr[i, j] = k * dr;
                }
                else
                {
                    Fr[i, j] = 0.0f;
                }
                e = (X[i] - X[j]) / D[i, j];
                // The force act on i
                F_pair[i, j] = e * Fr[i, j];
                // The force act on j
                F_pair[j, i] = -F_pair[i, j];
            }
        }
        //----------------------------------------------------------------------
        dL = L - L0;
        if (dL>0.0f)
        {
            Fsr = ks * dL;
        }
        else
        {
            Fsr = 0.0f;
        }
        //----------------------------------------------------------------------
        e = (X[1] - X[2]) / L2;
        Fs2 = e * Fsr;
        Ft2 = -Fs2;
        //----------------------------------------------------------------------
        e = (X[0] - X[1]) / L1; 
        Ft1 = e * Fsr;
        Fs1 = Ft2 + Ft1;
        //----------------------------------------------------------------------
        Fs1 = Fs1 - V[1] * damping;
        Fs2 = Fs2 - V[2] * damping;
        //----------------------------------------------------------------------
    }
    void Cal_force()
    {
        Distance();
        Cal_force_pair();
        //----------------------------------------------------------------------
        for (int i = 0; i < nball; i++)
        {
            F[i] = new Vector3(0.0f, 0.0f, 0.0f);
            for (int j = 0; j < nball; j++)
            {
                if (j != i && D[i, j] < force_radius)
                {
                    F[i] = F[i] + F_pair[i, j];
                }
            }
            //------------------------------------------------------------------
            F[i].y = F[i].y - mass[i] * g;
            //------------------------------------------------------------------
        }
        //----------------------------------------------------------------------
        F[1] = F[1] + Fs1;
        F[2] = F[2] + Fs2;
        //----------------------------------------------------------------------

        for (int i = 0; i < nball; i++)
        {
            a[i] = F[i] / mass[i];
        }
        a[0] = new Vector3(0.0f, 0.0f, 0.0f);
    }
    void Update_xv(int N, Vector3[] X, Vector3[] V, float dt, int ndyn, int itype)
    {
        //----------------------------------------------------------------------
        // C# 函数2 (传值与传址)
        // https://www.cnblogs.com/mdnx/archive/2012/09/04/2671060.html
        //
        for (int idyn=1; idyn<ndyn; idyn++)
        {
            Cal_force();
            //----------------------------------------------------------------------
            for (int i = 0; i < N; i++)
            {
                V[i].x = V[i].x + a[i].x * dt / 2.0f;
                V[i].y = V[i].y + a[i].y * dt / 2.0f;
                V[i].z = V[i].z + a[i].z * dt / 2.0f;
            }
            //----------------------------------------------------------------------
            for (int i = 0; i < N; i++)
            {
                t = t + dt;
                //string cx = Convert.ToString(X[0].x);
                //string appendText = "This is extra text "+cx + Environment.NewLine;
                //File.AppendAllText(path, appendText);
                if (i == 0)
                {
                    if (RunType==1 && driver_f > 0.0f) {
                        // driver_a = driver_f * dt;
                        driver_a = driver_f * t;
                        
                        dX        = new Vector3(0.0f, 0.0f, 0.0f);
                        dX1       = new Vector3(0.0f, 0.0f, 0.0f);
                        ZOrigin   = new Vector3(0.0f, X[0].y, 0.0f);
                        //----------------------------------------------------------
                        // move around y axis
                        //
                        dX = X[i] - ZOrigin;
                        
                        dX1.x = (float)Math.Cos(driver_a) * dX.x - (float)Math.Sin(driver_a) * dX.z;
                        dX1.z = (float)Math.Sin(driver_a) * dX.x + (float)Math.Cos(driver_a) * dX.z;
                        dX1.y = dX.y;
                        
                        //----------------------------------------------------------
                        // move along y axis
                        //
                        // dX.y = amp * (float)Math.Cos(driver_a + driver_p);
                        // dX.y = amp * (float)Math.Cos(driver_a);
                        //----------------------------------------------------------
                        X[i] = ZOrigin + dX1;
                        
                    }
                }
                else
                {
                    X[i].x = X[i].x + V[i].x * dt;
                    X[i].y = X[i].y + V[i].y * dt;
                    X[i].z = X[i].z + V[i].z * dt;
                }
            }
            //----------------------------------------------------------------------
            Cal_force();
            //----------------------------------------------------------------------
            for (int i = 0; i < N; i++)
            {
                V[i].x = V[i].x + a[i].x * dt / 2.0f;
                V[i].y = V[i].y + a[i].y * dt / 2.0f;
                V[i].z = V[i].z + a[i].z * dt / 2.0f;
                //------------------------------------------------------------------
            }
            Get_bond();
        }
     }

    // Update is called once per frame
    void Update()
    {
    }
}