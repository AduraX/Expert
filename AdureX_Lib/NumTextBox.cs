using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AdureX_Lib
{
    public partial class NumTextBox : TextBox
    {
        public NumTextBox()
        {
            InitializeComponent();
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if ((this.Text.Contains('-') == true) && this.SelectionStart == 0) e.Handled = true;
            if (Char.IsDigit(e.KeyChar)) return;
            if (Char.IsControl(e.KeyChar)) return;
            if ((e.KeyChar == '.') && (this.Text.Contains('.') == false) && type != "int") return;
            if ((e.KeyChar == '-') && (this.Text.Contains('-') == false))
            {
                this.Text = '-' + this.Text;
                this.SelectionLength = 0;
                this.Select(1, 0);
            }
            e.Handled = true;
        }
        private string type = "int";
        [TypeConverter(typeof(NumType))]
        public string Type
        {
            get { return type; }
           set { type = value; }
        }
        public float NumValue
        {
            get {return float.Parse(this.Text); }
        }
    }
}

//^^^^^^^^^^^^Angle^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
public class NumType : StringConverter
{
    public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
    {
        return true;
    }
    public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
    {
        return new StandardValuesCollection(new string[] { "int", "flaot", "double", "decimal" });
    }
}