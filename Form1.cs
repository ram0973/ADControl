using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.DirectoryServices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ADcontrol
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DirectoryEntry adEntry = ActiveDirectoryServices.CreateDirectoryEntry();
            List<string> computers = ActiveDirectoryServices.GetADComputersNames(adEntry);
            dataGridView1.ColumnCount = 1;
            dataGridView1.Columns[0].Name = "Computer name";
            
            foreach (string computer in computers)
            {
                dataGridView1.Rows.Add(computer);
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_MouseClick (object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = dataGridView1.HitTest(e.X, e.Y).RowIndex;

                if (currentMouseOverRow >= 0)
                {
                    
                }

                AppMenus.gridContextMenu.Show(dataGridView1, new Point(e.X, e.Y));
            }
        }
    }
}
