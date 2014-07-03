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
        private static ProfileEntityList Profiles;
        /// <summary>
        /// 配置规则哈希表,用于快速索引到配置
        /// </summary>
        private static Hashtable htProfiles;

        /// <summary>
        /// 配置管理服务
        /// </summary>
        static ProfileService()
        {
            Profiles = new ProfileEntityList();
            htProfiles = new Hashtable();
        }

        /// <summary>
        /// 设置关联的配置列表
        /// </summary>
        /// <param name="profiles"></param>
        public static void Use(ProfileEntityList profiles)
        {
            lock (Profiles)
            {
                Profiles = profiles;
                htProfiles = new Hashtable();
                foreach (ProfileEntity profile in Profiles)
                {
                    htProfiles.Add(profile.Name, profile);
                }
            }
        }

        #region 配置规则的增删改
        /// <summary>
        /// 添加规则到数据列表
        /// </summary>
        /// <param name="name">规则名称</param>
        /// <param name="enable">是否启用</param>
        /// <param name="hosts">域名地址列表</param>
        /// <returns></returns>
        public static bool Add(string name, bool enable, string[] hosts)
        {
            List<string> list = ConvertToList(hosts);
            return Add(name, enable, list);
        }
        /// <summary>
        /// 添加规则到数据列表
        /// </summary>
        /// <param name="name">规则名称</param>
        /// <param name="enable">是否启用</param>
        /// <param name="hosts">域名地址列表</param>
        /// <returns></returns>
        public static bool Add(string name, bool enable, List<string> hosts)
        {
            ProfileEntity profile = new ProfileEntity()
            {
                Name = name,
                Enable = enable,
                Hosts = hosts
            };

            return Add(profile);
        }
        /// <summary>
        /// 添加规则到数据列表
        /// </summary>
        /// <param name="profile">规则实体</param>
        /// <returns></returns>
        public static bool Add(ProfileEntity profile)
        {
            if (htProfiles.ContainsKey(profile.Name))
                return false;

            //保存
            htProfiles.Add(profile.Name, profile);
            Profiles.Add(profile);
            return true;
        }

        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="name">需要删除的规则名称</param>
        public static bool Remove(string name)
        {
            if (htProfiles.ContainsKey(name))
            {
                var result = Remove((ProfileEntity)htProfiles[name]); //先删除实际的规则
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
        public static bool Remove(ProfileEntity profile)
        {
            return Profiles.Remove(profile);
        }

        /// <summary>
        /// 设置配置规则
        /// </summary>
        /// <param name="name">规则名称</param>
        /// <param name="enable">是否启用该规则</param>
        public static void Set(string name, bool enable)
        {
            if (htProfiles.ContainsKey(name))
            {
                ProfileEntity profile = (ProfileEntity)htProfiles[name];
                profile.Enable = enable;
            }
        }
        /// <summary>
        /// 设置配置规则
        /// </summary>
        /// <param name="name">规则名称</param>
        /// <param name="enable">是否启用该规则</param>
        /// <param name="hosts">域名地址列表</param>
        public static void Set(string name, bool enable, string[] hosts)
        {
            if (htProfiles.ContainsKey(name))
            {
                ProfileEntity profile = (ProfileEntity)htProfiles[name];
                profile.Enable = enable;
                profile.Hosts = ConvertToList(hosts);
            }
        }
        /// <summary>
        /// 设置配置规则
        /// </summary>
        /// <param name="name">规则名称</param>
        /// <param name="enable">是否启用该规则</param>
        /// <param name="hosts">域名地址列表</param>
        public static void Set(string name, bool enable, List<string> hosts)
        {
            if (htProfiles.ContainsKey(name))
            {
                ProfileEntity profile = (ProfileEntity)htProfiles[name];
                profile.Enable = enable;
                profile.Hosts = hosts;
            }
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

        /// <summary>
        /// 获取配置规则实体
        /// </summary>
        /// <param name="name">需要获取的配置规则名称</param>
        /// <returns>规则实体</returns>
        private static ProfileEntity Get(string name)
        {
            if (htProfiles.ContainsKey(name))
            {
                ProfileEntity profile = (ProfileEntity)htProfiles[name];
                return profile;
            }
            return null;
        }
        #endregion
    }
}
