using System;
using ExitGames.Client.Photon;
using VanirProtocol;
using System.Collections.Generic;

namespace Assets.Controller.Network.ModuleNetwork.AccountManager
{
    public class AccountManager
    {
        private PhotonPeer peer;

        public bool LoginStatus;

        public string getNickname = "";
        public string LoginResult = "";

        public string memberID = "";
        public string memberPW = "";

        public AccountManager(PhotonPeer peer)
        {
            this.peer = peer;
        }

        internal bool CanRun(OperationResponse operationResponse)
        {
            if (((byte)OperationCode.AccountManager_Front < operationResponse.OperationCode)
                   && (operationResponse.OperationCode < (byte)OperationCode.AccountManager_Back))
                return true;
            else
                return false;
        }

        internal void Run(OperationResponse operationResponse)
        {
            switch (operationResponse.OperationCode)
            {
                case (byte)OperationCode.AccountManager_Login:
                    {
                        if (operationResponse.ReturnCode == (short)ErrorCode.Ok)
                        {
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

        internal void RunLogin()
        {
            var parameter = new Dictionary<byte, object>
                {
                    { (byte)LoginParameterCode.MemberID, memberID },
                    { (byte)LoginParameterCode.MemberPW, memberPW }
                };
            peer.OpCustom((byte)OperationCode.AccountManager_Login, parameter, true);
        }

        internal bool InitModule()
        {
            LoginStatus = false;
            return true;
        }
    }
}
