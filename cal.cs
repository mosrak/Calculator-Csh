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

namespace Calculator
{
    public partial class cal : Form
    {
        decimal fstNum, secNum = 0.0m;
        string result;
        string operation = "";

        [DllImport("dwmapi", PreserveSig = false)]
        static extern void DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, in bool pvAttribute, int cbAttribute);

        protected override void OnHandleCreated(EventArgs e)
        {
            const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
            DwmSetWindowAttribute(Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, true, Marshal.SizeOf<bool>());
        }

        public cal()
        {
            InitializeComponent();
            CustomizeUI();
        }

        private void CustomizeUI()
        {
            // Apply deep black background
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.Font = new Font("Exo 2", 16, FontStyle.Bold);

            txtBox.BackColor = Color.Black;
            txtBox.ForeColor = Color.Cyan;
            txtBox.Font = new Font("Orbitron", 24, FontStyle.Bold);
            txtBox.TextAlign = HorizontalAlignment.Right;
            txtBox.BorderStyle = BorderStyle.FixedSingle;

            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    btn.BackColor = Color.Black;
                    btn.ForeColor = Color.Magenta;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.Font = new Font("Orbitron", 18, FontStyle.Bold);
                    btn.FlatAppearance.BorderSize = 0;
                    btn.Cursor = Cursors.Hand;
                    btn.Size = new Size(80, 80);

                    // Neon glow effect
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 0, 150);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(255, 50, 200);
                }
            }
        }


        private void BtnDot_Click(object sender, EventArgs e)
        {
            if (!txtBox.Text.Contains("."))
            {
                txtBox.Text += ".";
            }
        }

        private void BtnNums_Click(object sender, EventArgs e)
        {
            if (txtBox.Text == result)
                txtBox.Text = "0";

            btnPlus.Enabled = true;
            btnMinus.Enabled = true;
            btnMultiply.Enabled = true;
            btnDivide.Enabled = true;

            Button button = (Button)sender;
            if (txtBox.Text == "0")
            {
                txtBox.Text = button.Text;
            }
            else
            {
                txtBox.Text += button.Text;
            }
        }

        private void Operation_Click(object sender, EventArgs e)
        {
            if (txtBox.Text == "Infinity" || txtBox.Text == "∞")
            {
                txtBox.Text = "0";
                MessageBox.Show("Cannot operate with infinity");
                btnC.PerformClick();
            }
            else
            {
                try
                {
                    btnEquals.PerformClick();

                    Button button = (Button)sender;
                    fstNum = decimal.Parse(txtBox.Text);
                    operation = button.Text;
                    txtBox.Text = "0";

                    txtDis.Text = fstNum.ToString() + " " + operation;

                    btnPlus.Enabled = false;
                    btnMinus.Enabled = false;
                    btnMultiply.Enabled = false;
                    btnDivide.Enabled = false;
                }
                catch (OverflowException)
                {
                    MessageBox.Show("The number is too long.");
                }
            }
        }

        private void BtnEquals_Click(object sender, EventArgs e)
        {
            secNum = decimal.Parse(txtBox.Text);
            txtDis.Text = $"{txtDis.Text} {txtBox.Text} =";

            btnPlus.Enabled = true;
            btnMinus.Enabled = true;
            btnMultiply.Enabled = true;
            btnDivide.Enabled = true;

            try
            {
                switch (operation)
                {
                    case "+":
                        result = txtBox.Text = (fstNum + secNum).ToString();
                        break;
                    case "−":
                        result = txtBox.Text = (fstNum - secNum).ToString();
                        break;
                    case "×":
                        result = txtBox.Text = (fstNum * secNum).ToString();
                        break;
                    case "÷":
                        try
                        {
                            result = txtBox.Text = (fstNum / secNum).ToString();
                        }
                        catch (DivideByZeroException)
                        {
                            result = txtBox.Text = "Infinity";
                        }
                        break;

                    default:
                        txtDis.Text = $"{txtBox.Text} =";
                        break;
                }
            }
            catch (OverflowException)
            {
                MessageBox.Show("The number is too long.");
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (txtBox.Text.Length > 0)
            {
                txtBox.Text = txtBox.Text.Remove(txtBox.Text.Length - 1, 1);
            }
            if (txtBox.Text == "")
                txtBox.Text = "0";
        }
        private void BtnC_Click(object sender, EventArgs e)
        {
            txtBox.Text = "0"; // Reset the text box or perform any required action
        }


        private void BtnCE_Click(object sender, EventArgs e)
        {
            txtBox.Text = "0";
        }

    



        private void BtnMP_Click(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtBox.Text, out decimal number))
            {
                number *= -1; // Toggle the sign
                txtBox.Text = number.ToString();
            }
        }
        private void BtnOperation_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            operation = button.Text;
            switch (operation)
            {
                case "√x":
                    txtDis.Text = $"√({txtBox.Text})";
                    result = txtBox.Text = Convert.ToString(Math.Sqrt(Convert.ToDouble(txtBox.Text)));
                    break;
                case "^2":
                    txtDis.Text = $"({txtBox.Text})^2";
                    result = txtBox.Text = Convert.ToString(Convert.ToDouble(txtBox.Text) * (Convert.ToDouble(txtBox.Text)));
                    break;
                case "¹/x":
                    txtDis.Text = $"¹/({txtBox.Text})";
                    result = txtBox.Text = Convert.ToString(1.0 / Convert.ToDouble(txtBox.Text));
                    break;
                case "%":
                    txtDis.Text = $"%({txtBox.Text})";
                    result = txtBox.Text = Convert.ToString(Convert.ToDouble(txtBox.Text) / 100);
                    break;

                default:
                    break;
            }
        }
    }
}

