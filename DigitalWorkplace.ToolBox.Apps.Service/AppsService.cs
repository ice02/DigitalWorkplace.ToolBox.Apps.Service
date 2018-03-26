using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DigitalWorkplace.ToolBox.Apps.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class AppsService : IAppsService
    {
        public void GetInfoFromCache()
        {
        }

        #region Callback Connection Management
        public void Connect(string name)
        {

        }

        public void Disconnect(string name)
        {

        }
        #endregion


        public void MessageToSomeone(CommunicatUnit composite, string someoneName)
        {
            throw new NotImplementedException();
        }


        public void MessagesToSomeone(List<CommunicatUnit> composites, string someoneName)
        {
            throw new NotImplementedException();
        }
    }

    public class NamedPipeBindingCallbackService : IAppsCallbackService
    {
        public void Message(CommunicatUnit composite)
        {
        }


        public void HostClientList(List<string> clientsAssemblyFriendNames)
        {
            throw new NotImplementedException();
        }


        public void Messages(List<CommunicatUnit> composite)
        {
            throw new NotImplementedException();
        }
    }
}
