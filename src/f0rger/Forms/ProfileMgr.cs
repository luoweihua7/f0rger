using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace f0rger
{
    public partial class ProfileMgr : Form
    {
        private string currentProfile = null;
        public ProfileMgr()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.tbxName.Text = "";
            this.tbxHosts.Text = "";

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lbProfiles.SelectedIndices.Count > 0)
            {
                foreach (var item in lbProfiles.SelectedIndices)
                {
                    lbProfiles.Items.Remove(item);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var name = this.tbxName.Text.Trim();
            var hosts = this.tbxHosts.Text.Trim()
                .Replace("\r\n", "")
                .Replace(" ", "");

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(hosts))
            {
                return;
            }


        }
    }
}
