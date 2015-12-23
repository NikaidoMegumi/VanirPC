using ExitGames.Client.Photon;
using VanirProtocol;
using System.Collections.Generic;
using VanirProtocol.ModuleParameters.ModuleAccountManager;

namespace Assets.Controller.Network.ModuleNetwork.AccountManager
{
    public class AccountManager
    {
        private PhotonPeer peer;

        public bool LoginStatus;

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
                        if (operationResponse.ReturnCode == (short)FormatReturnCodeLogin.RTC_Login_Success)
                        {
                            LoginStatus = true;
                        }
                        else
                        {
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
                { (byte)FormatRequestLogin.unityId, "noneId"},
                { (byte)FormatRequestLogin.userAccount, memberID},
                { (byte)FormatRequestLogin.userPassword, memberPW},
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
