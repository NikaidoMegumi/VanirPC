using UnityEngine;
using ExitGames.Client.Photon;
using System;
using System.Security;
using System.Collections.Generic;
using VanirProtocol;

public class PhotonClient : MonoBehaviour, IPhotonPeerListener
{

    public string ServerAddress = "139.196.167.225:5055";
    protected string ServerApplication = "Vanir";

    protected PhotonPeer peer;
    public bool ServerConnected;

    public string memberID = "";
    public string memberPW = "";
    public bool LoginStatus;
    public string getMemberID = "";
    public string getMemberPW = "";
    public string getNickname = "";
    public int getRet = 0;
    public string LoginResult = "";

    void Start()
    {
        ServerConnected = false;
        LoginStatus = false;

        peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        Connected();
    }

    internal virtual void Connected()
    {
        try
        {
            peer.Connect(ServerAddress, ServerApplication);
        }
        catch (SecurityException se)
        {
            DebugReturn(0, "Connection Failed. " + se.ToString());
        }
    }

    void Update()
    {
        peer.Service();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log(message);
    }

    public void OnEvent(EventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        DebugReturn(0, string.Format("OperationResult:" + operationResponse.OperationCode.ToString()));
        switch (operationResponse.OperationCode)
        {
            case (byte)OperationCode.Login:
                {
                    if (operationResponse.ReturnCode == (short)ErrorCode.Ok)
                    {
                        getRet = Convert.ToInt32(operationResponse.Parameters[(byte)LoginResponseCode.Ret]);
                        getMemberID = Convert.ToString(operationResponse.Parameters[(byte)LoginResponseCode.MemberID]);
                        getMemberPW = Convert.ToString(operationResponse.Parameters[(byte)LoginResponseCode.MemberPW]);
                        getNickname = Convert.ToString(operationResponse.Parameters[(byte)LoginResponseCode.Nickname]);
                        LoginStatus = true;
                    }
                    else
                    {
                        LoginResult = operationResponse.DebugMessage;
                        LoginStatus = false;
                    }
                    break;
                }
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        DebugReturn(0, string.Format("PeerStatusCallback: {0}", statusCode));
        switch (statusCode)
        {
            case StatusCode.Connect:
                ServerConnected = true;
                break;
            case StatusCode.Disconnect:
                ServerConnected = false;
                break;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(30, 10, 400, 20), "Vanir Unity3D Test");
        if (ServerConnected)
        {
            GUI.Label(new Rect(30, 30, 400, 20), "Connected");
            GUI.Label(new Rect(30, 60, 80, 20), "MemberID:");
            memberID = GUI.TextField(new Rect(110, 60, 100, 20), memberID, 10);
            GUI.Label(new Rect(30, 90, 80, 20), "MemberPW:");
            memberPW = GUI.TextField(new Rect(110, 90, 100, 20), memberPW, 10);
            if (GUI.Button(new Rect(30, 120, 100, 24), "Login"))
            {
                var parameter = new Dictionary<byte, object>
                {
                    { (byte)LoginParameterCode.MemberID, memberID },
                    { (byte)LoginParameterCode.MemberPW, memberPW }
                };
                peer.OpCustom((byte)OperationCode.Login, parameter, true);
            }
            if (LoginStatus)
            {
                GUI.Label(new Rect(30, 150, 400, 20), "Your MemberID : " + getMemberID);
                GUI.Label(new Rect(30, 170, 400, 20), "Your Password : " + getMemberPW);
                GUI.Label(new Rect(30, 190, 400, 20), "Your Nickname : " + getNickname);
                GUI.Label(new Rect(30, 210, 400, 20), "Ret : " + getRet.ToString());
            }
            else
            {
                GUI.Label(new Rect(30, 150, 400, 20), "Please Login");
                GUI.Label(new Rect(30, 170, 400, 20), LoginResult);
            }
        }
        else
        {
            GUI.Label(new Rect(30, 30, 400, 20), "Disconnect");
        }
    }
}
