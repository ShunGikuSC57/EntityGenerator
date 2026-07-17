using System.Windows;

namespace EntityGenerator
{
    /// <summary>
    /// メッセージボックス
    /// </summary>
    internal static class MsgBox
    {
        #region 公開メソッド
        #region ShowInfo
        /// <summary>
        /// メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="caption">キャプション</param>
        public static void ShowInfo(string message, string caption = "通知")
            => ShowMessage([message], caption, MessageBoxButton.OK, MessageBoxImage.Information);

        /// <summary>
        /// メッセージを表示します。
        /// </summary>
        /// <param name="messages">メッセージリスト</param>
        /// <param name="caption">キャプション</param>
        public static void ShowInfo(IEnumerable<string> messages, string caption = "通知")
            => ShowMessage(messages, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        #endregion

        #region ShowWarning
        /// <summary>
        /// 警告メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="caption">キャプション</param>
        public static void ShowWarning(string message, string caption = "警告")
            => ShowMessage([message], caption, MessageBoxButton.OK, MessageBoxImage.Warning);

        /// <summary>
        /// 警告メッセージを表示します。
        /// </summary>
        /// <param name="messages">メッセージリスト</param>
        /// <param name="caption">キャプション</param>
        public static void ShowWarning(IEnumerable<string> messages, string caption = "警告")
            => ShowMessage(messages, caption, MessageBoxButton.OK, MessageBoxImage.Warning);
        #endregion

        #region ShowError
        /// <summary>
        /// エラーメッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="caption">キャプション</param>
        public static void ShowError(string message, string caption = "エラー")
            => ShowMessage([message], caption, MessageBoxButton.OK, MessageBoxImage.Error);

        /// <summary>
        /// エラーメッセージを表示します。
        /// </summary>
        /// <param name="messages">メッセージリスト</param>
        /// <param name="caption">キャプション</param>
        public static void ShowError(IEnumerable<string> messages, string caption = "エラー")
            => ShowMessage(messages, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        #endregion

        #region ShowConfirm
        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="message">メッセージ</param>
        /// <param name="caption">キャプション</param>
        public static MessageBoxResult ShowConfirm(string message, string caption = "確認")
            => ShowMessage([message], caption, MessageBoxButton.YesNo, MessageBoxImage.Question);

        /// <summary>
        /// 確認メッセージを表示します。
        /// </summary>
        /// <param name="messages">メッセージリスト</param>
        /// <param name="caption">キャプション</param>
        public static MessageBoxResult ShowConfirm(IEnumerable<string> messages, string caption = "確認")
            => ShowMessage(messages, caption, MessageBoxButton.YesNo, MessageBoxImage.Question);
        #endregion
        #endregion

        #region 内部メソッド
        #region ShowMessage
        /// <summary>
        /// メッセージを表示します。
        /// </summary>
        /// <param name="messages">メッセージリスト</param>
        /// <param name="caption">キャプション</param>
        /// <param name="button">ボタン</param>
        /// <param name="icon">アイコン</param>
        /// <returns></returns>
        private static MessageBoxResult ShowMessage(IEnumerable<string> messages, string caption, MessageBoxButton button, MessageBoxImage icon)
            => MessageBox.Show(string.Join(Environment.NewLine, messages), caption, button, icon);
        #endregion
        #endregion
    }
}
