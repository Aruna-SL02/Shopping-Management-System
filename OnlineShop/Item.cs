using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace OnlineShop
{
    public partial class Item : Form
    {
        // 1) Your connection string
        string con = ConfigurationManager.ConnectionStrings["myConStr"].ConnectionString;

        public Item()
        {
            InitializeComponent();
            /*
            // ─────────────────────────────────────────────────────────────────────
            // 2) Make the form open maximized and centered on the screen:
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;

            // 3) If you want your DataGridView to stretch and fill the window:
            dgv_list.Dock = DockStyle.Fill;

            // 4) (Optional) If you have a header panel—e.g. panelTop—dock it to the top:
            // panelTop.Dock = DockStyle.Top;

            // 5) If you have a central panel (e.g. panelContent) that you want always centered,
            //    subscribe to Resize and center it below. If you don’t have such a panel,
            //    you can skip the Resize subscription.
            this.Resize += new EventHandler(Form_Resize);

            // 6) Call it once so that, on startup, your “centered” panel is already in place:
            Form_Resize(this, EventArgs.Empty);
        }

        /// <summary>
        /// If you have a Panel (for example, named panelContent) 
        /// that should stay centered, this will keep it centered whenever
        /// the window size changes. 
        /// 
        /// Replace “panelContent” with your actual panel name (or remove
        /// this entire method if you don’t have a central panel).
        /// </summary>
        private void Form_Resize(object sender, EventArgs e)
        {
            // Example: center “panelContent” horizontally & vertically.
            // If you don’t have panelContent, delete these two lines:
            if (this.Controls.ContainsKey("panelContent"))
            {
                var panelContent = this.Controls["panelContent"];
                panelContent.Left = (this.ClientSize.Width - panelContent.Width) / 2;
                panelContent.Top = (this.ClientSize.Height - panelContent.Height) / 2;
            }*/
        }

        private void LoadGridView()
        {
            MySqlConnection conn = new MySqlConnection(con);

            string cmd = "SELECT Item.Id AS Id, Item.Name AS Item, Category.Name AS Category, SellingPrice, Size, Color, Visible " +
                         "FROM Item, Category " +
                         "WHERE Item.CategoryId = Category.Id " +
                         "ORDER BY Item.Id";

            MySqlDataAdapter da = new MySqlDataAdapter(cmd, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "Category");

            dgv_list.DataSource = ds.Tables["Category"].DefaultView;
        }

        private bool check_item_ifexists(string id)
        {
            MySqlConnection conn = new MySqlConnection(con);
            string selectQuery = "SELECT * FROM Item WHERE Id = '" + id + "'";
            MySqlDataAdapter da = new MySqlDataAdapter(selectQuery, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "Item");

            int row = ds.Tables["Item"].Rows.Count;
            return row > 0;
        }

        private void lbl_back_Click(object sender, EventArgs e)
        {
            Admin admin = new Admin();
            this.Hide();
            admin.Show();
        }

        private void Item_Load(object sender, EventArgs e)
        {
            // Loads Item table on the grid view.
            LoadGridView();

            // Loads all Category in the combo box
            MySqlConnection conn = new MySqlConnection(con);
            string cmd = "SELECT Name FROM Category ORDER BY Id";
            MySqlDataAdapter da = new MySqlDataAdapter(cmd, conn);
            DataSet ds = new DataSet();
            da.Fill(ds, "Category");

            foreach (DataRow row in ds.Tables["Category"].Rows)
            {
                cmb_category.Items.Add(row["Name"].ToString());
            }
        }

        private void dgv_list_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // When a cell is clicked, displays the selected rows on the text boxes.
            txt_name.Text = dgv_list.SelectedRows[0].Cells[1].Value.ToString();
            cmb_category.Text = dgv_list.SelectedRows[0].Cells[2].Value.ToString();
            txt_price.Text = dgv_list.SelectedRows[0].Cells[3].Value.ToString();
            txt_size.Text = dgv_list.SelectedRows[0].Cells[4].Value.ToString();
            txt_color.Text = dgv_list.SelectedRows[0].Cells[5].Value.ToString();

            string visible = dgv_list.SelectedRows[0].Cells[6].Value.ToString();
            string status = (visible == "Yes") ? "Posted" : "Hidden";
            cmb_status.Text = status;
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string id = dgv_list.SelectedRows[0].Cells[0].Value.ToString();
            string name = txt_name.Text;
            string category = (string)cmb_category.SelectedItem;
            string categoryId = (cmb_category.SelectedIndex + 1).ToString();

            if (category == null)
            {
                MessageBox.Show("Please select from the set of categories.", "Incomplete Detail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string price = txt_price.Text;
            string size = txt_size.Text;
            string color = txt_color.Text;
            string posted = (string)cmb_status.SelectedItem;

            if (posted == null)
            {
                MessageBox.Show("Please select from the set of item status.", "Incomplete Detail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string visible = (posted == "Posted") ? "Yes" : "No";

            if (name == "" || price == "")
            {
                MessageBox.Show("Please complete all required fields.", "Incomplete Detail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MySqlConnection conn = new MySqlConnection(con);
            string queryUpdate = "UPDATE Item SET Name='" + name + "', " +
                                 "CategoryId=" + categoryId + "," +
                                 "SellingPrice=" + price + "," +
                                 "Size='" + size + "'," +
                                 "Color='" + color + "'," +
                                 "Visible='" + visible + "'" +
                                 " WHERE Id=" + id;

            conn.Open();
            MySqlCommand cmd = new MySqlCommand(queryUpdate, conn);

            bool id_exists = check_item_ifexists(id);
            if (!id_exists)
            {
                MessageBox.Show("Item Id doesn't exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Item is successfully updated.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception err)
                {
                    MessageBox.Show("Item was not updated successfully: " + err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            conn.Close();
            LoadGridView(); // Refresh the grid
            ClearInputs();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            string name = txt_name.Text;
            string category = (string)cmb_category.SelectedItem;
            string categoryId = (cmb_category.SelectedIndex + 1).ToString();

            if (category == null)
            {
                MessageBox.Show("Please select from the set of categories.", "Incomplete Detail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string price = txt_price.Text;
            string size = txt_size.Text;
            string color = txt_color.Text;
            string posted = (string)cmb_status.SelectedItem;

            if (posted == null)
            {
                MessageBox.Show("Please select from the set of item status.", "Incomplete Detail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string visible = (posted == "Posted") ? "Yes" : "No";

            if (name == "" || price == "")
            {
                MessageBox.Show("Please complete all required fields.", "Incomplete Detail", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MySqlConnection conn = new MySqlConnection(con);
            string queryInsert = "INSERT INTO Item(Name, CategoryId, SellingPrice, Size, Color, Visible) " +
                                 "VALUES('" + name + "', " + categoryId + ", " + price + ", '" + size + "', '" + color + "', '" + visible + "')";

            conn.Open();
            MySqlCommand cmd = new MySqlCommand(queryInsert, conn);

            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Item is successfully added.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Item already exists.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            conn.Close();
            LoadGridView();
            ClearInputs();
        }

        /// <summary>
        /// Helper to clear all textboxes and combo boxes after add/update
        /// </summary>
        private void ClearInputs()
        {
            txt_name.Text = "";
            cmb_category.Text = "";
            txt_size.Text = "";
            txt_color.Text = "";
            txt_price.Text = "";
            cmb_status.Text = "";
        }
    }
}
