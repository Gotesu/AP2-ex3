using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServiceGuiCommunication.GUI_side
{
    public class GuiSide_client : IGuiSide_client
    {
        private IPEndPoint ep { get; }
        private TcpClient client { get; }
        private bool connected;
        private static GuiSide_client instance;
        private ImageServiceConfig config;
        private EventLogEntryCollection EventEntries;

        public event EventHandler<EventLogEntry> newLog;

        public static IGuiSide_client get_instance()
        {
            if (instance == null)
            {
                instance = new GuiSide_client();
            }
            return instance;
        }

        private GuiSide_client()
        {
            connected = false;
            ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            client = new TcpClient();
            try
            {
                client.Connect(ep);
                
            }
            catch (Exception e)
            {
                Console.WriteLine("not connected", e.Data);
                return;
            }
            Console.WriteLine("connected");
            connected = true;
            using (NetworkStream stream = client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                try
                {
                    writer.Write((int)CommandEnum.GetConfigCommand);
                    string result = reader.ReadString();
                    config = ImageServiceConfig.FromJSON(result);
                    Console.WriteLine("configuration recieved");
                } catch(Exception)
                {
                    connected = false;
                    Console.WriteLine("error in getting configuration");
                }
                try
                {
                    writer.Write((int)CommandEnum.LogCommand);
                    string result = reader.ReadString();
                    EventEntries = JsonConvert.DeserializeObject<EventLogEntryCollection>(result);
                }
                catch (Exception)
                {
                    Console.WriteLine("failed to get log, disconnected");
                    connected = false;
                }
            }
            
        }


        public void closeClient()
        {
            client.Close();
        }

        public ImageServiceConfig getConfig()
        {
            return config;
        }

        public EventLogEntryCollection getEntries()
        {
            return EventEntries;
        }
    }
}
