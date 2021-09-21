using PMS.Demo.Entity;
using PMS.Demo.Runtime;
using PMS.Demo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PMS.Demo.Client
{
    public partial class FrmAddPerson : Form
    {
        private Person person = null;
        private AForgeHelper aforgeHelper = null;
        public FrmAddPerson(Person p = null)
        {
            InitializeComponent();
            aforgeHelper = new AForgeHelper();
            person = p;
        }

        private void Connect()
        {
            aforgeHelper.SetDevice(cbxCaptureList.SelectedIndex);
            videoSourcePlayer1.VideoSource = aforgeHelper.videoDevice;
            videoSourcePlayer1.Start();
        }

        private void DisConnect()
        {
            if (videoSourcePlayer1.VideoSource != null)
            {
                videoSourcePlayer1.SignalToStop();
                videoSourcePlayer1.WaitForStop();
                videoSourcePlayer1.VideoSource = null;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmAddPerson_Load(object sender, EventArgs e)
        {
            //设置摄像头
            var capturelist = aforgeHelper.GetCaptureList();
            if (capturelist == null || capturelist.Count == 0)
            {
                MessageBox.Show("未找到摄像头！");
            }
            else
            {
                cbxCaptureList.Items.AddRange(aforgeHelper.GetCaptureList().ToArray());
                cbxCaptureList.SelectedIndex = 0;
            }
            //若是更新界面，则刷新UI信息
            if (person != null)
            {
                btnSubmit.Text = "更新";
                tbName.Text = person.name;
                tbName.Enabled = false;
                comboBox1.Text = person.gender;
                numericUpDown1.Value = (decimal)person.age;
                DateTime d = DateTime.Now;
                dateTimePicker1.Value = DateTime.TryParse(person.birthday, out d) ? d : d;
                if (File.Exists(person.photopath))
                {
                    try
                    {
                        using (Image img = Bitmap.FromFile(person.photopath))
                        {
                            pictureBox1.Image = new Bitmap(img);
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("无法找到图片，请重新拍摄");
                        person.photopath = string.Empty;
                    }
                }
            }
        }

        private void FrmAddPerson_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisConnect();
        }

        private void cbxCaptureList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisConnect();
            aforgeHelper.SetDevice(cbxCaptureList.SelectedIndex);
            Connect();
            //if(videoSourcePlayer1.IsRunning)
            Console.WriteLine(videoSourcePlayer1.IsRunning);
        }

        private void btnShoot_Click(object sender, EventArgs e)
        {
            Image img = videoSourcePlayer1.GetCurrentVideoFrame();
            pictureBox1.Image = img;
            GC.Collect();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (person == null)
                person = new Person() { id = Common.GetGUID() };
            string path = string.Empty;
            if (pictureBox1.Image != null)
            {
                path = Global.CacheFilePath + $"{person.id + person.name}.jpg";
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(path);
                using(Bitmap bmp = new Bitmap(pictureBox1.Image))
                {
                    bmp.Save(path, ImageFormat.Jpeg);
                }
            }
            person.name = tbName.Text;
            person.age = (int)numericUpDown1.Value;
            person.gender = string.IsNullOrWhiteSpace(comboBox1.Text) ? "男" : comboBox1.Text;
            person.birthday = dateTimePicker1.Value.ToString("yyyy-MM-dd");
            if (!string.IsNullOrWhiteSpace(path))
                person.photopath = path;
            if ((sender as Button).Text == "添加")
                Service.Container.personService.Insert(person);
            else
                Service.Container.personService.Update(person);
            GC.Collect();
            this.Close();
        }
    }
}
