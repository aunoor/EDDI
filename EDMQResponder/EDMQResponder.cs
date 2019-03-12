using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Eddi;
using EddiEvents;
using Utilities;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;

namespace EDMQResponder 
{
    public class EDMQResponder : EDDIResponder
    {
        /// <summary>
        /// A short name for the responder
        /// </summary>
       public string ResponderName()
        {
            return "EDDI MQ Responder";
        }

        /// <summary>
        /// A localized name for the responder
        /// </summary>
        public string LocalizedResponderName()
        {
            return ResponderName();
        }

        /// <summary>
        /// The version of the responder
        /// </summary>
        public string ResponderVersion()
        {
            return "0.1";
        }

        /// <summary>
        /// A brief description of the responder
        /// </summary>
        public string ResponderDescription()
        {
            return "Simple ZeroMQ EDDI events server";
        }

        /// <summary>
        /// Called when this responder is started; time to carry out initialisation
        /// </summary>
        /// <returns>true if the responder has started successfully; otherwise false</returns>
        public bool Start()
        {
            if (_server != null)
            {
                _server.Dispose();
            }

            _server = new ResponseSocket();
            try
            {
                _server.Bind("@tcp://localhost:5556");
            } catch (Exception e)
            {
                Logging.Error("Can't bind socket: "+e.Message, memberName:"EDMQResponder");
                return false;
            }
            return true;            
        }

        /// <summary>
        /// Called when this responder is stopped; time to shut down daemons and similar
        /// </summary>
        public void Stop()
        {
            if (_server != null)
            {
                _server.Dispose();
                _server = null;
            }
        }

        /// <summary>
        /// Called when this responder needs to reload its configuration
        /// </summary>
        public void Reload()
        {

        }

        /// <summary>
        /// Called when an event is found. Must not change global states.
        /// </summary>
        public void Handle(Event theEvent)
        {
            if (_server == null) return;
            string output = JsonConvert.SerializeObject(theEvent);
            var message = new NetMQMessage();
            message.Append(theEvent.GetType().Name);
            message.Append(output);
            _server.SendMultipartMessage(message);
        }

        public UserControl ConfigurationTabItem()
        {
            return null;
        }

        #region Private members

        private ResponseSocket _server;

        #endregion

    }
}
