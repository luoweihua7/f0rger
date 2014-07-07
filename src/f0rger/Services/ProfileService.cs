using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;

namespace f0rger
{
    /// <summary>
    /// 配置管理类
    /// <para>实现不同站点按需挂载</para>
    /// </summary>
    public class ProfileService
    {
        /// <summary>
        /// 配置规则列表
        /// </summary>
        public static ProfileEntityList Profiles = Configs.Profiles;

        /// <summary>
        /// 需要处理的Host地址
        /// </summary>
        private static List<string> htHostList = new List<string>();

        /// <summary>
        /// 配置规则哈希表,用于快速索引到配置
        /// </summary>
        private static Hashtable htProfiles = new Hashtable();

        #region 配置规则的增删改

        public static ProfileEntity Add(string name, bool enable, string[] hosts, bool refresh = true)
        {
            List<string> list = new List<string>();
            foreach (string host in hosts)
            {
                list.Add(host);
            }
            return Add(name, enable, list, refresh);
        }

        /// <summary>
        /// 添加规则到数据列表
        /// </summary>
        /// <param name="name">规则名称</param>
        /// <param name="enable">是否启用</param>
        /// <param name="hosts">域名地址列表</param>
        /// <returns></returns>
        public static ProfileEntity Add(string name, bool enable, List<string> hosts, bool refresh = true)
        {
            ProfileEntity profile = new ProfileEntity()
            {
                Name = name,
                Enable = enable,
                Hosts = hosts
            };

            return Add(profile, refresh);
        }
        /// <summary>
        /// 添加规则到数据列表
        /// </summary>
        /// <param name="profile">规则实体</param>
        /// <returns></returns>
        public static ProfileEntity Add(ProfileEntity profile, bool refresh = true)
        {
            if (htProfiles.ContainsKey(profile.Name))
                return null;

            //保存
            htProfiles.Add(profile.Name, profile);
            Profiles.Add(profile);

            if (refresh)
            {
                Refresh();
            }

            return profile;
        }

        public static void Update(string name, ProfileEntity profile)
        {
            if (htProfiles.ContainsKey(name))
            {
                ProfileEntity entity = (ProfileEntity)htProfiles[name];

                //修改实体,实际是修改Profiles下的实体(引用类型)
                entity.Name = profile.Name;
                entity.Enable = profile.Enable;
                entity.Hosts = profile.Hosts;

                htProfiles.Remove(name);
                htProfiles.Add(profile.Name, entity);

                Refresh();
            }
        }

        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="name">需要删除的规则名称</param>
        public static bool Remove(string name, bool refresh = true)
        {
            if (htProfiles.ContainsKey(name))
            {
                var result = Remove((ProfileEntity)htProfiles[name], refresh); //先删除实际的规则
                if (result)
                {
                    htProfiles.Remove(name); //然后再删除哈希表
                }

                return result;
            }

            return false;
        }
        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="profile">需要删除的规则实体</param>
        public static bool Remove(ProfileEntity profile, bool refresh = false)
        {
            bool result = Profiles.Remove(profile);
            if (refresh)
            {
                Refresh();
            }
            return result;
        }

        /// <summary>
        /// 获取配置规则实体
        /// </summary>
        /// <param name="name">需要获取的配置规则名称</param>
        /// <returns>规则实体</returns>
        public static ProfileEntity Get(string name)
        {
            if (htProfiles.ContainsKey(name))
            {
                ProfileEntity profile = (ProfileEntity)htProfiles[name];
                return profile;
            }
            return null;
        }

        /// <summary>
        /// 刷新配置列表中需要挂载的域名
        /// </summary>
        public static void Refresh()
        {
            lock (htHostList)
            {
                htHostList.Clear();
                foreach (ProfileEntity profile in Profiles)
                {
                    if (profile.Enable)
                    {
                        //htHostList.AddRange(profile.Hosts);

                        //不考虑性能了,排重
                        foreach (string host in profile.Hosts)
                        {
                            if (!htHostList.Contains(host))
                            {
                                htHostList.Add(host);
                            }
                        }
                    }
                }
            }
            LogService.Log("profile list refresh, match " + htHostList.Count + " hosts");
        }

        /// <summary>
        /// 是否匹配到Host
        /// </summary>
        /// <param name="host">域名地址</param>
        /// <returns></returns>
        public static bool MatchHost(string host)
        {
            return htHostList.Contains(host);
        }

        /// <summary>
        /// 数组转换方法
        /// <para>将string[]数组转换成List&lt;string&gt;列表</para>
        /// </summary>
        /// <param name="strs">需要转换的string[]数组</param>
        /// <returns></returns>
        private static List<string> ConvertToList(string[] strs)
        {
            List<string> list = new List<string>();
            foreach (string str in strs)
            {
                list.Add(str);
            }
            return list;
        }
        #endregion
    }
}
