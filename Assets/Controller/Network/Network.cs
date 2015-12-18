using ExitGames.Client.Photon;
using Assets.Controller.Network.ModuleNetwork.AccountManager;
using System.Security;
using System;
using UnityEngine;

public class Network : IPhotonPeerListener
{
    private static Network network;
    private PhotonPeer peer;

    private string ServerAddress = "139.196.167.225:5055";
    private string ServerApplication = "Vanir";

    /// <summary>
    /// network连接结果字段
    /// </summary>
    public bool m_serverConnected;
    /// <summary>
    /// network初始化结果字段
    /// </summary>
    private bool m_networkSetuped;
    /// <summary>
    /// 各模块成员
    /// </summary>
    public AccountManager accountManager;

    private Network()
    {
        m_networkSetuped = false;
        m_serverConnected = false;
    }

    public static Network GetNetwork()
    {
        if(null == network)
        {
            network = new Network();
        }
        return network;
    }

    public void Start()
    {
        if(!m_networkSetuped)
        {
            m_serverConnected = false;
            peer = new PhotonPeer(this, ConnectionProtocol.Udp);
            if(!ConnectServer())
            {
                Debug.Log("连接服务器失败");
                return;
            }
            accountManager = new AccountManager(peer);
            if (!accountManager.InitModule())
            {
                Debug.Log("账号管理网络模块初始化失败");
                return;
            }
            m_networkSetuped = true;
        }
    }

    public void Update()
    {
        peer.Service();
    }

    private bool ConnectServer()
    {
        try
        {
            if (!peer.Connect(ServerAddress, ServerApplication))
            {
                Debug.Log("peer.Connect失败");
                return false;
            }
        }
        catch (SecurityException se)
        {
            Debug.Log("Connect抛出异常:" + se.ToString());
        }
        return true;
    }

    public void OnEvent(EventData eventData)
    {
        throw new NotImplementedException();
    }

    public void OnOperationResponse(OperationResponse operationResponse)
    {
        Debug.Log(string.Format("OperationResult:" + operationResponse.OperationCode.ToString()));
        if(accountManager.CanRun(operationResponse))
        {
            accountManager.Run(operationResponse);
        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        Debug.Log(string.Format("PeerStatusCallback: {0}", statusCode));
        switch (statusCode)
        {
            case StatusCode.Connect:
                m_serverConnected = true;
                break;
            case StatusCode.Disconnect:
                m_serverConnected = false;
                break;
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }
}
