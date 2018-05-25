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
        public List<string> handlers { get; set; }
        public int thumbSize { get; set; }
        public string source { get; set; }
        public string logName { get; set; }
        public string OPD { get; set; }

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
