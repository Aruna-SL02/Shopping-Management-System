using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineShop
{
    public partial class Admin : Form
    {
        public Admin()
        {
            InitializeComponent();

            // Assign hover events for clickable labels
            AddHoverEffect(lbl_login);
            AddHoverEffect(lbl_category1);
            AddHoverEffect(lbl_category2);
            AddHoverEffect(lbl_item1);
            AddHoverEffect(lbl_item2);
            AddHoverEffect(lbl_order1);
            AddHoverEffect(lbl_order2);
            AddHoverEffect(lbl_report1);
            AddHoverEffect(lbl_report2);
        }

        // DLL Imports to move borderless form
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        // Call this from any panel you want to drag the form with
        private void pnl_top_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        // Hover effect handler
        private void AddHoverEffect(Control ctrl)
        {
            ctrl.MouseEnter += (s, e) => { ctrl.ForeColor = Color.Blue; };
            ctrl.MouseLeave += (s, e) => { ctrl.ForeColor = Color.Black; };
            ctrl.Cursor = Cursors.Hand;
        }

        private void lbl_login_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            this.Hide();
            login.Show();
        }

        private void pnl_category_Click(object sender, EventArgs e)
        {
            Category category = new Category();
            this.Hide();
            category.Show();
        }

        private void lbl_category1_Click(object sender, EventArgs e)
        {
            Category category = new Category();
            this.Hide();
            category.Show();
        }

        private void lbl_category2_Click(object sender, EventArgs e)
        {
            Category category = new Category();
            this.Hide();
            category.Show();
        }

        private void pnl_item_Click(object sender, EventArgs e)
        {
            Item item = new Item();
            this.Hide();
            item.Show();
        }

        private void lbl_item1_Click(object sender, EventArgs e)
        {
            Item item = new Item();
            this.Hide();
            item.Show();
        }

        private void lbl_item2_Click(object sender, EventArgs e)
        {
            Item item = new Item();
            this.Hide();
            item.Show();
        }

        private void pnl_order_Click(object sender, EventArgs e)
        {
            Order order = new Order();
            this.Hide();
            order.Show();
        }

        private void lbl_order1_Click(object sender, EventArgs e)
        {
            Order order = new Order();
            this.Hide();
            order.Show();
        }

        private void lbl_order2_Click(object sender, EventArgs e)
        {
            Order order = new Order();
            this.Hide();
            order.Show();
        }

        private void pnl_report_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            report.Show();
        }

        private void lbl_report1_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            report.Show();
        }

        private void lbl_report2_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            report.Show();
        }
    }
}
