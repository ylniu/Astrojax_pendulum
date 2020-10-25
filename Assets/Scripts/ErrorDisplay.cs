using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorDisplay : MonoBehaviour
{
    internal void OnEnable()
    {
        Application.RegisterLogCallback(HandleLog);
    }
    internal void OnDisable()
    {
        Application.RegisterLogCallback(null);
    }
    private string m_logs;
    /// <summary>
    ///
    /// </summary>
    /// <param name="logString">������Ϣ</param>
    /// <param name="stackTrace">���ٶ�ջ</param>
    /// <param name="type">��������bai</param>
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        m_logs += logString + "\n";
    }
    public bool Log;
    private Vector2 m_scroll;
    internal void OnGUI()
    {
        if (!Log)
            return;
        m_scroll = GUILayout.BeginScrollView(m_scroll);
        GUILayout.Label(m_logs);
        GUILayout.EndScrollView();
    }
}
