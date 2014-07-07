using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using System.Collections;

namespace f0rger
{
    public class ToolStripController
    {
        public static ToolStrip toolbar = new ToolStrip();

        public static Hashtable htItems = new Hashtable();

        /// <summary>
        /// Enable/Disable Profiles
        /// </summary>
        /// <param name="enable">is enable</param>
        public static void Set(bool enable)
        {
            foreach (ToolStripItem item in toolbar.Items)
            {
                item.Enabled = enable;
            }
        }

        /// <summary>
        /// Add Profile
        /// </summary>
        /// <param name="name">profile name</param>
        /// <param name="enable">enable</param>
        /// <param name="list">host list</param>
        /// <param name="refresh">refresh host list</param>
        public static void Add(string name, bool enable, string[] list, bool refresh = true)
        {
            List<string> hosts = new List<string>();
            foreach (string host in list)
            {
                hosts.Add(host);
            }

            Add(name, enable, hosts, refresh);
        }

        /// <summary>
        /// Add Profile
        /// </summary>
        /// <param name="name">profile name</param>
        /// <param name="enable">enable</param>
        /// <param name="hosts">host list</param>
        public static void Add(string name, bool enable, List<string> hosts, bool refresh = true)
        {
            if (!htItems.ContainsKey(name))
            {
                ToolStripButton button = new ToolStripButton()
                {
                    DisplayStyle = ToolStripItemDisplayStyle.Text,
                    Text = name,
                    CheckState = CheckState.Checked,
                    Checked = enable
                };
                button.Click += new EventHandler(OnButtonCheckedChange);
                toolbar.Items.Add(button);

                htItems.Add(name, button);
                ProfileService.Add(name, enable, hosts, refresh);
            }
        }

        /// <summary>
        /// Add Batch Profiles
        /// </summary>
        /// <param name="profiles">profile list</param>
        public static void Add(ProfileEntityList profiles)
        {
            toolbar.SuspendLayout();
            foreach (ProfileEntity profile in profiles)
            {
                if (!htItems.ContainsKey(profile.Name))
                {
                    Add(profile.Name, profile.Enable, profile.Hosts, false);
                }
            }
            toolbar.ResumeLayout();

            ProfileService.Refresh();
        }

        public static void Update(string name, bool enable)
        {
            ProfileEntity profile = ProfileService.Get(name);
            if (profile != null)
            {
                profile.Enable = enable;
                Update(name, profile);
            }
        }

        /// <summary>
        /// Update profile
        /// </summary>
        /// <param name="name">old profile name</param>
        /// <param name="profile">new profile</param>
        public static void Update(string name, ProfileEntity profile)
        {
            //if user change profile name ,we can find it
            if (htItems.ContainsKey(name))
            {
                ToolStripButton button = (ToolStripButton)htItems[name];
                button.Checked = profile.Enable;
                button.Text = profile.Name;

                //update hashtable
                htItems.Remove(name);
                htItems.Add(profile.Name, button);

                ProfileService.Update(name, profile);
            }
        }

        /// <summary>
        /// Remove profile by name
        /// </summary>
        /// <param name="name">profile name</param>
        public static void Remove(string name)
        {
            if (htItems.ContainsKey(name))
            {
                ToolStripButton button = (ToolStripButton)htItems[name];
                toolbar.Items.Remove(button);
                htItems.Remove(name);
                ProfileService.Remove(name);
            }
        }

        /// <summary>
        /// Get profile
        /// </summary>
        /// <param name="name">profile name</param>
        /// <returns></returns>
        public static ProfileEntity Get(string name)
        {
            return ProfileService.Get(name);
        }

        static void OnButtonCheckedChange(object sender, EventArgs e)
        {
            ToolStripButton button = (ToolStripButton)sender;

            string name = button.Text;
            bool enable = !button.Checked;

            Update(name, enable);
        }
    }
}
