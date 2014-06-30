using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace f0rger
{
    /// <summary>
    /// 配置管理类
    /// <para>用于配置的读取和保存</para>
    /// </summary>
    public class ConfigService
    {
        static BinaryFormatter formatter = new BinaryFormatter();

        public static ConfigEntity Read()
        {
            ConfigEntity config = new ConfigEntity();

            if (File.Exists(Configs.ConfigPath))
            {
                FileStream fs = new FileStream(Configs.ConfigPath, FileMode.Open);
                try
                {
                    config = (ConfigEntity)formatter.Deserialize(fs);
                }
                catch (Exception err)
                {
                    LogService.Log(err.Message);
                }
                finally
                {
                    fs.Flush();
                    fs.Close();
                    fs.Dispose();
                }
            }

            return config;
        }

        public static bool Save(ConfigEntity config)
        {
            var result = false;
            FileStream fs = new FileStream(Configs.ConfigPath, FileMode.Create); //创建or覆盖
            try
            {
                formatter.Serialize(fs, config);
                result = true;
            }
            catch (Exception err)
            {
                LogService.Log(err.Message);
            }
            finally
            {
                fs.Flush();
                fs.Close();
                fs.Dispose();
            }
            return result;
        }
    }
}
