using CommunityToolkit.Mvvm.ComponentModel;
using EntityGenerator.Models;

namespace EntityGenerator.ViewModels
{
    /// <summary>
    /// ViewModel基底クラス
    /// </summary>
    internal abstract class ViewModelBase : ObservableObject
    {
        #region UIアクション
        /// <summary>
        /// メッセージ表示を実装します。
        /// </summary>
        public Action<MessageParam>? ShowInfo { get; set; }

        /// <summary>
        /// 警告表示を実装します。
        /// </summary>
        public Action<MessageParam>? ShowWarning { get; set; }

        /// <summary>
        /// エラーメッセージ表示を実装します。
        /// </summary>
        public Action<MessageParam>? ShowError { get; set; }

        /// <summary>
        /// 確認メッセージ表示を実装します。
        /// </summary>
        public Func<MessageParam, bool>? ShowConfirm { get; set; }

        /// <summary>
        /// 画面終了時のUIアクションを取得または設定します。
        /// </summary>
        public Action? Close { get; set; }
        #endregion
    }
}
