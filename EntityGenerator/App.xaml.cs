using EntityGenerator.Views;
using System.Windows;

namespace EntityGenerator
{
    /// <summary>
    /// Entity生成ツール
    /// </summary>
    public partial class App : Application
    {
        #region メンバ変数
        /// <summary>
        /// 多重起動抑止用
        /// </summary>
        private Mutex? _mutex;

        /// <summary>
        /// 新たにMutexを作成できたか？
        /// </summary>
        private bool _isCreatedNew;
        #endregion

        #region オーバーライド
        /// <summary>
        /// OnStartup
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            _mutex = new Mutex(true, nameof(EntityGenerator), out _isCreatedNew);
            if (!_isCreatedNew)
            {
                Shutdown();
                return;
            }

            new MainWindow().Show();
        }

        /// <summary>
        /// OnExit
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            if (_isCreatedNew)
                _mutex?.ReleaseMutex();
            _mutex?.Dispose();
            base.OnExit(e);
        }
        #endregion
    }
}