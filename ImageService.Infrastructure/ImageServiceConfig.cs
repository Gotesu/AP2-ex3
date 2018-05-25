using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Infrastructure
{
    public class ImageServiceConfig
    {
        private List<string> handlers { get; set; }
        private int thumbSize { get; set; }
        private string source { get; set; }
        private string logName { get; set; }
        private string OPD { get; set; }

        public string ToJSON()
        {
            string json = JsonConvert.SerializeObject(this, Formatting.Indented);
            return json;
        }

        public static ImageServiceConfig FromJSON(string str)
        {
            return JsonConvert.DeserializeObject<ImageServiceConfig>(str);
        }
    }
}
