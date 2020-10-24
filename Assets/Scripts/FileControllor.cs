using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Common;
using UnityEngine.UI;
using System;

public class FileControllor : MonoBehaviour
{
    public Button btnUpload;

    public ParamSetting paramSetting;


    private void Start()
    {
        btnUpload.onClick.AddListener(UploadFile);
    }

    void UploadFile()
    {
        OpenFileDlg pth = new OpenFileDlg();
        pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
        //pth.filter = "all (*.*)";
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath;  // default path  
        //pth.title = "Upload data";
        //pth.defExt = "txt";
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        //0x00080000   是否使用新版文件选择窗口
        //0x00000200   是否可以多选文件
        if (OpenFileDialog.GetOpenFileName(pth))
        {
            string filepath = pth.file;
            paramSetting.ParseFile(filepath);
            //string newfile = Application.streamingAssetsPath + "/" + pth.fileTitle;
            
            //if (!Directory.Exists(Application.streamingAssetsPath)) {
            //    Directory.CreateDirectory(Application.streamingAssetsPath);
            //}
            //File.WriteAllText(newfile, File.ReadAllText(filepath));
        }

    }


    public void OpenProject()
    {
        OpenFileDlg pth = new OpenFileDlg();
        pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
        pth.filter = "txt (*.txt)";
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath;  // default path  
        pth.title = "打开项目";
        pth.defExt = "txt";
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        //0x00080000   是否使用新版文件选择窗口
        //0x00000200   是否可以多选文件
        if (OpenFileDialog.GetOpenFileName(pth))
        {
            string filepath = pth.file;//选择的文件路径;  
            Debug.Log(filepath);
        }
    }
    public void SaveProject()
    {
        SaveFileDlg pth = new SaveFileDlg();
        pth.structSize = System.Runtime.InteropServices.Marshal.SizeOf(pth);
        pth.filter = "txt (*.txt)";
        pth.file = new string(new char[256]);
        pth.maxFile = pth.file.Length;
        pth.fileTitle = new string(new char[64]);
        pth.maxFileTitle = pth.fileTitle.Length;
        pth.initialDir = Application.dataPath;  // default path  
        pth.title = "保存项目";
        pth.defExt = "txt";
        pth.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;
        if (SaveFileDialog.GetSaveFileName(pth))
        {
            string filepath = pth.file;//选择的文件路径;  
            Debug.Log(filepath);
        }
    }

}