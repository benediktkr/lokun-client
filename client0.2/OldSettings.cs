using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lokunclient
{
    class OldSettings
    {
        public static OldSettings Load()
        {
            bool autostart = (bool)Properties.Settings.Default["autostart"];
            return new OldSettings() {
                Autostart = autostart
            };
        }

        public bool Autostart
        {
            get;
            set;
        }

        public void Save()
        {
            Properties.Settings.Default["autostart"] = this.Autostart;
        }
    }
}
