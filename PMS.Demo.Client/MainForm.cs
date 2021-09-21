using PMS.Demo.Entity;
using PMS.Demo.Runtime;
using PMS.Demo.Service;
using PMS.Demo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PMS.Demo.Client
{
    public partial class MainForm : Form
    {
        private FrmAddPerson addPerson = null;

        public MainForm()
        {
            InitializeComponent();
        }

        private void RefreshDataGrid()
        {
            dgvPersonTable.DataSource = Service.Container.personService.QueryList();
            dgvPersonTable.Columns["id"].Visible = false;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            //添加测试数据
            //Person p1 = new Person() { id = Common.GetGUID(), name = "张三", age = 25, gender = "男", birthday = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), photopath = "1" };
            //Person p2 = new Person() { id = Common.GetGUID(), name = "张四", age = 25, gender = "男", birthday = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), photopath = "1" };
            //Person p3 = new Person() { id = Common.GetGUID(), name = "张五", age = 25, gender = "男", birthday = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), photopath = "1" };
            //Person p4 = new Person() { id = Common.GetGUID(), name = "张六", age = 25, gender = "男", birthday = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), photopath = "1" };
            //Person p5 = new Person() { id = Common.GetGUID(), name = "张七", age = 25, gender = "男", birthday = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), photopath = "1" };

            //Service.Container.personService.Insert(p1);
            //Service.Container.personService.Insert(p2);
            //Service.Container.personService.Insert(p3);
            //Service.Container.personService.Insert(p4);
            //Service.Container.personService.Insert(p5);

            RefreshDataGrid();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (addPerson != null)
            {
                MessageBox.Show("正在进行其他操作，请勿删除记录！");
                return;
            }
            List<string> lstPersonIds = new List<string>();
            foreach (DataGridViewRow item in dgvPersonTable.SelectedRows)
            {
                lstPersonIds.Add(item.Cells["id"].Value.ToString());
            }
            if (lstPersonIds.Count > 0)
            {
                foreach (var item in lstPersonIds)
                {
                    Service.Container.personService.Delete(item);
                }
            }
            RefreshDataGrid();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            if (addPerson != null)
                return;
            addPerson = new FrmAddPerson();
            addPerson.FormClosed += new FormClosedEventHandler(AfterMethod);
            addPerson.Show();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (addPerson != null)
                return;
            if (dgvPersonTable.SelectedRows.Count > 1)
            {
                MessageBox.Show("请选择一条记录");
                return;
            }
            addPerson = new FrmAddPerson(Service.Container.personService.Query(dgvPersonTable.SelectedRows[0].Cells["id"].Value.ToString()));
            addPerson.FormClosed += new FormClosedEventHandler(AfterMethod);
            addPerson.Show();
        }

        private void AfterMethod(object sender, FormClosedEventArgs e)
        {
            addPerson = null; 
            RefreshDataGrid();
        }

        private void dgvPersonTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            btnUpdate.PerformClick();
        }
    }
}
