using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace PureSharp
{
    public partial class MainForm : MaterialForm
    {
        public MainForm()
        {
            // make from center in screen
            StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
        }

        private List<string> LoadHostFile()
        {
            var host = File.ReadAllText(Constant.hostFile);
            var content = host.Split('\n').ToList();
            return content;
        }

        private void SaveHostFile(List<string> content)
        {
            var host = string.Join("\n", content);
            File.WriteAllText(Constant.hostFile, host);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // load material theme
            // Create a material theme manager and add the form to manage (this)
            MaterialSkinManager materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;

            // Configure color schema
            materialSkinManager.ColorScheme = new ColorScheme(
                Primary.Red800, Primary.Red800,
                Primary.Red900, Accent.Red400,
                TextShade.WHITE
            );

            // fix size of form
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;

            this.SetStyle(ControlStyles.StandardDoubleClick, false);

            LoadTime();
            LoadOptionScreen();
        }

        private void LoadOptionScreen()
        {
            Configure configure = new Configure();

            if (configure.GetConfigs().Count == 0)
            {
                return;
            }

            // get value of checkbox 
            materialCheckBox1.Checked = Convert.ToBoolean(configure.GetValueConfigByKey("facebook").ToLower().Trim());
            materialCheckBox2.Checked = Convert.ToBoolean(configure.GetValueConfigByKey("youtobe").ToLower().Trim());
            materialCheckBox3.Checked = Convert.ToBoolean(configure.GetValueConfigByKey("zalo").ToLower().Trim());
            materialCheckBox4.Checked = Convert.ToBoolean(configure.GetValueConfigByKey("messenger").ToLower().Trim());
            materialSingleLineTextField1.Text = configure.GetValueConfigByKey("locktime");
        }

        private void GetOptionScreen()
        {
            Constant.options.Clear();
            // get value of checkbox 
            Constant.options.Add(materialCheckBox1.Text.ToString().ToLower().Trim(), materialCheckBox1.Checked);
            Constant.options.Add(materialCheckBox2.Text.ToString().ToLower().Trim(), materialCheckBox2.Checked);
            Constant.options.Add(materialCheckBox3.Text.ToString().ToLower().Trim(), materialCheckBox3.Checked);
            Constant.options.Add(materialCheckBox4.Text.ToString().ToLower().Trim(), materialCheckBox4.Checked);
            Constant.lockTime = materialSingleLineTextField1.Text;
        }

        private void SaveOptionScreen()
        {
            Configure configure = new Configure();
            configure.SetKeyAndValueConfig(materialCheckBox1.Text.ToString().ToLower().Trim(),
                materialCheckBox1.Checked.ToString().ToLower().Trim());
            configure.SetKeyAndValueConfig(materialCheckBox2.Text.ToString().ToLower().Trim(),
                materialCheckBox2.Checked.ToString().ToLower().Trim());
            configure.SetKeyAndValueConfig(materialCheckBox3.Text.ToString().ToLower().Trim(),
                materialCheckBox3.Checked.ToString().ToLower().Trim());
            configure.SetKeyAndValueConfig(materialCheckBox4.Text.ToString().ToLower().Trim(),
                materialCheckBox4.Checked.ToString().ToLower().Trim());
            configure.SetKeyAndValueConfig("locktime", materialSingleLineTextField1.Text);
        }

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            var content = LoadHostFile();
            GetOptionScreen();

            foreach (var item in Constant.options)
            {
                if (item.Value)
                {
                    var check = content.Find(element => element.Contains(item.Key.Trim().ToLower()));
                    if (check == null)
                    {
                        // add facebook to host file
                        content.Add($"127.0.0.1 www.{item.Key.ToLower().Trim()}.com");
                    }
                }
                else
                {
                    var check = content.Find(element => element.Contains(item.Key.Trim().ToLower()));
                    if (check != null)
                    {
                        // remove facebook to host file
                        content.Remove(check);
                    }
                }
            }

            SaveOptionScreen();
            SaveHostFile(content);
            LoadTime();
        }

        private void LoadTime()
        {
            Configure configure = new Configure();

            if (configure.GetValueConfigByKey("locktime") == "")
            {
                return;
            }

            DisableAll();
            materialRaisedButton1.Text = "Locking...";
            materialRaisedButton1.Enabled = false;
            materialSingleLineTextField1.Enabled = false;
            materialSingleLineTextField1.Text = configure.GetValueConfigByKey("locktime");
            // make countdown timer
            var time = Convert.ToInt32(configure.GetValueConfigByKey("locktime")) * 60;

            Timer timer = new Timer();
            timer.Interval = 1000;
            timer.Start();
            timer.Tick += (sender, e) =>
            {
                if (time <= 0)
                {
                    timer.Stop();
                    timer.Dispose();
                    materialRaisedButton1.Text = "Active";
                    materialRaisedButton1.Enabled = true;
                    materialSingleLineTextField1.Enabled = true;
                    materialSingleLineTextField1.Text = "0";
                    EnableAll();
                }

                materialLabel2.Text = GetTime(time);
                time--;
            };
        }

        private string GetTime(int time)
        {
            var minute = time / 60;
            var second = time % 60;
            return $"{minute}:{second}";
        }

        private void DisableAll()
        {
            materialCheckBox1.Enabled = false;
            materialCheckBox2.Enabled = false;
            materialCheckBox3.Enabled = false;
            materialCheckBox4.Enabled = false;
            materialRaisedButton1.Enabled = false;
            materialSingleLineTextField1.Enabled = false;
        }

        private void EnableAll()
        {
            materialCheckBox1.Enabled = true;
            materialCheckBox2.Enabled = true;
            materialCheckBox3.Enabled = true;
            materialCheckBox4.Enabled = true;
            materialRaisedButton1.Enabled = true;
            materialSingleLineTextField1.Enabled = true;
        }

        private void materialSingleLineTextField1_Click(object sender, EventArgs e)
        {
        }

        private void materialRaisedButton1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void materialRaisedButton1_MouseCaptureChanged(object sender, EventArgs e)
        {
        }
    }
}