namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
            Manager man = new Manager();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            man.StreamWrite(rtbMessage.Text);
        }
    }
}