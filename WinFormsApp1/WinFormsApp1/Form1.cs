namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        Listener listener;
        Client client;
        public Form1()
        {
            InitializeComponent();
            listener = new Listener();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            client = new Client(txtIp.Text);
            client.StreamWrite(rtbMessage.Text);
        }
    }
}