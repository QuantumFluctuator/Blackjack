using System.IO;
using System.Windows.Forms;

namespace Project
{
    public partial class htmlViewer : Form
    {
        public htmlViewer(string path)
        {
            string strAppPath = Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            InitializeComponent();
            webBrowser.Navigate(Path.Combine(strAppPath, path));
            this.Text = path;
            this.ShowDialog();
        }
    }
}
