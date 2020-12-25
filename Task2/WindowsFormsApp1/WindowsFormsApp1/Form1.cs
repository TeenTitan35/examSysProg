using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public void GetAllProcess()
        {
            listView1.Items.Clear();
            Process[] processList = Process.GetProcesses();
            foreach (Process process in processList)
            {
                ListViewItem item0 = new ListViewItem(process.ProcessName);
                item0.SubItems.Add(process.MainWindowTitle);//Под процессы.ProcessName - гугл. MainWindowTitle - видео/др. 
                item0.SubItems.Add(process.Id.ToString());
                // item0.SubItems.Add(process.StartTime.ToString());ОШИБКА
                var ram = Process.GetCurrentProcess().WorkingSet64;
                var ramLV = ram / (1024 * 1024);
                item0.SubItems.Add(ramLV.ToString() + " МБ");
                listView1.Items.Add(item0);
                item0.Tag = process;

            }

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            GetAllProcess();
        }

        //------------------------TOOLSTRIP'ы-----------------------------------
        private void запуститьПОToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (formRNT frm = new formRNT())
            {
                if (frm.ShowDialog() == DialogResult.OK) ;
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void завершитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ListViewItem item = listView1.SelectedItems[0];
                Process process = (Process)item.Tag;
                process.Kill();
                process.WaitForExit();
                GetAllProcess();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetAllProcess();
        }

        //------------------------НАСТРОЙКИ-------------------------------------
        private void radioButton1LightTheme_CheckedChanged(object sender, EventArgs e)
        {
            Form1 f1 = new Form1(); //Если конструкторы будут один общий конструктор(под коммом "НАСТРОЙКИ) - будет StackOverFlow.Exception
                                    ///I wrote this com in 27.08. I dunno wha I meant
            f1.BackColor = Color.WhiteSmoke;
            this.BackColor = Color.WhiteSmoke;
            menuStrip1.BackColor = Color.WhiteSmoke;
            tabPage1.BackColor = Color.WhiteSmoke;
            tabPage2.BackColor = Color.WhiteSmoke;
        }

        private void radioButton2DarkTheme_CheckedChanged(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            f1.BackColor = Color.DarkGray;
            this.BackColor = Color.DarkGray;
            menuStrip1.BackColor = Color.DarkGray;
            tabPage1.BackColor = Color.DarkGray;
            tabPage2.BackColor = Color.DarkGray;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                TopMost = true;
            }
            else { TopMost = false; }
        }

        private void button1SSS_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            int s = 0;//секунда
            ++s;
            if (s == 3) //после 3 секунды - обновление списка
            {
                GetAllProcess();
            }
        }

        private void показатьKeyLoggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string pathToDirectory = "C:/Users/adosp/source/repos/tipKL/tipKL/bin/Debug"; //Не забудь изменить путь!!!!!
            string pathToFileString = "tipKL.exe"; 
            ProcessStartInfo infoStartProcess = new ProcessStartInfo();

            infoStartProcess.WorkingDirectory = pathToDirectory;
            infoStartProcess.FileName = pathToFileString;

            Process.Start(infoStartProcess);
        }

    }
}
