using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Rendering.HybridV2;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;

public class ScanR2 : MonoBehaviour
{
    private double[] FF2;
    private double[] FF1;
    private double dL;
    private double L2;
    private int nL;
    private int CR;
    private int IsDebug;
    //--------------------------------------------------------------------------
    private double R0;
    private double R1;
    private double R2;
    private double eta;
    private double M1;
    private double M2;
    private double w0;
    private double g;
    private double LL;
    private double L0;
    private double L1;
    private double dFr;
    private double dFr_min;
    private double L2_min;
    private double ks;
    //--------------------------------------------------------------------------
    private double cosf2;
    private double phi2;
    private double cosf1;
    private double phi1;
    private double AC;
    private double AB;
    private double H1;
    private double H2;
    private double beta1;
    private double beta2;
    private double alpha1;
    private double cosb2;
    private double cosa1;
    private double T;
    private double FP;
    private double P01;
    private double PI;
    private double[] T01;
    private double[] T21;
    private double[] T12;
    private double eps;
    //--------------------------------------------------------------------------
    private double[] X0;
    private double[] X1;
    private double[] X2;
    private double[] V0;
    private double[] V1;
    private double[] V2;
    private double[] L2i;
    private string path;
    private string appendText;
    //--------------------------------------------------------------------------
    private bool find;
    private bool[] find1;
    private bool[] find1_min;
    //--------------------------------------------------------------------------

