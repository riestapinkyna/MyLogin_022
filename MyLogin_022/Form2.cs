using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyLogin_022
{
    public partial class Form2 : Form
    {
        SqlConnection con = new SqlConnection("integrated security=true; data source=.;initial catalog=MyPractice");
        public Form2()
        {
            InitializeComponent();
        }
        DataClasses1DataContext db = new DataClasses1DataContext();



        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select isnull(max (cast (ID as int)),0) +1 from TB_M_PRODUCT", con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                txtID.Text = dt.Rows[0][0].ToString();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

       void LoadData()
        {
            try
            {
                var st = from tb in db.TB_M_PRODUCTs select tb;
                dataGridView1.DataSource = st;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var st = from s in db.TB_M_PRODUCTs where s.itemName == txtSearchItem.Text || s.design == txtSearchDesign.Text select s;
                dataGridView1.DataSource = st;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int id = int.Parse(txtID.Text);
                string item = txtName.Text;
                string design = txtDesign.Text;
                string color = cbColor.Text;
                DateTime expiredDate = DateTime.Parse(dateTimePicker1.Text);

                var data = new TB_M_PRODUCT
                {
                    ID = id,
                    itemName = item,
                    color = color,
                    design = design,
                    expiredDate = expiredDate
                };
                db.TB_M_PRODUCTs.InsertOnSubmit(data);
                db.SubmitChanges();
                MessageBox.Show("Save Successfully");
                txtDesign.Clear();
                txtName.Clear();
                cbColor.Items.Clear();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string item = txtName.Text;
                string design = txtDesign.Text;
                string color = cbColor.Text;
                DateTime expiredDate = DateTime.Parse(dateTimePicker1.Text);

                var st = (from s in db.TB_M_PRODUCTs where s.ID == int.Parse(txtID.Text) select s).First();

                st.itemName = item;
                st.color = color;
                st.design = design;
                st.expiredDate = expiredDate;
                db.SubmitChanges();



                MessageBox.Show("Update Succesfuly");
                txtDesign.Clear();
                txtName.Clear();
                cbColor.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                var delete = from s in db.TB_M_PRODUCTs where s.itemName == txtSearchItem.Text select s;
                foreach (var t in delete)
                {
                    db.TB_M_PRODUCTs.DeleteOnSubmit(t);
                }
                db.SubmitChanges();
                MessageBox.Show("Delete Succesfully");
                txtDesign.Clear();
                txtSearchItem.Clear();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error" + ex);
            }
        }
    }
}
