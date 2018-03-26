using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DigitalWorkplace.ToolBox.Apps.Service
{
    ///  Callback operations are part of the service contract, and it's up to the service contract to define its own callback contract.
    ///  A service conract cann have at most one callback contract. Once defined, the clients are required to support the callback and
    ///  provide the callback endpoint to the service in every call. To define a callback contract, the ServiceContract attribute offers 
    ///  the CallbackContract property of the type Type.
    [ServiceContract(SessionMode = SessionMode.Required,
                     CallbackContract = typeof(IAppsCallbackService))]
    public interface IAppsService
    {
        [OperationContract(IsOneWay = true)]
        void GetInfoFromCache();

        //[OperationContract(IsOneWay = true)]
        //void MessageToSomeone(CommunicatUnit composite, string someoneName);

        //[OperationContract(IsOneWay = true)]
        //void MessagesToSomeone(List<CommunicatUnit> composites, string someoneName);

        #region Callback Connection Management
        /// Because of mechanism of callback supplied by WCF, we have to come up with ourself some application-level protocol or a 
        /// consistent parttern for managing the life cycle of the connection.
        /// The service can only call back to the client if the client-side channel is still open, which is typically achieved by
        /// not closing the proxy. Keeping the proxy open will also prevent the callback object from bien garbage-collected.

        /// You may always want to add the Connect() and Disconnect() pair on a sessionful service simply as feature, because it
        /// enables the client to decide when to start or stop receiving callbacks during the session

        [OperationContract(IsOneWay = false)]
        void Connect(string name);

        /// If the services  maintains a reference on a callback endpoint and the client-side proxy is closed or the client application 
        /// itself is gone, when the service invokes the callback it will get an ObjectDisposedException from the service channel. 
        /// It is therefore preferable for the client to inform the service when it no longer wishes to receive callbacks or when the
        /// client application is shutting down. In NamedPipeBindingService, InstanceContextMode = PerSession, read comments written on
        /// NamedPipeBindingService to get more info.
        [OperationContract(IsOneWay = true)]
        void Disconnect(string name);
        #endregion
    }

    [ServiceContract]
    public interface IAppsCallbackService
    {
        /// IsOneWay = true  
        /// Because : The service may want to invoke the callback reference that's passed in during the execution of a 
        /// contract operation. How ever such invocations are disallowed by defaut. By defaut, the service class is 
        /// configured for single-threaded access: the service instance context is associated with a lock, and only 
        /// one thread at a time can own the lock and access the service instance inside that context. Calling out to 
        /// the client during an operation call requires blocking the service thread and invoking the callback. The problem
        /// is that processing the reply message from the client on the same channel once the callback returns requires
        /// reentering the same context and negotiating ownership of the same lock, which will result in a deadlock. Note that
        /// the service may still invoke callbacks to other clients or call other services; it is the callback to its calling client 
        /// that will cause the deadlock.
        /// Set IsOneWay = true is method to avoid deadlock. Instead of two other method, set IsOneWay = true can maintain the service
        /// as single-threaded without release the lock.  Set IsOneWay = true enables the service to call back even when the
        /// concurrency mode is set to single-threaded, because there will not be any reply message to contend for the lock.
        [OperationContract(IsOneWay = true)]
        // If set IsOneWay = true, we cannot return a result. so  void Message();
        void GetInfoFromCache();

        [OperationContract(IsOneWay = true)]
        void Messages(List<CommunicatUnit> composite);

        [OperationContract(IsOneWay = true)]
        void HostClientList(List<string> clientsAssemblyFriendNames);
    }

    [DataContract]
    public class CommunicatUnit
    {
        string _message = "Message empty";
        [DataMember]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}