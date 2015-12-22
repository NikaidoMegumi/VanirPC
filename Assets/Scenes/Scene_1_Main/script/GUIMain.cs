using UnityEngine;

public class GUIMain : MonoBehaviour {

    private Network network;

    // Use this for initialization
    void Start ()
    {
        network = Network.GetNetwork();
        network.Start();
    }
	
	// Update is called once per frame
	void Update ()
    {
        network.Update();
    }

    void OnGUI()
    {
        if (network.m_serverConnected)
        {
            GUI_Main();
        }
        else
        {
            GUI_Disconnect();
        }
    }

    private void GUI_Main()
    {
    }

    private void GUI_Disconnect()
    {
    }
}
