using EntityGenerator.Models;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace EntityGenerator.Views
{
    /// <summary>
    /// メイン画面
    /// </summary>
    public partial class MainWindow : Window
    {
        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow() => InitializeComponent();
        #endregion

        #region イベントハンドラ
        #region Window_Loaded
        /// <summary>
        /// 画面読み込み終了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModels.MainViewModel vm)
            {
                vm.ShowInfo = ShowInfo;
                vm.ShowError = ShowError;
                vm.ShowConfirm = ShowConfirm;
                vm.Close = Close;
                vm.SelectFile = SelectFile;
                vm.SaveFile = SaveFile;
            }
        }
        #endregion

        #region Window_Closing
        /// <summary>
        /// 画面終了イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var messages = new List<string> { $"{Title}を終了します。", "よろしいですか？" };
            if (MsgBox.ShowConfirm(messages, "システム終了") != MessageBoxResult.Yes) e.Cancel = true;
        }
        #endregion

        #region TextBox_GotFocus
        /// <summary>
        /// テキストボックスフォーカスイン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// フォーカスしたときにテキストを全選択する。
        /// </remarks>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb) tb.SelectAll();
        }
        #endregion
        #endregion

        #region 内部メソッド
        #region ShowInfo
        /// <summary>
        /// メッセージを表示します。
        /// </summary>
        /// <param name="param">メッセージボックスパラメータ</param>
        private void ShowInfo(MessageParam param)
        {
            if (string.IsNullOrWhiteSpace(param.Caption))
            {
                if (!string.IsNullOrEmpty(param.Message))
                    MsgBox.ShowInfo(param.Message);
                else if (param.Messages.Count > 0)
                    MsgBox.ShowInfo(param.Messages);
            }
            else
            {
                if (!string.IsNullOrEmpty(param.Message))
                    MsgBox.ShowInfo(param.Message, param.Caption);
                else if (param.Messages.Count > 0)
                    MsgBox.ShowInfo(param.Messages, param.Caption);
            }
        }
        #endregion

        #region ShowError
        /// <summary>
        /// エラーメッセージを表示します。
        /// </summary>
        /// <param name="param">メッセージボックスパラメータ</param>
        private void ShowError(MessageParam param)
        {
            if (string.IsNullOrWhiteSpace(param.Caption))
            {
                if (!string.IsNullOrEmpty(param.Message))
                    MsgBox.ShowError(param.Message);
                else if (param.Messages.Count > 0)
                    MsgBox.ShowError(param.Messages);
            }
            else
            {
                if (!string.IsNullOrEmpty(param.Message))
                    MsgBox.ShowError(param.Message, param.Caption);
                else if (param.Messages.Count > 0)
                    MsgBox.ShowError(param.Messages, param.Caption);
            }
        }
        #endregion

        #region ShowConfirm
        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="param">メッセージボックスパラメータ</param>
        private bool ShowConfirm(MessageParam param)
        {
            var result = false;
            if (string.IsNullOrWhiteSpace(param.Caption))
            {
                if (!string.IsNullOrEmpty(param.Message))
                    result = MsgBox.ShowConfirm(param.Message) == MessageBoxResult.Yes;
                else if (param.Messages.Count > 0)
                    result = MsgBox.ShowConfirm(param.Messages) == MessageBoxResult.Yes;
            }
            else
            {
                if (!string.IsNullOrEmpty(param.Message))
                    result = MsgBox.ShowConfirm(param.Message, param.Caption) == MessageBoxResult.Yes;
                else if (param.Messages.Count > 0)
                    result = MsgBox.ShowConfirm(param.Messages, param.Caption) == MessageBoxResult.Yes;
            }

            return result;
        }
        #endregion

        #region SelectFile
        /// <summary>
        /// ファイル選択ダイアログを表示します。
        /// </summary>
        /// <param name="preFilePath">元ファイルパス</param>
        /// <returns>表示結果, ファイルパス</returns>
        private (bool Result, string FilePath) SelectFile(string preFilePath, string filter)
        {
            var result = false;
            var filePath = string.Empty;

            var dialog = new OpenFileDialog()
            {
                Filter = filter,
                FileName = preFilePath
            };

            result = dialog.ShowDialog().GetValueOrDefault();
            if (result) filePath = dialog.FileName;

            return (result, filePath);
        }
        #endregion

        #region SaveFile
        /// <summary>
        /// ファイル保存ダイアログを表示します。
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <returns>表示結果, ファイルパス</returns>
        private (bool Result, string FilePath) SaveFile(string fileName, string filter)
        {
            var result = false;
            var filePath = string.Empty;

            var dialog = new SaveFileDialog
            {
                Filter = filter,
                FileName = fileName
            };

            result = dialog.ShowDialog().GetValueOrDefault();
            if (result) filePath = dialog.FileName;

            return (result, filePath);
        }
        #endregion
        #endregion
    }
}
