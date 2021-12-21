using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Utils
{
    /// <summary>
    /// HotKeySettingsWindow.xaml 的交互逻辑
    /// </summary>
    /// <summary>
    /// 快捷键设置窗体交互逻辑
    /// </summary>
    public partial class HotKeySettingsWindow : Window
    {
        /// <summary>
        /// 单例实例
        /// </summary>
        private static HotKeySettingsWindow m_Instance;

        private Config config;
        public string SecretIdContent { get; set; }
        public string SecretKeyContent { get; set; }
        private ObservableCollection<HotKeyModel> m_HotKeyList = new ObservableCollection<HotKeyModel>();
        private ObservableCollection<Option> m_source= new ObservableCollection<Option>();
        private ObservableCollection<Option> m_target = new ObservableCollection<Option>();
        /// <summary>
        /// 快捷键设置项集合
        /// </summary>
        public ObservableCollection<HotKeyModel> HotKeyList
        {
            get { return m_HotKeyList; }
            set { m_HotKeyList = value; }
        }
        public ObservableCollection<Option> SourceOptions
        {
            get { return m_source; }
            set { m_source = value; }
        }
        public ObservableCollection<Option> TargetOptions
        {
            get { return m_target; }
            set { m_target = value; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public HotKeySettingsWindow(Config con)
        {
            InitializeComponent();
            config = con;
            IdTextBox.Text = config.getConfig(Config.SECRET_ID);
            KeyTextBox.Text = config.getConfig(Config.SECRET_KEY);
            ProjectTextBox.Text = config.getConfig(Config.PROJECT_ID); 
        }

        /// <summary>
        /// 创建系统参数设置窗体实例
        /// </summary>
        /// <returns></returns>
        public static HotKeySettingsWindow CreateInstance(Config config)
        {
            return m_Instance ?? (m_Instance = new HotKeySettingsWindow(config));
        }

        /// <summary>
        /// 窗体加载完成后事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void win_Loaded(object sender, RoutedEventArgs e)
        {
            InitHotKey();
            InitOptions();
        }

        /// <summary>
        /// 初始化快捷键
        /// </summary>
        private void InitHotKey()
        {
            var list = config.getHotKey();
            list.ToList().ForEach(x => HotKeyList.Add(x));
        }
        private void InitOptions()
        {
            var source = OptionManager.LoadDefaultSource();
            var target = OptionManager.LoadDefaultTarget()[config.getConfig(Config.SOURCE)];
            int sourceSelected = 0;
            int targetSelected = 0;
            for (int i = 0; i < source.Count; i++)
            {
                var x = source[i];
                SourceOptions.Add(x);
                if (x.Value.Equals(config.getConfig(Config.SOURCE)))
                {
                    sourceSelected = i;
                }
            }
            for(int i = 0; i < target.Count; i++)
            {
                var x = target[i];
                TargetOptions.Add(x);
                if (x.Value.Equals(config.getConfig(Config.TARGET)))
                {
                    targetSelected = i;
                }
            }
            sourceSelect.ItemsSource = source;
            sourceSelect.DisplayMemberPath = "Name";
            sourceSelect.SelectedValuePath = "Value";
            sourceSelect.SelectedIndex = sourceSelected;
            targetSelect.ItemsSource = target;
            targetSelect.DisplayMemberPath = "Name";
            targetSelect.SelectedValuePath = "Value";
            targetSelect.SelectedIndex = targetSelected;
            
        }
        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveSetting_Click(object sender, RoutedEventArgs e)
        {
            if (!HotKeySettingsManager.Instance.RegisterGlobalHotKey(HotKeyList))
                return;
            config.setConfig(Config.SECRET_ID, IdTextBox.Text);
            config.setConfig(Config.SECRET_KEY, KeyTextBox.Text);
            config.setConfig(Config.PROJECT_ID, ProjectTextBox.Text);
            config.setConfig(Config.SOURCE, sourceSelect.SelectedValue.ToString());
            config.setConfig(Config.TARGET, targetSelect.SelectedValue.ToString());
            config.loadHotKey(HotKeyList);
            config.save();
            this.Close();
        }

        /// <summary>
        /// 窗体关闭后事件处理函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void win_Closed_1(object sender, EventArgs e)
        {
            m_Instance = null;
        }

        private void sourceSelect_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var target = OptionManager.LoadDefaultTarget()[sourceSelect.SelectedValue.ToString()];
            targetSelect.ItemsSource = target;
            targetSelect.SelectedIndex = 0;
        }
    }
}
