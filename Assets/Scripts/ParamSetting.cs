using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ParamSetting : MonoBehaviour
{
    public BallController ballcontroller;
    private ScanR2 scanR2 = new ScanR2();
    public Button btnSubmit;
    public Button btnReset;
    public Button btnLoad;
    public Button btnScanR2;

    //-------------------------------------------------------------------------
    public InputField RunType;
    public InputField Damping;
    public InputField L0;
    public InputField R2;
    public InputField G;
    public InputField ks;
    public InputField dt;
    public InputField driverF;
    public InputField ChooseRoot;
    public InputField M0;
    public InputField M1;
    public InputField M2;
    public InputField X0x;
    public InputField X0y;
    public InputField X0z;
    public InputField V0x;
    public InputField V0y;
    public InputField V0z;
    public InputField X1x;
    public InputField X1y;
    public InputField X1z;
    public InputField V1x;
    public InputField V1y;
    public InputField V1z;
    public InputField X2x;
    public InputField X2y;
    public InputField X2z;
    public InputField V2x;
    public InputField V2y;
    public InputField V2z;
    //-------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        btnSubmit.onClick.AddListener(ParamSubmit);
        btnReset.onClick.AddListener(OnReset);
        btnLoad.onClick.AddListener(OnLoad);
        btnScanR2.onClick.AddListener(OnScanR2);
        OnReset();
    }

    void OnReset()
    {
        // Damping factor
        RunType.text = "0";
        Damping.text = "0.005";
        L0.text = "0.61";
        R2.text = "0.14335";
        G.text = "9.8";
        ks.text = "280.0";
        dt.text = "0.00001";
        driverF.text = "14.10";
        ChooseRoot.text = "1";
        M0.text = "0.03188";
        M1.text = "0.03188";
        M2.text = "0.03188";
        X0x.text = "0.0";
        X0y.text = "0.0";
        X0z.text = "0.0";
        V0x.text = "0.0";
        V0y.text = "0.0";
        V0z.text = "0.0";
        X1x.text = "0.0";
        X1y.text = "-0.3";
        X1z.text = "0.0";
        V1x.text = "-1.0";
        V1y.text = "-1.0";
        V1z.text = "0.0";
        X2x.text = "0.0";
        X2y.text = "-0.61";
        X2z.text = "0.0";
        V2x.text = "3.0";
        V2y.text = "0.0";
        V2z.text = "0.0";
    }

    private void OnScanR2()
    {
        int in_CRoot = int.Parse(ChooseRoot.text);
        double in_r2    = double.Parse(R2.text);
        double in_eta   = double.Parse(Damping.text);
        double in_M1    = double.Parse(M1.text);
        double in_M2    = double.Parse(M2.text);
        double in_w0    = double.Parse(driverF.text);
        double in_g     = double.Parse(G.text);
        double out_L0   = double.Parse(L0.text);
        double in_ks    = double.Parse(ks.text);

        double[] X0 = new double[3];
        double[] X1 = new double[3];
        double[] X2 = new double[3];
        double[] V0 = new double[3];
        double[] V1 = new double[3];
        double[] V2 = new double[3];

        scanR2.Calc(in_CRoot, in_r2, in_eta, in_M1, in_M2, in_w0, in_g, in_ks,
            ref out_L0, ref X0,ref X1, ref X2, ref V0, ref V1, ref V2);

        // 返回值写到输入框
        // X0x.text = x0.x + "";
        X0x.text = X0[0].ToString("f6");
        X0y.text = X0[1].ToString("f6");
        X0z.text = X0[2].ToString("f6");
        X1x.text = X1[0].ToString("f6");
        X1y.text = X1[1].ToString("f6");
        X1z.text = X1[2].ToString("f6");
        X2x.text = X2[0].ToString("f6");
        X2y.text = X2[1].ToString("f6");
        X2z.text = X2[2].ToString("f6");
        V0x.text = V0[0].ToString("f6");
        V0y.text = V0[1].ToString("f6");
        V0z.text = V0[2].ToString("f6");
        V1x.text = V1[0].ToString("f6");
        V1y.text = V1[1].ToString("f6");
        V1z.text = V1[2].ToString("f6");
        V2x.text = V2[0].ToString("f6");
        V2y.text = V2[1].ToString("f6");
        V2z.text = V2[2].ToString("f6");
        L0.text  = out_L0.ToString("f6");
    }
    private void OnLoad()
    {
        // string datafile = Application.dataPath + "/files/data.txt";


        //string datafile = "D:/Share/Data/GitHub/Astrojax_pendulum/Assets/files/data.txt";
        //if (!File.Exists(datafile))
        //{
        //    Debug.Log("find not found");
        //    return;
        //}
        //string[] fileText2Content = File.ReadAllLines(datafile);
        //foreach (string str in fileText2Content)
        //{
        //    Parseline(str);
        //}
        OnReset();
    }

    public void ParseFile(string filepath)
    {
        if (!File.Exists(filepath))
        {
            Debug.Log("find not found");
            return;
        }
        Debug.Log("parse file " + filepath);
        string[] fileText2Content = File.ReadAllLines(filepath);
        foreach (string str in fileText2Content)
        {
            Parseline(str);
        }
    }

    void Parseline(string line)
    {
        string[] tokens = line.Split('=');
        if (tokens.Length == 2)
        {
            string key = tokens[0].Trim();
            string value = tokens[1].Trim();
             
            if (key == "RunType")
            {
                RunType.text = value;
            }
            else if (key == "Damping")
            {
                Damping.text = value;
            }
            else if (key == "L0")
            {
                L0.text = value;
            }
            else if (key == "R2")
            {
                R2.text = value;
            }
            else if (key == "g")
            {
                G.text = value;
            }
            else if (key == "ks")
            {
                ks.text = value;
            }
            else if (key == "dt")
            {
                dt.text = value;
            }
            else if (key == "driverF")
            {
                driverF.text = value;
            }
            else if (key == "ChooseRoot")
            {
                ChooseRoot.text = value;
            }
            else if (key == "M0")
            {
                M0.text = value;
            }
            else if (key == "M1")
            {
                M1.text = value;
            }
            else if (key == "M2")
            {
                M2.text = value;
            }
            else if (key == "X0x")
            {
                X0x.text = value;
            }
            else if (key == "X0y")
            {
                X0y.text = value;
            }
            else if (key == "X0z")
            {
                X0z.text = value;
            }
            else if (key == "V0x")
            {
                V0x.text = value;
            }
            else if (key == "V0y")
            {
                V0y.text = value;
            }
            else if (key == "V0z")
            {
                V0z.text = value;
            }
            else if (key == "X1x")
            {
                X1x.text = value;
            }
            else if (key == "X1y")
            {
                X1y.text = value;
            }
            else if (key == "X1z")
            {
                X1z.text = value;
            }
            else if (key == "V1x")
            {
                V1x.text = value;
            }
            else if (key == "V1y")
            {
                V1y.text = value;
            }
            else if (key == "V1z")
            {
                V1z.text = value;
            }
            else if (key == "X2x")
            {
                X2x.text = value;
            }
            else if (key == "X2y")
            {
                X2y.text = value;
            }
            else if (key == "X2z")
            {
                X2z.text = value;
            }
            else if (key == "V2x")
            {
                V2x.text = value;
            }
            else if (key == "V2y")
            {
                V2y.text = value;
            }
            else if (key == "V2z")
            {
                V2z.text = value;
            }
        }
    }
    public string GetParam(string key)
    {
        if (key == "RunType")
        {
            return RunType.text;
        }
        else
        {
            return "Null";
        }
    }
    void ParamSubmit()
    {
        string strRunType = RunType.text;
        string strDamping = Damping.text;
        string strL0 = L0.text;
        string strG = G.text;
        string strks = ks.text;
        string strdt = dt.text;
        string strdriverF = driverF.text;
        string strM0 = M0.text;
        string strM1 = M1.text;
        string strM2 = M2.text;
        string strX0x = X0x.text;
        string strX0y = X0y.text;
        string strX0z = X0z.text;
        string strV0x = V0x.text;
        string strV0y = V0y.text;
        string strV0z = V0z.text;
        string strX1x = X1x.text;
        string strX1y = X1y.text;
        string strX1z = X1z.text;
        string strV1x = V1x.text;
        string strV1y = V1y.text;
        string strV1z = V1z.text;
        string strX2x = X2x.text;
        string strX2y = X2y.text;
        string strX2z = X2z.text;
        string strV2x = V2x.text;
        string strV2y = V2y.text;
        string strV2z = V2z.text;
        //ballcontroller.ChangeSomething1();
        ballcontroller.ChangeSomething("RunType", strRunType);
        ballcontroller.ChangeSomething("Damping", strDamping);
        ballcontroller.ChangeSomething("L0", strL0);
        ballcontroller.ChangeSomething("g", strG);
        ballcontroller.ChangeSomething("ks", strks);
        ballcontroller.ChangeSomething("dt", strdt);
        ballcontroller.ChangeSomething("driverF", strdriverF);
        ballcontroller.ChangeSomething("M0", strM0);
        ballcontroller.ChangeSomething("M1", strM1);
        ballcontroller.ChangeSomething("M2", strM2);
        ballcontroller.ChangeSomething("X0x", strX0x);
        ballcontroller.ChangeSomething("X0y", strX0y);
        ballcontroller.ChangeSomething("X0z", strX0z);
        ballcontroller.ChangeSomething("V0x", strV0x);
        ballcontroller.ChangeSomething("V0y", strV0y);
        ballcontroller.ChangeSomething("V0z", strV0z);
        ballcontroller.ChangeSomething("X1x", strX1x);
        ballcontroller.ChangeSomething("X1y", strX1y);
        ballcontroller.ChangeSomething("X1z", strX1z);
        ballcontroller.ChangeSomething("V1x", strV1x);
        ballcontroller.ChangeSomething("V1y", strV1y);
        ballcontroller.ChangeSomething("V1z", strV1z);
        ballcontroller.ChangeSomething("X2x", strX2x);
        ballcontroller.ChangeSomething("X2y", strX2y);
        ballcontroller.ChangeSomething("X2z", strX2z);
        ballcontroller.ChangeSomething("V2x", strV2x);
        ballcontroller.ChangeSomething("V2y", strV2y);
        ballcontroller.ChangeSomething("V2z", strV2z);
        ballcontroller.UpdataParams();
    }
    // Update is called once per frame
    void Update()
    {
    }
}