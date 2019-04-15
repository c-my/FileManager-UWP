using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.StartScreen;

namespace FileManager_UWP.Model
{
    public class VirtualFolder {
        public string Name { get; set; }
        public List<string> Includes { get; set; }
    }
}
