using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace StikyNotes
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel
    {
        /// <summary>
        /// 窗体数据
        /// </summary>
        public WindowsData Datas { get; set; }

        public ProgramData ProgramData { get; set; }
        #region 命令
        public RelayCommand NewWindowCommand { get; private set; }
        public RelayCommand OpenSettingCommand { get; private set; }
        public RelayCommand OpenAboutCommand { get; private set; }
        public RelayCommand AddFontSizeCommand { get; private set; }
        public RelayCommand ReduceFontSizeCommand { get; private set; }
        public RelayCommand<object> MoveWindowCommand { get; private set; }
        public RelayCommand<MainWindow> DeletePaWindowCommand { get; private set; }
        #endregion

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            NewWindowCommand = new RelayCommand(NewWindowMethod);
            OpenSettingCommand = new RelayCommand(OpenSettingMethod);
            OpenAboutCommand = new RelayCommand(OpenAboutMethod);
            DeletePaWindowCommand = new RelayCommand<MainWindow>(DeleteWindowMethod);
            MoveWindowCommand = new RelayCommand<object>(MoveWindowMethod);
            AddFontSizeCommand = new RelayCommand(AddFontSizeMethod);
            ReduceFontSizeCommand = new RelayCommand(ReduceFontSizeMethod);
            ProgramData=ProgramData.Instance;
        }

        /// <summary>
        /// 打开相关窗口
        /// </summary>
        private void OpenAboutMethod()
        {
            var win=new AboutWindow();
            win.Show();
        }

        /// <summary>
        /// 减少字体
        /// </summary>
        private void ReduceFontSizeMethod()
        {
            if (Datas.FontSize > 8)
            {
                Datas.FontSize -= 2;
            }
        }
        /// <summary>
        /// 放大字体
        /// </summary>
        private void AddFontSizeMethod()
        {
            if (Datas.FontSize < 32)
            {
                Datas.FontSize += 2;
            }
        }

        private void OpenSettingMethod()
        {
            var win = new SettingWindow();
            var vm=new SettingViewModel();
            win.DataContext = vm;
            win.Show();
        }

        /// <summary>
        /// 移动窗体
        /// </summary>
        /// <param name="e">当前的Window</param>
        private void MoveWindowMethod(object e)
        {
            var win = e as MainWindow;
            win.ResizeMode = ResizeMode.NoResize;
            win.DragMove();
            win.ResizeMode = ResizeMode.CanResize;

            //            var newPos=win.PointFromScreen(new Point(0, 0));
            //            Datas.StartUpPosition = newPos;
        }

        /// <summary>
        /// 新建窗体
        /// </summary>
        void NewWindowMethod()
        {
            MainWindow win = new MainWindow();
            var vm = new MainViewModel();
            vm.Datas = new WindowsData();
            win.DataContext = vm;
            win.Show();
            ProgramData.Instance.Datas.Add(vm.Datas);
            WindowsManager.Instance.Windows.Add(win);

        }

        /// <summary>
        /// 删除窗体
        /// </summary>
        void DeleteWindowMethod(MainWindow obj)
        {
            var win = obj as MainWindow;
            if (WindowsManager.Instance.Windows.Contains(win))
            {
                WindowsManager.Instance.Windows.Remove(win);
                ProgramData.Instance.Datas.Remove(Datas);
            }
            win.Close();
        }

    }
}