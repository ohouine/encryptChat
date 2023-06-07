using System.Diagnostics;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        Listener listener;
        Client client;
        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            client = new Client(txtIp.Text);
            if (!await client.HandShake())
            {
                Debug.WriteLine("btnConnect_Click - HandShake failed");
            }
        }
    }
}