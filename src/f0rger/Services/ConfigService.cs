using System;
using System.Collections.Generic;
using System.Text;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;

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
                    formatter.Binder = new ConfigBinder(); //反序列化的时候需要有一个转换器
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

            if (!Directory.Exists(Configs.ConfigDir))
            {
                Directory.CreateDirectory(Configs.ConfigDir);
            }

            FileStream fs = new FileStream(Configs.ConfigPath, FileMode.Create); //创建or覆盖
            try
            {
                formatter.Serialize(fs, config);
                result = true;
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show(err.Message);
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

    public class ConfigBinder : SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            return ass.GetType(typeName);
        }
    } 
}
