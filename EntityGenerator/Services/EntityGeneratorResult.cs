namespace EntityGenerator.Services
{
    /// <summary>
    /// エンティティ生成結果
    /// </summary>
    internal sealed class EntityGeneratorResult
    {
        #region プロパティ
        /// <summary>
        /// 生成したソースコードを取得または設定します。
        /// </summary>
        public string GeneratedCode { get; set; } = string.Empty;

        /// <summary>
        /// ファイル出力する場合のクラス名を取得または設定します。
        /// </summary>
        public string OutputClassName { get; set; } = string.Empty;

        /// <summary>
        /// エラーメッセージリストを取得します。
        /// </summary>
        public List<string> ErrorMessages { get; } = new List<string>();
        #endregion
    }
}
