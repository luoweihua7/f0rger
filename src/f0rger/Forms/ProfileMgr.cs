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
        private string current = null;
        private int index = -1;
        private List<string> profiles = new List<string>(); //快速索引,检查是否存在于列表

        public ProfileMgr()
        {
            InitializeComponent();
        }

        private void ProfileMgr_Load(object sender, EventArgs e)
        {
            foreach (ProfileEntity profile in Configs.Profiles)
            {
                lbProfiles.Items.Add(profile.Name);
                profiles.Add(profile.Name);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearSelected();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int selectedIndex = lbProfiles.SelectedIndex;
            if (selectedIndex >= 0)
            {
                ToolStripController.Remove(lbProfiles.SelectedItem.ToString()); //remove entity first
                lbProfiles.Items.RemoveAt(selectedIndex); //remove view
                profiles.RemoveAt(selectedIndex); //remove from templete list

                //select same line or pre line or none
                lbProfiles.SelectedIndex = -1;
                if (lbProfiles.Items.Count > selectedIndex) //remind: count=index+1
                {
                    //select same index
                    lbProfiles.SelectedIndex = selectedIndex;
                }
                else
                {
                    if (selectedIndex - 1 >= 0)
                    {
                        //select pre index
                        lbProfiles.SelectedIndex = selectedIndex - 1;
                    }
                    else
                    {
                        //no profile
                        ClearSelected();
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var enable = this.cbEnable.Checked;
            var name = this.tbxName.Text.Trim();
            var hosts = this.tbxHosts.Text.Trim().Replace(" ", "").Replace("\r\n", ",").Replace(";", ",");

            if (string.IsNullOrEmpty(name))
            {
                this.tbxName.Focus();
                return;
            }

            if (string.IsNullOrEmpty(hosts))
            {
                this.tbxHosts.Focus();
                return;
            }

            if (name != current && profiles.Contains(name))
            {
                MessageBox.Show("Profile name exists");
                return;
            }

            List<string> list = new List<string>();
            foreach (string host in hosts.Split(new char[] { ',' }))
            {
                list.Add(host);
            }

            //add or update
            if (string.IsNullOrEmpty(current))
            {
                //add
                lbProfiles.Items.Add(name);
                profiles.Add(name);

                ToolStripController.Add(name, enable, list);

                ClearSelected();
            }
            else
            {
                //update
                lbProfiles.Items[index] = name;
                profiles[index] = name;

                ToolStripController.Update(current, new ProfileEntity()
                {
                    Name = name,
                    Enable = enable,
                    Hosts = list
                });

                lbProfiles.SelectedIndex = -1;
                lbProfiles.SelectedIndex = index; //select again
            }
        }

        private void lbProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbProfiles.SelectedItem != null)
            {
                ProfileEntity profile = ToolStripController.Get(lbProfiles.SelectedItem.ToString());
                if (profile != null)
                {
                    string hosts = string.Empty;
                    foreach (var host in profile.Hosts)
                    {
                        hosts += host + Environment.NewLine;
                    }

                    this.cbEnable.Checked = profile.Enable;
                    this.tbxName.Text = profile.Name;
                    this.tbxHosts.Text = hosts;

                    //保存编辑的位置和名称(名称可以被修改)
                    current = lbProfiles.SelectedItem.ToString();
                    index = lbProfiles.SelectedIndex;
                }
            }
        }

        void ClearSelected()
        {
            current = string.Empty;
            index = -1;

            this.cbEnable.Checked = true;
            this.tbxName.Text = string.Empty;
            this.tbxHosts.Text = string.Empty;
        }
    }
}
