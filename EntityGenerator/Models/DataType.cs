using System.IO;
using System.Text;
using System.Text.Json;

namespace EntityGenerator.Models
{
    /// <summary>
    /// データ型
    /// </summary>
    /// <remarks>
    /// データベースや定義書に定義されている型と対応するC#のデータ型のペアを定義するクラスです。
    /// </remarks>
    public class DataType
    {
        #region 定数
        /// <summary>
        /// データ定義ファイル名
        /// </summary>
        public static readonly string DATA_DEF_FILE_NAME = $"{nameof(DataType)}s.json";

        /// <summary>
        /// 標準データ型
        /// </summary>
        private static readonly List<DataType> DEFAULT_DATA_TYPE =
        [
            new DataType { DefDataType = "文字列", CsDataType = "string" },
            new DataType { DefDataType = "整数", CsDataType = "int" },
            new DataType { DefDataType = "長整数", CsDataType = "long" },
            new DataType { DefDataType = "小数", CsDataType = "decimal" },
            new DataType { DefDataType = "浮動小数点数", CsDataType = "double" },
            new DataType { DefDataType = "真偽値", CsDataType = "bool" },
            new DataType { DefDataType = "日付", CsDataType = "DateTime" },
            new DataType { DefDataType = "日付時刻", CsDataType = "DateTime" },
            new DataType { DefDataType = "バイナリ", CsDataType = "byte[]" }
        ];
        #endregion

        #region プロパティ
        /// <summary>
        /// 定義書のデータ型を取得または設定します。
        /// </summary>
        public string DefDataType { get; set; } = string.Empty;

        /// <summary>
        /// C#のデータ型を取得または設定します。
        /// </summary>
        public string CsDataType { get; set; } = string.Empty;
        #endregion

        #region 公開メソッド
        /// <summary>
        /// データ型のペアを読み込みます。
        /// </summary>
        /// <returns>データ型のペア</returns>
        public static Dictionary<string,string> Load()
        {
            List<DataType> dataTypes = [.. DEFAULT_DATA_TYPE];
            try
            {
                var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(EntityGenerator));
                var filePath = Path.Combine(dir, DATA_DEF_FILE_NAME);
                if (!File.Exists(filePath))
                {
                    // 標準データ型JSON出力
                    if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                    var def = JsonSerializer.Serialize(DEFAULT_DATA_TYPE, new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                    });

                    File.WriteAllText(filePath, def, Encoding.UTF8);
                }

                var json = File.ReadAllText(filePath, Encoding.UTF8);
                var list = JsonSerializer.Deserialize<List<DataType>>(json);
                if (list != null) dataTypes = list;
            }
            catch { }

            Dictionary<string, string> dataTypePairs = [];
            foreach (var dataType in dataTypes)
                dataTypePairs[dataType.DefDataType] = dataType.CsDataType;

            return dataTypePairs;
        }
        #endregion
    }
}
