namespace EntityGenerator.Models
{
    /// <summary>
    /// 変換項目
    /// </summary>
    internal class ConvertItem
    {
        #region プロパティ
        /// <summary>
        /// 論理名を取得または設定します。
        /// </summary>
        public string LogicalName { get; set; } = string.Empty;

        /// <summary>
        /// 物理名を取得または設定します。
        /// </summary>
        public string PhysicsName { get; set; } = string.Empty;

        /// <summary>
        /// C#データ型文字列を取得または設定します。（初期化値「string」）
        /// </summary>
        public string DataType { get; set; } = Constants.DEFAULT_DATA_TYPE;

        /// <summary>
        /// Null不許可か？を取得または設定します。
        /// </summary>
        public bool IsNotNull { get; set; }

        /// <summary>
        /// 変換後の名を取得または設定します。
        /// </summary>
        public string ConvertedName { get; set; } = string.Empty;
        #endregion
    }
}
