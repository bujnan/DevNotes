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

        private bool hasUnsavedChanges = false;

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtNote.Clear();
            txtNote.Focus();
            hasUnsavedChanges = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (!hasUnsavedChanges)
            {
                this.Close();
                return;
            }

            DialogResult result = MessageBox.Show("You have unsaved changes.\nDo you want to exit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }

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
                hasUnsavedChanges = false;
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
                hasUnsavedChanges = false;
            }
        }

        private void txtNote_TextChanged(object sender, EventArgs e)
        {
            hasUnsavedChanges = true;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N)
            {
                btnNew_Click(btnNew, EventArgs.Empty);
            }

            if (e.Control && e.KeyCode == Keys.O)
            {
                btnOpen_Click(btnOpen, EventArgs.Empty);
            }

            if (e.Control && e.KeyCode == Keys.S)
            {
                btnSave_Click(btnSave, EventArgs.Empty);
            }
        }
    }
}
