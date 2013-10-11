using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using eBdb.EpubReader;
using Ionic.Zip;

namespace Viewer
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog epubFileDialog = new OpenFileDialog();
            epubFileDialog.Title = "Open Resource File";
            epubFileDialog.Filter = "EPUB Files (.epub)|*.epub";

            if (epubFileDialog.ShowDialog() == DialogResult.OK) {
                string filename = epubFileDialog.FileName;
                mainForm.ActiveForm.Text = filename;
                readFile(filename);
                pnlHome.Visible = false;
                pnlViewer.Visible = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e) {
            pnlHome.Visible = true;
            pnlViewer.Visible = false;
        }        

        public void readFile(string filename){
            Epub epub = new Epub(@filename);
            epubExtract(@filename);
            string author = epub.Title[0];
            string title = epub.Creator[0];
            string htmlText = epub.GetContentAsHtml();
            string plainText = epub.GetContentAsPlainText();
            lblEpubDetail.Text = title + " - "+author;
            
            using (FileStream filestream = new FileStream("temp.html", FileMode.Create)) {
                using (StreamWriter write = new StreamWriter(filestream, Encoding.UTF8)) {
                    write.Write(htmlText);
                    //webViewer.Navigate("temp.html");
                    string curDir = Directory.GetCurrentDirectory();
                    webViewer.Url = new Uri(String.Format("file:///{0}/temp.html", curDir));
                }
            }
        }

        public void epubExtract(string filename){
            string tempDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(tempDirectory);
            string toUnpack = filename;
            string unpackDir = @tempDirectory;
            using (ZipFile zip = ZipFile.Read(toUnpack)){
                foreach (ZipEntry e in zip){
                    e.Extract(unpackDir, ExtractExistingFileAction.OverwriteSilently);
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e){
            OpenFileDialog epubFileDialog = new OpenFileDialog();
            epubFileDialog.Title = "Open Resource File";
            epubFileDialog.Filter = "EPUB Files (.epub)|*.epub";

            if (epubFileDialog.ShowDialog() == DialogResult.OK){
                string filename = epubFileDialog.FileName;
                mainForm.ActiveForm.Text = filename;
                pnlHome.Visible = false;
                pnlViewer.Visible = true;
                readFile(filename);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void webViewer_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAbout form = new frmAbout();
            form.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string tempFile = Path.GetTempFileName();
            //Use the file
            File.Delete(tempFile);
            Application.Exit();
        }
    }
}
