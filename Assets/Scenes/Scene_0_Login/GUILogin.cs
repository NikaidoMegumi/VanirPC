
using System;
using UnityEngine;

public class GUILogin: MonoBehaviour
{
    private Network network;

    void Start()
    {
        network = Network.GetNetwork();
        network.Start();
    }

    void Update()
    {
        network.Update();
    }

    void OnGUI()
    {
        if (network.m_serverConnected)
        {
            if(network.accountManager.LoginStatus)
            {
                GUI_LoginSuccess();
            }
            else
            {
                GUI_ToLogin();
            }
        }
        else
        {
            GUI_Disconnect();
        }
    }

    private void GUI_ToLogin()
    {
        GUI.Label(new Rect(30, 60, 80, 20), "账号ID:");
        network.accountManager.memberID = GUI.TextField(new Rect(110, 60, 100, 20), network.accountManager.memberID, 10);
        GUI.Label(new Rect(30, 90, 80, 20), "账号密码:");
        network.accountManager.memberPW = GUI.TextField(new Rect(110, 90, 100, 20), network.accountManager.memberPW, 10);
        if (GUI.Button(new Rect(30, 120, 100, 24), "登录"))
        {
            network.accountManager.RunLogin();
        }
    }

    private void GUI_LoginSuccess()
    {
        GUI.Label(new Rect(30, 190, 400, 20), "你的角色名称:" + network.accountManager.getNickname);
    }

    private void GUI_Disconnect()
    {
        GUI.Label(new Rect(30, 30, 400, 20), "断开连接");
    }
}