    public void Calc(
        //----------------------------------------------------------------------
        int in_CRoot,
        double in_R2,
        double in_eta,
        double in_M1,
        double in_M2,
        double in_w0,
        double in_g,
        double in_ks,
        //----------------------------------------------------------------------
        ref double   outL0,
        ref double[] outX0,
        ref double[] outX1,
        ref double[] outX2,
        ref double[] outV0,
        ref double[] outV1,
        ref double[] outV2
        )
    {
        //----------------------------------------------------------------------
        X0 = new double[3];
        X1 = new double[3];
        X2 = new double[3];
        V0 = new double[3];
        V1 = new double[3];
        V2 = new double[3];
        //----------------------------------------------------------------------
        T01 = new double[3];
        T21 = new double[3];
        T12 = new double[3];
        FF1 = new double[3];
        FF2 = new double[3];
        L2i = new double[2];
        find1 = new bool[2];
        find1_min = new bool[2];
        //----------------------------------------------------------------------
        nL = 100000;
        double dFr =0.0;
        bool find=false;
        //----------------------------------------------------------------------
        R2  = in_R2;
        eta = in_eta;
        M1  = in_M1;
        M2  = in_M2;
        w0  = in_w0;
        g   = in_g;
        LL  = outL0;
        CR  = in_CRoot;
        ks  = in_ks;
        PI  = System.Math.PI;
        //----------------------------------------------------------------------
        FF2[0] = -M2  * w0*w0 * R2;
        FF2[1] = -eta * w0 * R2;
        FF2[2] = -M2  * g;
        dFr_min = 100.0;
        eps = 0.05;
        //----------------------------------------------------------------------
        dL = LL / nL;
        //======================================================================
        IsDebug = 1;
        if (IsDebug == 1)
        {
            path = Application.dataPath + "/Debug_ScanR2.txt";
            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                string createText = Environment.NewLine;
                File.WriteAllText(path, createText);
            }
            File.WriteAllText(path, "");
        }
        //======================================================================
        //======================================================================
        if (IsDebug == 1)
        {
            string cLL = Convert.ToString(LL);
            appendText = "L0 = " + cLL + Environment.NewLine;
            File.AppendAllText(path, appendText);
        }
        //======================================================================
        //----------------------------------------------------------------------
        for (int i=1; i<=nL; i++)
        {
            L2 = i * dL;
            reverse_search2_only(ref dFr, ref find, ref find1);
            if (find && dFr_min > dFr)
            {
                dFr_min = dFr;
                L2_min = L2;
                find1_min[0] = find1[0];
                find1_min[1] = find1[1];
                if (find1[0]) L2_min = L2i[0];
                if (find1[1]) L2_min = L2i[1];
            }
        }
        //----------------------------------------------------------------------
        L2 = L2_min;
        //======================================================================
        if (IsDebug == 1)
        {
            string cfind0 = Convert.ToString(find1_min[0]);
            appendText = "find0 = " + cfind0 + Environment.NewLine;
            File.AppendAllText(path, appendText);
            string cfind1 = Convert.ToString(find1_min[1]);
            appendText = "find1 = " + cfind1 + Environment.NewLine;
            File.AppendAllText(path, appendText);
            string cL2 = Convert.ToString(L2);
            appendText = "L2 = " + cL2 + Environment.NewLine;
            File.AppendAllText(path, appendText);
        }
        //======================================================================
        //----------------------------------------------------------------------
        Reverse_search2();
        //----------------------------------------------------------------------
        outX0 = X0;
        outX1 = X1;
        outX2 = X2;
        outV0 = V0;
        outV1 = V1;
        outV2 = V2;
        //----------------------------------------------------------------------
    }
    void reverse_search2_only(ref double dFr, ref bool find, ref bool[] find1)
    {
        double[] root = new double[4];
        bool[] ifroot = new bool[4];
        double l = L2 / R2;
        double sigma = Math.Abs(FF2[1] / FF2[0]);
        double kappa = Math.Abs(FF2[2] / Math.Sqrt(FF2[0] * FF2[0] + FF2[1] * FF2[1]));
        //----------------------------------------------------------------------
        solve_equation2(sigma, kappa, l, ref root, ref ifroot);
        //----------------------------------------------------------------------
        if (!ifroot[0] && !ifroot[1])
        {
            return;
        }
        //----------------------------------------------------------------------
        find = false;
        find1 = new bool[2];
        find1[0] = false;
        find1[1] = false;
        for(int i=0; i<=1; i++)
        {
            if (ifroot[i])
            {
                double r = Math.Sqrt(root[i]);
                double h = kappa * l / Math.Sqrt(1.0 + kappa * kappa);
                cosf2 = (r * r + kappa * kappa * r * r + kappa * kappa - l * l + 1) / (2.0 * r * (1 + kappa * kappa));
                phi2 = Math.Acos(cosf2);
                //--------------------------------------------------------------
                R1 = r * R2;
                H2 = h * R2;
                //--------------------------------------------------------------
                AC = Math.Sqrt(R1 * R1 + R2 * R2 - 2 * R1 * R2 * cosf2);
                //--------------------------------------------------------------
                cosb2 = (R1 * R1 - R2 * R2 - AC * AC) / (2.0 * R2 * AC);
                beta2 = Math.Acos(cosb2);
                beta1 = phi2 + PI - beta2;
                //--------------------------------------------------------------
                T = Math.Sqrt(FF2[0] * FF2[0] + FF2[1] * FF2[1] + FF2[2] * FF2[2]);
                FP = T * AC / L2;
                //--------------------------------------------------------------
                T12[0] = FP * Math.Cos(beta2);
                T12[1] = FP * Math.Cos(beta2 - PI / 2.0);
                T12[2] = T * H2 / L2;
                //--------------------------------------------------------------
                T21[0] = FP * Math.Cos(beta1);
                T21[1] = FP * Math.Cos(beta1 + PI / 2.0);
                T21[2] = -T12[2];
                //--------------------------------------------------------------
                FF1[0] = -M1  * w0 * w0 * R1 - T21[0];
                FF1[1] = -eta * w0 * R1      + T21[1];
                FF1[2] = -M1  * g            + T21[2];
                //--------------------------------------------------------------
                double[] ddF = new double[3];
                ddF[0] = FF2[0] - T12[0];
                ddF[1] = FF2[1] + T12[1];
                ddF[2] = FF2[2] + T12[2];
                double dF   = Math.Sqrt(ddF[0] * ddF[0] + ddF[1] * ddF[1] + ddF[2] * ddF[2]);
                double ddd1 = Math.Sqrt(FF1[0] * FF1[0] + FF1[1] * FF1[1] + FF1[2] * FF1[2]);
                double ddd2 = Math.Sqrt(T12[0] * T12[0] + T12[1] * T12[1] + T12[2] * T12[2]);
                dFr = Math.Abs(ddd1 - ddd2);
                if (dF < eps && dFr < eps)
                {
                    L1 = LL - L2;
                    find = true;
                    find1[i] = true;
                    L2i[i] = L2;
                    //======================================================================
                    if (IsDebug == 1)
                    {
                        string ci = Convert.ToString(i);
                        string cL21 = Convert.ToString(L2);
                        string cdFr = Convert.ToString(dFr);
                        appendText = ci + "  " + cL21 + "  " + cdFr  + Environment.NewLine;
                        File.AppendAllText(path, appendText);
                    }
                    //======================================================================
                }
                //--------------------------------------------------------------
            }
        }
        //----------------------------------------------------------------------
        //----------------------------------------------------------------------
    }
    void Reverse_search2()
    {
        double[] root = new double[4];
        bool[] ifroot = new bool[4];
        double lll = L2 / R2;
        double sigma = Math.Abs(FF2[1] / FF2[0]);
        double kappa = Math.Abs(FF2[2] / Math.Sqrt(FF2[0] * FF2[0] + FF2[1] * FF2[1]));
        //----------------------------------------------------------------------
        solve_equation2(sigma, kappa, lll, ref root, ref ifroot);
        //----------------------------------------------------------------------
        if (!ifroot[0] && !ifroot[1])
        {
            return;
        }
        //----------------------------------------------------------------------
        for (int i = 0; i <= 1; i++)
        {
            if (ifroot[i] && find1_min[i])
            {
                double r = Math.Sqrt(root[i]);
                double h = kappa * lll / Math.Sqrt(1.0 + kappa * kappa);
                cosf2 = (r * r + kappa * kappa * r * r + kappa * kappa - lll * lll + 1) / (2.0 * r * (1 + kappa * kappa));
                phi2 = Math.Acos(cosf2);
                //--------------------------------------------------------------
                R1 = r * R2;
                H2 = h * R2;
                //--------------------------------------------------------------
                AC = Math.Sqrt(R1 * R1 + R2 * R2 - 2 * R1 * R2 * cosf2);
                //--------------------------------------------------------------
                cosb2 = (R1 * R1 - R2 * R2 - AC * AC) / (2.0 * R2 * AC);
                beta2 = Math.Acos(cosb2);
                beta1 = phi2 + PI - beta2;
                //--------------------------------------------------------------
                T = Math.Sqrt(FF2[0] * FF2[0] + FF2[1] * FF2[1] + FF2[2] * FF2[2]);
                FP = T * AC / L2;
                //--------------------------------------------------------------
                T12[0] = FP * Math.Cos(beta2);
                T12[1] = FP * Math.Cos(beta2 - PI / 2.0);
                T12[2] = T * H2 / L2;
                //--------------------------------------------------------------
                T21[0] = FP * Math.Cos(beta1);
                T21[1] = FP * Math.Cos(beta1 + PI / 2.0);
                T21[2] = -T12[2];
                //--------------------------------------------------------------
                double[] ddF = new double[3];
                ddF[0] = FF2[0] - T12[0];
                ddF[1] = FF2[1] + T12[1];
                ddF[2] = FF2[2] + T12[2];
                double dF = Math.Sqrt(ddF[0] * ddF[0] + ddF[1] * ddF[1] + ddF[2] * ddF[2]);

                //======================================================================
                if (IsDebug == 1)
                {
                    string str0 = Convert.ToString(cosb2);
                    appendText = "cosb2 = " + str0 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str1 = Convert.ToString(R2);
                    appendText = "R2 = " + str1 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str2 = Convert.ToString(R1);
                    appendText = "R1 = " + str2 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str3 = Convert.ToString(dF);
                    appendText = "dF = " + str3 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str4 = Convert.ToString(FF2[0]);
                    appendText = "FF2 = " + str4 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str5 = Convert.ToString(FF2[1]);
                    appendText = "FF2 = " + str5 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str6 = Convert.ToString(FF2[2]);
                    appendText = "FF2 = " + str6 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str7 = Convert.ToString(T12[0]);
                    appendText = "T12 = " + str7 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str8 = Convert.ToString(T12[1]);
                    appendText = "T12 = " + str8 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str9 = Convert.ToString(T12[2]);
                    appendText = "T12 = " + str9 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                }
                //======================================================================
                if (dF < eps)
                {
                    FF1[0] = -M1 * w0 * w0 * R1 - T21[0];
                    FF1[1] = -eta * w0 * R1 + T21[1];
                    FF1[2] = -M1 * g + T21[2];
                    L1 = LL - L2;
                    if (IsDebug == 1)
                    {
                        string str4 = Convert.ToString(FF1[0]);
                        appendText = "FF1T21 = " + str4 + Environment.NewLine;
                        File.AppendAllText(path, appendText);
                        string str5 = Convert.ToString(FF1[1]);
                        appendText = "FF2T21 = " + str5 + Environment.NewLine;
                        File.AppendAllText(path, appendText);
                        string str6 = Convert.ToString(FF1[2]);
                        appendText = "FF3T21 = " + str6 + Environment.NewLine;
                        File.AppendAllText(path, appendText);
                    }
                    Reverse_search3();
                }
            }
        }
        //----------------------------------------------------------------------
        //----------------------------------------------------------------------
    }
    void Reverse_search3()
    {
        double[] dFs = new double[2];
        Calc_dF1(ref dFs);
        //----------------------------------------------------------------------
        double[] root = new double[4];
        bool[] ifroot = new bool[4];
        double l = L1 / R1;
        double sigma = Math.Abs(FF1[1] / FF1[0]);
        double kappa = Math.Abs(FF1[2] / Math.Sqrt(FF1[0] * FF1[0] + FF1[1] * FF1[1]));
        //----------------------------------------------------------------------
        solve_equation2(sigma, kappa, l, ref root, ref ifroot);
        //----------------------------------------------------------------------
        if (!ifroot[0] && !ifroot[1])
        {
            return;
        }
        //----------------------------------------------------------------------
        int i_root;
        if (ifroot[0] && !ifroot[1])
        {
            i_root = 0;
        }
        else if (!ifroot[0] && ifroot[1])
        {
            i_root = 1;
        }
        else if (ifroot[0] && ifroot[1] && CR == 1)
        {
            i_root = 0;
        }
        else if (ifroot[0] && ifroot[1] && CR == 2)
        {
            i_root = 1;
        }
        else
        {
            return;
        }
        for(int i=0; i<=1; i++)
        {
            if (dFs[i] < eps)
            {
                double r = Math.Sqrt(root[i]);
                double h = kappa * l / Math.Sqrt(1.0 + kappa * kappa);
                cosf1 = (r * r + kappa * kappa * r * r + kappa * kappa - l * l + 1) / (2.0 * r * (1 + kappa * kappa));
                phi1 = Math.Acos(cosf1);
                //--------------------------------------------------------------
                R0 = r * R1;
                H1 = h * R1;
                //--------------------------------------------------------------
                AB = Math.Sqrt(R0 * R0 + R1 * R1 - 2 * R0 * R1 * cosf1);
                //--------------------------------------------------------------
                cosa1 = (R0 * R0 - R1 * R1 - AB * AB) / (2.0 * R1 * AB);
                alpha1 = Math.Acos(cosa1);
                //--------------------------------------------------------------
                T = Math.Sqrt(FF1[0] * FF1[0] + FF1[1] * FF1[1] + FF1[2] * FF1[2]);
                P01 = T * AB / L1;
                //--------------------------------------------------------------
                T01[0] = P01 * Math.Cos(alpha1);
                T01[1] = P01 * Math.Cos(alpha1 - PI / 2.0);
                T01[2] = T * H1 / L1;
                //--------------------------------------------------------------
                double[] ddF = new double[3];
                ddF[0] = FF1[0] - T01[0];
                ddF[1] = FF1[1] + T01[1];
                ddF[2] = FF1[2] + T01[2];
                double dF = Math.Sqrt(ddF[0] * ddF[0] + ddF[1] * ddF[1] + ddF[2] * ddF[2]);
                //======================================================================
                if (IsDebug == 1)
                {
                    string cL2 = Convert.ToString(L2);
                    appendText = "L2 = " + cL2 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string cdF = Convert.ToString(dF);
                    appendText = "dF = " + cdF + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string ceps = Convert.ToString(eps);
                    appendText = "eps= " + ceps + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str1 = Convert.ToString(FF1[0]);
                    appendText = "F1r = " + str1 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str2 = Convert.ToString(FF1[1]);
                    appendText = "F1p = " + str2 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str3 = Convert.ToString(FF1[2]);
                    appendText = "F1z = " + str3 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str4 = Convert.ToString(T01[0]);
                    appendText = "T01 = " + str4 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str5 = Convert.ToString(T01[1]);
                    appendText = "T01 = " + str5 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                    string str6 = Convert.ToString(T01[2]);
                    appendText = "T01 = " + str6 + Environment.NewLine;
                    File.AppendAllText(path, appendText);
                }
                //======================================================================
                if (dF < eps)
                {
                    Output_xyz();
                }
                //--------------------------------------------------------------
            }
        }
    }
    void Calc_dF1(ref double [] dFs)
    {
        dFs = new double[2];
        double[] root = new double[4];
        bool[] ifroot = new bool[4];
        double l = L1 / R1;
        double sigma = Math.Abs(FF1[1] / FF1[0]);
        double kappa = Math.Abs(FF1[2] / Math.Sqrt(FF1[0] * FF1[0] + FF1[1] * FF1[1]));
        //----------------------------------------------------------------------
        solve_equation2(sigma, kappa, l, ref root, ref ifroot);
        //----------------------------------------------------------------------
        for (int i = 0; i <= 1; i++)
        {
            double r = Math.Sqrt(root[i]);
            double h = kappa * l / Math.Sqrt(1.0 + kappa * kappa);
            cosf1 = (r * r + kappa * kappa * r * r + kappa * kappa - l * l + 1) / (2.0 * r * (1 + kappa * kappa));
            phi1 = Math.Acos(cosf1);
            //--------------------------------------------------------------
            R0 = r * R1;
            H1 = h * R1;
            //--------------------------------------------------------------
            AB = Math.Sqrt(R0 * R0 + R1 * R1 - 2 * R0 * R1 * cosf1);
            //--------------------------------------------------------------
            cosa1 = (R0 * R0 - R1 * R1 - AB * AB) / (2.0 * R1 * AB);
            alpha1 = Math.Acos(cosa1);
            //--------------------------------------------------------------
            T = Math.Sqrt(FF1[0] * FF1[0] + FF1[1] * FF1[1] + FF1[2] * FF1[2]);
            P01 = T * AB / L1;
            //--------------------------------------------------------------
            T01[0] = P01 * Math.Cos(alpha1);
            T01[1] = P01 * Math.Cos(alpha1 - PI / 2.0);
            T01[2] = T * H1 / L1;
            //--------------------------------------------------------------
            double[] ddF = new double[3];
            ddF[0] = FF1[0] - T01[0];
            ddF[1] = FF1[1] + T01[1];
            ddF[2] = FF1[2] + T01[2];
            dFs[i] = Math.Sqrt(ddF[0] * ddF[0] + ddF[1] * ddF[1] + ddF[2] * ddF[2]);
        }
    }
    void Output_xyz()
    {
        double V0R;
        double V1R;
        double V2R;
        dL = T / ks;
        L0 = LL - dL;
        
        for(int i=0; i <= 2; i++)
        {
            X0[i] = 0.0;
            X1[i] = 0.0;
            X2[i] = 0.0;
            V0[i] = 0.0;
            V1[i] = 0.0;
            V2[i] = 0.0;
        }
        //---------------------------------------------------------------------
        V0R = w0 * R0;
        V1R = w0 * R1;
        V2R = w0 * R2;
        //---------------------------------------------------------------------
        X0[0] = R0;
        V0[2] = V0R;
        //---------------------------------------------------------------------
        X1[0] = R1 * Math.Cos(-phi1);
        X1[1] = -H1;
        X1[2] = R1 * Math.Sin(-phi1);
        //---------------------------------------------------------------------
        V1[0] = V1R * Math.Cos(-phi1 + PI / 2.0);
        V1[2] = V1R * Math.Sin(-phi1 + PI / 2.0);
        //---------------------------------------------------------------------
        X2[0] = R2 * Math.Cos(-phi1 - phi2);
        X2[1] = -H1 - H2;
        X2[2] = R2 * Math.Sin(-phi1 - phi2);
        //---------------------------------------------------------------------
        V2[0] = V2R * Math.Cos(-phi1 - phi2 + PI / 2.0);
        V2[2] = V2R * Math.Sin(-phi1 - phi2 + PI / 2.0);
        //---------------------------------------------------------------------
    }
    void solve_equation2(double s, double k, double l, ref double [] root, ref bool [] ifroot)
    {
        double a2 = k*k + 1;
        double a1 = -2 * (k * k + l * l + 1);
        double a0 = 2 * l * l * (s * s - 1) / (s * s + 1) + l * l * l * l / (k * k + 1) + k * k + 1;
        double delta = a1 * a1 - 4 * a0 * a2;
        if (delta < 0)
        {
            ifroot[0] = false;
            ifroot[1] = false;
        }
        else if (delta == 0)
        {
            ifroot[0] = true;
            ifroot[1] = false;
            root[0] = -a1 / 2.0 / a0;
        }
        else
        {
            root[0] = (-a1 - System.Math.Sqrt(delta))/2.0/a2;
            root[1] = (-a1 + System.Math.Sqrt(delta))/2.0/a2;
            if (root[0] >= 0)
            {
                ifroot[0] = true;
            }
            if (root[1] >= 0)
            {
                ifroot[1] = true;
            }
        }
    }
}
