using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class Config
    {
        public const string SOURCE = "Source";
        public const string TARGET = "Target";
        public const string SECRET_ID = "SecretId";
        public const string SECRET_KEY = "SecretKey";
        public const string PROJECT_ID = "ProjectId";

        public const string DEFAULT_SOURCE = "auto";
        public const string DEFAULT_TARGET = "zh";
        public const string DEFAULT_SECRET_ID = "123";
        public const string DEFAULT_SECRET_KEY = "123";
        public const string DEFAULT_PROJECT_ID = "0";
        public string DEFAULT_PATH = Directory.GetCurrentDirectory() + @"\translate.ini";
        private Dictionary<string, string> configMap = new Dictionary<string, string>();
        public Config()
        {
            configMap.Add(SECRET_ID, DEFAULT_SECRET_ID);
            configMap.Add(SECRET_KEY, DEFAULT_SECRET_KEY);
            configMap.Add(PROJECT_ID, DEFAULT_PROJECT_ID);
            configMap.Add(SOURCE, DEFAULT_SOURCE);
            configMap.Add(TARGET, DEFAULT_TARGET);
            loadHotKey(HotKeySettingsManager.Instance.LoadDefaultHotKey());
            try
            {
                // 创建一个 StreamReader 的实例来读取文件 
                // using 语句也能关闭 StreamReader
                using (StreamReader sr = new StreamReader(DEFAULT_PATH))
                {
                    string line;

                    // 从文件读取并显示行，直到文件的末尾 
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] cons = line.Split('=');
                        if (cons.Length == 2)
                        {
                            string k = cons[0].Trim();
                            string v = cons[1].Trim();
                            configMap[k] = v;
                        }
                    }
                }
            }
            catch (Exception)
            {
                save();
            }
        }
        public void loadHotKey(ObservableCollection<HotKeyModel> hotkeys)
        {
            for(int i = 0; i < hotkeys.Count; i++)
            {
                var x = hotkeys[i];
                if (configMap.ContainsKey(x.Name))
                {
                    configMap[x.Name] = x.IsUsable.ToString() + " " + x.IsSelectCtrl.ToString() + " " + x.IsSelectShift + " " + x.IsSelectAlt + " " + x.SelectKey;
                }
                else
                {
                    configMap.Add(x.Name, x.IsUsable.ToString() + " " + x.IsSelectCtrl.ToString() + " " + x.IsSelectShift + " " + x.IsSelectAlt + " " + x.SelectKey);
                }
            }
        }
        public ObservableCollection<HotKeyModel> getHotKey() {
            var result = HotKeySettingsManager.Instance.LoadDefaultHotKey();
            if (configMap.ContainsKey(EHotKeySetting.去除换行.ToString()))
            {
                string[] t = configMap[EHotKeySetting.去除换行.ToString()].Split(' ');
                if (t.Length == 5)
                {
                    try
                    {
                        bool isUsable = Boolean.Parse(t[0]);
                        bool isSelectCtrl = Boolean.Parse(t[1]);
                        bool isSelectShift = Boolean.Parse(t[2]);
                        bool isSelectAlt = Boolean.Parse(t[3]);
                        EKey key = (EKey)Enum.Parse(typeof(EKey), t[4]);
                        result[0] = new HotKeyModel { Name = EHotKeySetting.去除换行.ToString(), IsUsable = isUsable, IsSelectCtrl = isSelectCtrl, IsSelectShift = isSelectShift, IsSelectAlt = isSelectAlt, SelectKey = key };
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            if (configMap.ContainsKey(EHotKeySetting.文字翻译.ToString()))
            {
                string[] t = configMap[EHotKeySetting.文字翻译.ToString()].Split(' ');
                if (t.Length == 5)
                {
                    try
                    {
                        bool isUsable = Boolean.Parse(t[0]);
                        bool isSelectCtrl = Boolean.Parse(t[1]);
                        bool isSelectShift = Boolean.Parse(t[2]);
                        bool isSelectAlt = Boolean.Parse(t[3]);
                        EKey key = (EKey)Enum.Parse(typeof(EKey), t[4]);
                        result[1] = new HotKeyModel { Name = EHotKeySetting.文字翻译.ToString(), IsUsable = isUsable, IsSelectCtrl = isSelectCtrl, IsSelectShift = isSelectShift, IsSelectAlt = isSelectAlt, SelectKey = key };
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            if (configMap.ContainsKey(EHotKeySetting.图片翻译.ToString()))
            {
                string[] t = configMap[EHotKeySetting.图片翻译.ToString()].Split(' ');
                if (t.Length == 5)
                {
                    try
                    {
                        bool isUsable = Boolean.Parse(t[0]);
                        bool isSelectCtrl = Boolean.Parse(t[1]);
                        bool isSelectShift = Boolean.Parse(t[2]);
                        bool isSelectAlt = Boolean.Parse(t[3]);
                        EKey key = (EKey)Enum.Parse(typeof(EKey), t[4]);
                        result[2] = new HotKeyModel { Name = EHotKeySetting.图片翻译.ToString(), IsUsable = isUsable, IsSelectCtrl = isSelectCtrl, IsSelectShift = isSelectShift, IsSelectAlt = isSelectAlt, SelectKey = key };
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            if (configMap.ContainsKey(EHotKeySetting.换行翻译.ToString()))
            {
                string[] t = configMap[EHotKeySetting.换行翻译.ToString()].Split(' ');
                if (t.Length == 5)
                {
                    try
                    {
                        bool isUsable = Boolean.Parse(t[0]);
                        bool isSelectCtrl = Boolean.Parse(t[1]);
                        bool isSelectShift = Boolean.Parse(t[2]);
                        bool isSelectAlt = Boolean.Parse(t[3]);
                        EKey key = (EKey)Enum.Parse(typeof(EKey), t[4]);
                        result[3] = new HotKeyModel { Name = EHotKeySetting.换行翻译.ToString(), IsUsable = isUsable, IsSelectCtrl = isSelectCtrl, IsSelectShift = isSelectShift, IsSelectAlt = isSelectAlt, SelectKey = key };
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            if (configMap.ContainsKey(EHotKeySetting.插件锁定.ToString()))
            {
                string[] t = configMap[EHotKeySetting.插件锁定.ToString()].Split(' ');
                if (t.Length == 5)
                {
                    try
                    {
                        bool isUsable = Boolean.Parse(t[0]);
                        bool isSelectCtrl = Boolean.Parse(t[1]);
                        bool isSelectShift = Boolean.Parse(t[2]);
                        bool isSelectAlt = Boolean.Parse(t[3]);
                        EKey key = (EKey)Enum.Parse(typeof(EKey), t[4]);
                        result[4] = new HotKeyModel { Name = EHotKeySetting.插件锁定.ToString(), IsUsable = isUsable, IsSelectCtrl = isSelectCtrl, IsSelectShift = isSelectShift, IsSelectAlt = isSelectAlt, SelectKey = key };
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            if (configMap.ContainsKey(EHotKeySetting.插件顶层.ToString()))
            {
                string[] t = configMap[EHotKeySetting.插件顶层.ToString()].Split(' ');
                if (t.Length == 5)
                {
                    try
                    {
                        bool isUsable = Boolean.Parse(t[0]);
                        bool isSelectCtrl = Boolean.Parse(t[1]);
                        bool isSelectShift = Boolean.Parse(t[2]);
                        bool isSelectAlt = Boolean.Parse(t[3]);
                        EKey key = (EKey)Enum.Parse(typeof(EKey), t[4]);
                        result[5] = new HotKeyModel { Name = EHotKeySetting.插件顶层.ToString(), IsUsable = isUsable, IsSelectCtrl = isSelectCtrl, IsSelectShift = isSelectShift, IsSelectAlt = isSelectAlt, SelectKey = key };
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return result;
        }
        public string getConfig(string key)
        {
            return configMap[key];
        }
        public void setConfig(string key,string value)
        {
            configMap[key] = value;
        }
        public void save()
        {
            StreamWriter writer = File.CreateText(DEFAULT_PATH);
            foreach (var item in configMap)
            {
                writer.WriteLine(item.Key + "=" + item.Value);
            }
            writer.Flush();
            writer.Close();

        }
    }
}
