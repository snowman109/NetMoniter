using Echevil;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using System.IO;
using IDataObject = System.Windows.Forms.IDataObject;
using System.Collections.ObjectModel;

namespace Utils
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotifyIcon TrayIcon;
        private ContextMenu notifyiconMnu;
        private NetworkAdapter[] adapters;
        private DispatcherTimer myTimer = null;
        private NetworkMonitor monitor;
        private translate tran = null;
        private Config config;
        //tray menu
        MenuItem[] mnuItms = new MenuItem[8];
        /// <summary>
        /// 当前窗口句柄
        /// </summary>
        private IntPtr m_Hwnd = new IntPtr();
        // <summary>
        /// 记录快捷键注册项的唯一标识符
        /// </summary>
        private Dictionary<EHotKeySetting, int> m_HotKeySettings = new Dictionary<EHotKeySetting, int>();
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += Hotkey_Loaded;
            this.Loaded += image_Loaded;
            this.Loaded += FormMain_Load;
            myTimer = new DispatcherTimer();  //实例化定时器
            //设置定时器属性
            myTimer.Interval = new TimeSpan(0, 0, 1);  //创建时分秒
            myTimer.Tick += new EventHandler(Timer_Tick);
            myTimer.Start();
            config = new Config();
            tran = new translate(config);
            Initializenotifyicon();
        }
        /// <summary>
        /// WPF窗体的资源初始化完成，并且可以通过WindowInteropHelper获得该窗体的句柄用来与Win32交互后调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            // 获取窗体句柄
            m_Hwnd = new WindowInteropHelper(this).Handle;
            HwndSource hWndSource = HwndSource.FromHwnd(m_Hwnd);
            // 添加处理程序
            if (hWndSource != null) hWndSource.AddHook(WndProc);
        }

        /// <summary>
        /// 所有控件初始化完成后调用
        /// </summary>
        /// <param name="e"></param>
        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            // 注册热键
            InitHotKey();
        }
        // ###########注册快捷键
        [DllImport("user32")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32")]
        private static extern IntPtr FindWindowEx(IntPtr par1, IntPtr par2, String par3, String par4);
        [DllImport("user32")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        // 注册热键
        private void Hotkey_Loaded(object sender, RoutedEventArgs e)
        {
            HotKeySettingsManager.Instance.RegisterGlobalHotKeyEvent += Instance_RegisterGlobalHotKeyEvent;
        }
        /// <summary>
        /// 通知注册系统快捷键事件处理函数
        /// </summary>
        /// <param name="hotKeyModelList"></param>
        /// <returns></returns>
        private bool Instance_RegisterGlobalHotKeyEvent(ObservableCollection<HotKeyModel> hotKeyModelList)
        {
            return InitHotKey(hotKeyModelList);
        }
        /// <summary>
        /// 初始化注册快捷键
        /// </summary>
        /// <param name="hotKeyModelList">待注册热键的项</param>
        /// <returns>true:保存快捷键的值；false:弹出设置窗体</returns>
        private bool InitHotKey(ObservableCollection<HotKeyModel> hotKeyModelList = null)
        {
            var list = hotKeyModelList ?? HotKeySettingsManager.Instance.LoadDefaultHotKey();
            // 注册全局快捷键
            string failList = HotKeyHelper.RegisterGlobalHotKey(list, m_Hwnd, out m_HotKeySettings);
            if (string.IsNullOrEmpty(failList))
                return true;
            MessageBoxResult mbResult = System.Windows.MessageBox.Show(string.Format("无法注册下列快捷键\n\r{0}是否要改变这些快捷键？", failList), "提示", MessageBoxButton.YesNo);
            // 弹出热键设置窗体
            var win = HotKeySettingsWindow.CreateInstance(config);
            if (mbResult == MessageBoxResult.Yes)
            {
                if (!win.IsVisible)
                {
                    win.ShowDialog();
                }
                else
                {
                    win.Activate();
                }
                return false;
            }
            return true;
        }
        /// <summary>
        /// 窗体回调函数，接收所有窗体消息的事件处理函数
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <param name="msg">消息</param>
        /// <param name="wideParam">附加参数1</param>
        /// <param name="longParam">附加参数2</param>
        /// <param name="handled">是否处理</param>
        /// <returns>返回句柄</returns>
        private IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wideParam, IntPtr longParam, ref bool handled)
        {
           
            switch (msg)
            {
                case HotKeyManager.WM_HOTKEY:
                    int sid = wideParam.ToInt32();
                    if (sid == m_HotKeySettings[EHotKeySetting.去除换行])
                    {
                        CopyWithNewLine();
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.换行翻译])
                    {
                        CopyWithNewLine();
                        TranslateText();
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.文字翻译])
                    {
                        TranslateText();
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.图片翻译])
                    {
                        TranslateImage();
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.插件锁定])
                    {
                        mnuItms[1].Checked = !mnuItms[1].Checked;
                    }
                    else if (sid == m_HotKeySettings[EHotKeySetting.插件顶层])
                    {
                        mnuItms[3].Checked = !mnuItms[3].Checked;
                        topAlo(mnuItms[3].Checked);
                    }
                    //MessageBox.Show(string.Format("触发【{0}】快捷键", hotkeySetting));
                    handled = true;
                    break;
            }
            return IntPtr.Zero;
        }

        private void CopyWithNewLine()
        {
            string s = System.Windows.Clipboard.GetText();
            s = s.Replace("\r\n", " ");
            s = s.Replace("\r", " ");
            s = s.Replace("\n", " ");
            s = s.Replace("Eq.", "Eq");
            System.Windows.Clipboard.SetText(s);
        }
        private void TranslateText()
        {
            if (System.Windows.Clipboard.ContainsText())
            {
                string s = System.Windows.Clipboard.GetText();
                dialog d = new dialog();
                System.Drawing.Point point = System.Windows.Forms.Cursor.Position;
                d.Left = point.X - 50;
                d.Top = point.Y - 50;
                d.OriginText = s;
                d.GoalText = tran.translateText(s);
                d.Show();
            }
        }
        private void TranslateImage()
        {
            if (System.Windows.Clipboard.ContainsImage())
            {
                Image data = System.Windows.Forms.Clipboard.GetImage();
                dialog d = new dialog();
                System.Drawing.Point point = System.Windows.Forms.Cursor.Position;
                d.Left = point.X - 50;
                d.Top = point.Y - 50;
                string base64 = ToBase64(data);
                Tuple<string, string> result = tran.translateImage(base64);
                d.OriginText = result.Item1;
                d.GoalText = result.Item2;
                d.Show();
            }
        }

        private string ToBase64(Image img)
        {
            try
            {
                Bitmap bmp = new Bitmap(img);
                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                String strbaser64 = Convert.ToBase64String(arr);
                return strbaser64;
            }
            catch (Exception ex)
            {
                Console.WriteLine("出问题了" + ex.ToString());
                return "";
            }
        }
        // ######################

        // ####################下面的菜单
        private void Initializenotifyicon()
        {
            string basePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;


            TrayIcon = new NotifyIcon();
            TrayIcon.Icon = new Icon(basePath + "Tray.ico");

            TrayIcon.Text = "NetMonitor";
            TrayIcon.Visible = true;
            TrayIcon.Click += new EventHandler(this.click);

            mnuItms[0] = new MenuItem("-");

            mnuItms[1] = new MenuItem();
            mnuItms[1].Text = "Lock";
            mnuItms[1].Click += new EventHandler(this.Locked);
            

            mnuItms[2] = new MenuItem("-");

            mnuItms[3] = new MenuItem();
            mnuItms[3].Text = "Top";
            mnuItms[3].Click += new EventHandler(this.TopAlo);
            

            mnuItms[4] = new MenuItem("-");

            mnuItms[5] = new MenuItem();
            mnuItms[5].Text = "Setting";
            mnuItms[5].Click += new EventHandler(this.Set);

            mnuItms[6] = new MenuItem("-");

            mnuItms[7] = new MenuItem();
            mnuItms[7].Text = "Exit";
            mnuItms[7].Click += new EventHandler(this.ExitSelect);
            mnuItms[7].DefaultItem = true;
            
            notifyiconMnu = new ContextMenu(mnuItms);
            TrayIcon.ContextMenu = notifyiconMnu;
        }
        private void Set(object sender, EventArgs e)
        {
            var win = HotKeySettingsWindow.CreateInstance(config);
            if (!win.IsVisible)
            {
                win.ShowDialog();
            }
            else
            {
                win.Activate();
            }
            //System.Diagnostics.Process.Start("notepad.exe", tran.DEFAULT_PATH);
        }
        private void TopAlo(object sender, EventArgs e)
        {
            System.Windows.Forms.MenuItem item = (System.Windows.Forms.MenuItem)sender;
            bool check = item.Checked;
            item.Checked = !check;
            topAlo(check);
        }
        private void topAlo(bool check)
        {
            if (check)
            {
                Topmost = true;
                Deactivated += Window_Deactivated;
            }
            else
            {
                Topmost = false;
                Deactivated -= Window_Deactivated;
            }
        }
        private void Window_Deactivated(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            window.Topmost = true;
        }
        private void ExitSelect(object sender, EventArgs e)
        {
            TrayIcon.Visible = false;
            this.Close();
        }

        private void Locked(object sender, EventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            item.Checked = !item.Checked;
        }
        public void click(object sender, System.EventArgs e)
        {
            this.Activate();
        }
        // #############################
        // ######################完全插件
        #region Window styles
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);


        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]

        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);
        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }
        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }
        public enum GetWindowLongFields
        {
            // ...
            GWL_EXSTYLE = (-20),
            // ...
        }
        [Flags]
        public enum ExtendedWindowStyles
        {
            // ...
            WS_EX_TOOLWINDOW = 0x00000080,
            // ...
        }
        private void image_Loaded(object sender, RoutedEventArgs e)
        {
            WindowInteropHelper wndHelper = new WindowInteropHelper(this);

            int exStyle = (int)GetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE);

            exStyle |= (int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;

            SetWindowLong(wndHelper.Handle, (int)GetWindowLongFields.GWL_EXSTYLE, (IntPtr)exStyle);
        }
        #endregion
        // ##############################
        // #########################显示桌面不返回
        /// <summary>
        /// 将程序嵌入桌面
        /// </summary>
        /// <returns></returns>
        private IntPtr GetDesktopPtr()//寻找桌面的句柄
        {
            // 情况一
            IntPtr hwndWorkerW = IntPtr.Zero;
            IntPtr hShellDefView = IntPtr.Zero;
            IntPtr hwndDesktop = IntPtr.Zero;
            IntPtr hProgMan = FindWindow("Progman", "Program Manager");
            if (hProgMan != IntPtr.Zero)
            {
                hShellDefView = FindWindowEx(hProgMan, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (hShellDefView != IntPtr.Zero)
                {
                    hwndDesktop = FindWindowEx(hShellDefView, IntPtr.Zero, "SysListView32", null);
                }
            }
            if (hwndDesktop != IntPtr.Zero) return hwndDesktop;

            // 情况二
            //必须存在桌面窗口层次
            while (hwndDesktop == IntPtr.Zero)
            {
                //获得WorkerW类的窗口
                hwndWorkerW = FindWindowEx(IntPtr.Zero, hwndWorkerW, "WorkerW", null);
                if (hwndWorkerW == IntPtr.Zero) break;
                hShellDefView = FindWindowEx(hwndWorkerW, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (hShellDefView == IntPtr.Zero) continue;
                hwndDesktop = FindWindowEx(hShellDefView, IntPtr.Zero, "SysListView32", null);
            }
            return hwndDesktop;
        }
        // 获取当前进程的句柄
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetParent(new WindowInteropHelper(this).Handle, GetDesktopPtr());
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!mnuItms[1].Checked) { this.DragMove(); }
        }
        //################## 网速显示
        private void FormMain_Load(object sender, System.EventArgs e)
        {
            monitor = new NetworkMonitor();
            this.adapters = monitor.Adapters;
        }
        private void Timer_Tick(Object sender, EventArgs e)
        {          
            TimerCounter_Tick();
        }
        private void TimerCounter_Tick()
        {
            string down = "kB/s";
            string up = "KB/s";
            double downloadSpeed = 0;
            double uploadSpeed = 0;
            if (adapters == null)
            {
                return;
            }
            for (int i = 0; i < adapters.Length; i++)
            {
                monitor.StartMonitoring();
                //Console.WriteLine(adapters[i].DownloadSpeedKbps);
                downloadSpeed += adapters[i].DownloadSpeedKbps;
                uploadSpeed += adapters[i].UploadSpeedKbps;
            }
            //// 转换成KB
            //downloadSpeed /= 8;
            //uploadSpeed /= 8;
            // 转换成MB
            if (downloadSpeed > 1024)
            {
                downloadSpeed /= 1024;
                down = "MB/s";
            }
            if (uploadSpeed > 1024)
            {
                uploadSpeed /= 1024;
                up = "MB/s";
            }
            this.Download.Content = "↓ " + downloadSpeed.ToString("0.00") + down;
            this.Upload.Content = "↑ " + uploadSpeed.ToString("0.00") + up;
        }
    }
}
