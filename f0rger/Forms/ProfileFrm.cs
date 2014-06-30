using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace f0rger
{
    public partial class ProfileFrm : Form
    {
        public Form parent;
        public ProfileFrm()
        {
            InitializeComponent();

            parent = (Form)this.Owner;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }
    }
}
