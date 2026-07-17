namespace EntityGenerator.Models
{
    /// <summary>
    /// メッセージボックスパラメータ
    /// </summary>
    internal class MessageParam
    {
        #region プロパティ
        /// <summary>
        /// メッセージを取得または設定します。
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 見出しを取得または設定します。
        /// </summary>
        public string Caption { get; set; } = string.Empty;

        /// <summary>
        /// 複数行メッセージを取得または設定します。
        /// </summary>
        public List<string> Messages { get; set; } = [];
        #endregion
    }
}