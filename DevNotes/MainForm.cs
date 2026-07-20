using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DevNotes
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtNote.Clear();
            txtNote.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Note";
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;
                txtNote.Text = File.ReadAllText(selectedFile);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNote.Text))
            {
                MessageBox.Show("No Note to Save", "Warning!");
                return;
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Save Note";
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult result = saveFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string selectedFile = saveFileDialog.FileName;
                File.WriteAllText(selectedFile,txtNote.Text);
            }
        }
    }
}
