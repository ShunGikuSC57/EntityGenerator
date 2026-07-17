namespace EntityGenerator
{
    /// <summary>
    /// 定数
    /// </summary>
    internal static class Constants
    {
        #region 拡張子
        /// <summary>
        /// 拡張子
        /// </summary>
        internal struct Extensions
        {
            /// <summary>JSON</summary>
            public const string JSON = ".json";
            /// <summary>EXCEL</summary>
            public const string EXCEL = ".xlsx";
            /// <summary>C Sharp</summary>
            public const string CS = ".cs";
        }
        #endregion

        #region ファイルフィルター
        /// <summary>
        /// ファイルフィルター
        /// </summary>
        internal struct FileFilter
        {
            /// <summary>EXCEL</summary>
            public const string EXCEL = $"Excelブック (*{Extensions.EXCEL})|*{Extensions.EXCEL}";
            /// <summary>C Sharp</summary>
            public const string CS = $"CSファイル (*{Extensions.CS})|*{Extensions.CS}";
        }
        #endregion

        #region データ型
        /// <summary>
        /// 標準データ型
        /// </summary>
        public const string DEFAULT_DATA_TYPE = "string";

        /// <summary>
        /// 型別宣言時代入値ペア
        /// </summary>
        public static readonly Dictionary<string, string> DEFAULT_VALUE_STRING_PAIRS = new()
        {
            { "string", " = string.Empty;" },
            { "int", string.Empty },
            { "long", string.Empty },
            { "decimal", string.Empty },
            { "double", string.Empty },
            { "float", string.Empty },
            { "bool", string.Empty },
            { "DateTime", " = DateTime.MinValue;" },
            { "DateOnly", " = DateOnly.MinValue;" },
            { "TimeOnly", " = TimeOnly.MinValue;" },
            { "TimeSpan", " = TimeSpan.Zero;" },
            { "Guid", " = Guid.Empty;" },
            { "byte[]", " = [];" }
        };
        #endregion
    }
}
