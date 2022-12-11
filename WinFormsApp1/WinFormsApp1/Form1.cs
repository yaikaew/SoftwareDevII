namespace WinFormsApp1


{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 obj = new AboutBox1();
            obj.Show();
        }

        private OpenFileDialog openFileDialog1;

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1 = new OpenFileDialog()
            {
                FileName = "Select a text file",
                Filter = "Text files (*.txt) | *.txt",
                Title = "Open text file"
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filePath = openFileDialog1.FileName;
                using (System.IO.Stream str = openFileDialog1.OpenFile())
                {

                }
            }
        }
    }
}