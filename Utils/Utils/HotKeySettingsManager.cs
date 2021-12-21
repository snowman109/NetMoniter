using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace Utils
{
    /// <summary>
    /// 快捷键设置管理器
    /// </summary>
    public class HotKeySettingsManager
    {
        private static HotKeySettingsManager m_Instance;
        /// <summary>
        /// 单例实例
        /// </summary>
        public static HotKeySettingsManager Instance
        {
            get { return m_Instance ?? (m_Instance = new HotKeySettingsManager()); }
        }

        /// <summary>
        /// 加载默认快捷键
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<HotKeyModel> LoadDefaultHotKey()
        {
            var hotKeyList = new ObservableCollection<HotKeyModel>();
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.去除换行.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = false, IsSelectShift = false, SelectKey = EKey.P });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.文字翻译.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.E });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.图片翻译.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.Q });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.换行翻译.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.R });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.插件锁定.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.L });
            hotKeyList.Add(new HotKeyModel { Name = EHotKeySetting.插件顶层.ToString(), IsUsable = true, IsSelectCtrl = true, IsSelectAlt = true, IsSelectShift = false, SelectKey = EKey.T });
            return hotKeyList;
        }

        /// <summary>
        /// 通知注册系统快捷键委托
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        public delegate bool RegisterGlobalHotKeyHandler(ObservableCollection<HotKeyModel> hotKeyModelList);
        public event RegisterGlobalHotKeyHandler RegisterGlobalHotKeyEvent;
        public bool RegisterGlobalHotKey(ObservableCollection<HotKeyModel> hotKeyModelList)
        {
            if (RegisterGlobalHotKeyEvent != null)
            {
                return RegisterGlobalHotKeyEvent(hotKeyModelList);
            }
            return false;
        }

    }
}
