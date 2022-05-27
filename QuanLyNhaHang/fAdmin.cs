using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
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

namespace QuanLyQuanCafe
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();
        BindingSource accountList = new BindingSource();
        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            LoadAccountList();
            LoadDateTimePickerBill();
            LoadListBillByDate(dtpFromdate.Value, dtpTodate.Value);
            LoadListFood();
            AddFoodBinding();
            AddAccountBinding();
            LoadCategoryIntoCombobox(cbbCategory);
            LoadAccount();
            LoadListFoodSort();
            
        }

        #region methods

        List<Food> SearchFoodByName(string name)
        {
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);
            return listFood;
        }

        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpFromdate.Value = new DateTime(today.Year, today.Month, 1);
            dtpTodate.Value = dtpFromdate.Value.AddMonths(1).AddDays(-1);
        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }

        void AddFoodBinding()
        {
            tbFoodName.DataBindings.Add(new Binding("Text", dtgFood.DataSource, "Name", true, DataSourceUpdateMode.Never));
            tbFoodID.DataBindings.Add(new Binding("Text", dtgFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            nudFoodPrice.DataBindings.Add(new Binding("Value", dtgFood.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }
        #endregion

        #region events

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }


        private void txbPass_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (dtgFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgFood.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category cateogory = CategoryDAO.Instance.GetCategoryByID(id);

                    cbbCategory.SelectedItem = cateogory;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbbCategory.Items)
                    {
                        if (item.ID == cateogory.ID)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }
                    cbbCategory.SelectedIndex = index;
                }
            }
            catch { }
        }

        private void tpMenu_Click(object sender, EventArgs e)
        {

        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {

        }

        void LoadAccountList()
        {
            string query = "exec dbo.USP_GetAccountByUserName @userName";

            dtgAcc.DataSource = DataProvider.Instance.ExecuteQuery(query, new object[] { "Michu" });
        }

        private void dtgAcc_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void fAdmin_Load(object sender, EventArgs e)
        {

        }

        private void tpTable_Click(object sender, EventArgs e)
        {

        }

        private void btSearchFood_Click(object sender, EventArgs e)
        {
            foodList.DataSource = SearchFoodByName(txbSearchFood.Text);
        }

        private void tbFoodName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void btViewbill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpFromdate.Value, dtpTodate.Value);
        }

        private void dtpTodate_ValueChanged(object sender, EventArgs e)
        {

        }

        void LoadListFood()
        {
            dtgFood.DataSource = foodList;
            foodList.DataSource = FoodDAO.Instance.GetListFood();
        }
        private void btViewFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        #endregion

        void AddAccountBinding()
        {
            dtgAcc.DataSource = accountList;
            tbAccName.DataBindings.Add(new Binding("Text", dtgAcc.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            tbAccViewName.DataBindings.Add(new Binding("Text", dtgAcc.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            numericUpDown1.DataBindings.Add(new Binding("Value", dtgAcc.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }

        void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
        }

        private void dtgvBill_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        void AddAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại");
            }

            LoadAccount();
        }

        void EditAccount(string userName, string displayName, int type)
        {
            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Cập nhật tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Cập nhật tài khoản thất bại");
            }

            LoadAccount();
        }

        void DeleteAccount(string userName)
        {
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng đừng xóa chính bạn chứ");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
            }
            else
            {
                MessageBox.Show("Xóa tài khoản thất bại");
            }

            LoadAccount();
        }

        void ResetPass(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thất bại");
            }
        }

        private void btAddAcc_Click(object sender, EventArgs e)
        {
            string userName = tbAccName.Text;
            string displayName = tbAccViewName.Text;
            int type = (int)numericUpDown1.Value;
            AddAccount(userName, displayName, type);
        }

        private void btDelAcc_Click(object sender, EventArgs e)
        {
            string userName = tbAccName.Text;
            DeleteAccount(userName);
        }

        private void btEditAcc_Click(object sender, EventArgs e)
        {
            string userName = tbAccName.Text;
            string displayName = tbAccViewName.Text;
            int type = (int)numericUpDown1.Value;
            EditAccount(userName, displayName, type);
        }

        private void btResetPass_Click(object sender, EventArgs e)
        {
            string userName = tbAccName.Text;
            ResetPass(userName);
        }

        private void btAddFood_Click(object sender, EventArgs e)
        {
            string name = tbFoodName.Text;
            int categoryID = (cbbCategory.SelectedItem as Category).ID;
            float price = (float)nudFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categoryID, price))
            {
                MessageBox.Show("Thêm món thành công");
                LoadListFood();
                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private void btEditFood_Click(object sender, EventArgs e)
        {
            string name = tbFoodName.Text;
            int categoryID = (cbbCategory.SelectedItem as Category).ID;
            float price = (float)nudFoodPrice.Value;
            int id = Convert.ToInt32(tbFoodID.Text);

            if (FoodDAO.Instance.UpdateFood(id, name, categoryID, price))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();
                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }

        private void btDelFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(tbFoodID.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();
                if (deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa thức ăn");
            }
        }

        private void btViewAcc_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }

        private event EventHandler insertFood;
        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadListFoodSort();
        }

        public void LoadListFoodSort()
        {
            List<Food> dt = FoodDAO.Instance.GetListFood();
            for (int i = 0; i < dt.Count() - 1; i++) 
            {
                for (int j = i + 1; j < dt.Count(); j++) 
                {
                    if (dt[j].Price < dt[i].Price)
                    {
                        Food v = dt[j];
                        dt[j] = dt[i];
                        dt[i] = v;
                    }    
                }
            }
            dtgFood.DataSource = dt;
        }
    }
}
