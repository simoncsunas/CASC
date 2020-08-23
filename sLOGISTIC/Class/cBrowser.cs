using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// add-in
using System.Windows.Forms;

namespace sLOGISTIC
{
    class cBrowser
    {
        public static string  gsBrowser(string psIniDir, string psRem, string psFilter)
        {
            string lsReturn = string.Empty;
            try
            {
                OpenFileDialog of = new OpenFileDialog();

                of.InitialDirectory = psIniDir;
                of.Filter = psRem + "|" + psFilter + "|All Files (*.*)|*.*";
                of.FilterIndex = 1;
                of.Multiselect = false;

                if (of.ShowDialog() == DialogResult.OK) { lsReturn = of.FileName; }

                of.Dispose();
            }
            catch 
            { 
                return lsReturn; 
            }
            return lsReturn;
        }
    }
}
