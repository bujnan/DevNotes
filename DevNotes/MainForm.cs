using System;
using System.Windows.Forms;
using System.IO;

namespace DevNotes
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            RestoreWindowState();
            OpenRecentFile();
        }

        private bool hasUnsavedChanges = false;
        private readonly string recentFilePath = 
        Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "DevNotes",
        "recent.txt");
        private readonly string windowStateFilePath =
        Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "DevNotes",
        "windowstate.txt");

        private void NewNote()
        {
            txtNote.Clear();
            txtNote.Focus();
            hasUnsavedChanges = false;
        }

        private void RememberRecentFile(string openedFile)
        {
            try
            {
                string folder = Path.GetDirectoryName(recentFilePath);
        
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
        
                File.WriteAllText(recentFilePath, openedFile);
            }
            catch
            {
                // Ignore.
                // Failure to remember the recent file
                // must never stop the application.
            }
        }

        private void OpenRecentFile()
        {
            try
            {
                if (!File.Exists(recentFilePath))
                {
                    return;
                }
        
                string previousFile = File.ReadAllText(recentFilePath);
        
                if (!File.Exists(previousFile))
                {
                    return;
                }
        
                txtNote.Text = File.ReadAllText(previousFile);
                hasUnsavedChanges = false;
            }
            catch
            {
                // Ignore.
                // Startup should never fail because of a recent file.
            }
        }

        private void RememberWindowState()
        {
            try
            {
                string folder = Path.GetDirectoryName(windowStateFilePath);
        
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
        
                File.WriteAllText(windowStateFilePath, this.WindowState.ToString());
            }
            catch
            {
                // Ignore.
                // Failing to save the window state should never stop the application.
            }
        }

        private void RestoreWindowState()
        {
            try
            {
                if (!File.Exists(windowStateFilePath))
                {
                    return;
                }
        
                string savedState = File.ReadAllText(windowStateFilePath);
        
                if (savedState == FormWindowState.Maximized.ToString())
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                }
            }
            catch
            {
                // Ignore.
                // Startup should never fail because of an invalid window state.
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewNote();
        }

        private void ExitApplication()
        {
            if (!hasUnsavedChanges)
            {
                RememberWindowState();
                this.Close();
                return;
            }

            DialogResult result = MessageBox.Show("You have unsaved changes.\nDo you want to exit?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                RememberWindowState();
                this.Close();
            }
        }

        private void OpenNote()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Open Note";
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    string selectedFile = openFileDialog.FileName;
                    txtNote.Text = File.ReadAllText(selectedFile);
                    RememberRecentFile(selectedFile);
                    hasUnsavedChanges = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("Sorry! I can't Open the file.");
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            ExitApplication();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenNote();
        }

        private void SaveNote()
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
                try
                {
                    string selectedFile = saveFileDialog.FileName;
                    File.WriteAllText(selectedFile, txtNote.Text);
                    hasUnsavedChanges = false;
                }
                catch (Exception)
                {
                    MessageBox.Show("Sorry! I can't save this note to Disk!");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveNote();
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
