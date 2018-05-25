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
    }
}
