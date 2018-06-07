using GUICommunication.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ConfigModel
    {
        private IGUIClient client;

        public ConfigModel()
        {
            client = GUIClient.Instance();
        }

        public JObject getConfig()
        {
            return client.
        }
    }
}