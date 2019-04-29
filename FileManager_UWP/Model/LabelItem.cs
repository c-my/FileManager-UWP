using System;
using System.Collections.Generic;
using Windows.UI;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileManager_UWP.Model {
    public class LabelItem {
        public string tag;
        public Color color = Color.FromArgb(0xf0, 0xC6, 0xE2, 0xFF);
        public LabelItem(string tag) {
            this.tag = tag;
        }
    }
}
