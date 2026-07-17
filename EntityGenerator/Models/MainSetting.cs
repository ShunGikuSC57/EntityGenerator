using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EntityGenerator.Models
{
    /// <summary>
    /// メイン画面項目値
    /// </summary>
    public class MainSetting
    {
        #region プロパティ
        #region 生成対象
        /// <summary>
        /// 生成対象 プロパティを取得または設定します。
        /// </summary>
        public bool IsGenerateProperty { get; set; } = true;

        /// <summary>
        /// 生成対象 ViewModelを取得または設定します。
        /// </summary>
        public bool IsGenerateViewModel { get; set; }

        /// <summary>
        /// 生成対象 変換（命名規則に従って変換）を取得または設定します。
        /// </summary>
        public bool IsGenerateConverted { get; set; }
        #endregion

        #region ViewModel実装方式
        /// <summary>
        /// ViewModel実装方式 ObservablePropertyを取得または設定します。
        /// </summary>
        public bool IsViewModelStyleObservableProperty { get; set; } = true;

        /// <summary>
        /// ViewModel実装方式 SetPropertyを取得または設定します。
        /// </summary>
        public bool IsViewModelStyleSetProperty { get; set; }
        #endregion

        #region 命名規則
        /// <summary>
        /// 命名規則 パスカルケースを取得または設定します。
        /// </summary>
        public bool IsNamingPascal { get; set; } = true;

        /// <summary>
        /// 命名規則 キャメルケースを取得または設定します。
        /// </summary>
        public bool IsNamingCamel { get; set; }

        /// <summary>
        /// 命名規則 大文字を取得または設定します。
        /// </summary>
        public bool IsNamingUpper { get; set; }

        /// <summary>
        /// 命名規則 小文字を取得または設定します。
        /// </summary>
        public bool IsNamingLower { get; set; }

        /// <summary>
        /// 命名規則 物理名のままを取得または設定します。
        /// </summary>
        public bool IsNamingAsIs { get; set; }
        #endregion

        #region 変換対象
        /// <summary>
        /// 変換対象 EXCELを取得または設定します。
        /// </summary>
        public bool IsSourceExcel { get; set; } = true;

        /// <summary>
        /// 変換対象 入力を取得または設定します。
        /// </summary>
        public bool IsSourceInput { get; set; }
        #endregion

        #region EXCEL
        /// <summary>
        /// EXCEL ファイルパスを取得または設定します。
        /// </summary>
        public string ExcelFilePath { get; set; } = string.Empty;

        /// <summary>
        /// EXCEL 読み込む対象のシート名を取得または設定します。
        /// </summary>
        public string ReadSheetName { get; set; } = string.Empty;

        /// <summary>
        /// EXCEL 読み込む対象行数（自）を取得または設定します。
        /// </summary>
        public int ReadRowFrom { get; set; } = 5;

        /// <summary>
        /// EXCEL 読み込む対象行数（至）を取得または設定します。
        /// </summary>
        public int ReadRowTo { get; set; } = 10;

        /// <summary>
        /// EXCEL 論理名列数を取得または設定します。
        /// </summary>
        public int LogicalNameColumn { get; set; } = 1;

        /// <summary>
        /// EXCEL 物理名列数を取得または設定します。
        /// </summary>
        public int PhysicsNameColumn { get; set; } = 2;

        /// <summary>
        /// EXCEL データ型列数を取得または設定します。
        /// </summary>
        public int DataTypeColumn { get; set; } = 3;

        /// <summary>
        /// EXCEL Null制約列数を取得または設定します。
        /// </summary>
        public int NullableColumn { get; set; } = 4;
        #endregion

        #region 出力方法
        /// <summary>
        /// 出力方法 画面出力を取得または設定します。
        /// </summary>
        public bool IsScreenOutput { get; set; } = true;

        /// <summary>
        /// 出力方法 画面出力を取得または設定します。
        /// </summary>
        public bool IsFileOutput { get; set; }

        /// <summary>
        /// 出力方法 名前空間を取得または設定します。
        /// </summary>
        public string Namespace { get; set; } = string.Empty;

        /// <summary>
        /// 出力方法 クラス名を取得または設定します。
        /// </summary>
        public string ClassName { get; set; } = string.Empty;
        #endregion

        #region 画面出力
        /// <summary>
        /// 入力テキストを取得または設定します。
        /// </summary>
        [JsonIgnore]
        public string InputText { get; set; } = string.Empty;
        #endregion
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainSetting() { }

        /// <summary>
        /// コピーコンストラクタ
        /// </summary>
        /// <param name="mainSetting">メイン画面項目値</param>
        public MainSetting(MainSetting mainSetting)
        {
            IsGenerateProperty = mainSetting.IsGenerateProperty;
            IsGenerateViewModel = mainSetting.IsGenerateViewModel;
            IsGenerateConverted = mainSetting.IsGenerateConverted;
            IsViewModelStyleObservableProperty = mainSetting.IsViewModelStyleObservableProperty;
            IsViewModelStyleSetProperty = mainSetting.IsViewModelStyleSetProperty;
            IsNamingPascal = mainSetting.IsNamingPascal;
            IsNamingCamel = mainSetting.IsNamingCamel;
            IsNamingUpper = mainSetting.IsNamingUpper;
            IsNamingLower = mainSetting.IsNamingLower;
            IsNamingAsIs = mainSetting.IsNamingAsIs;
            IsSourceExcel = mainSetting.IsSourceExcel;
            IsSourceInput = mainSetting.IsSourceInput;
            ExcelFilePath = mainSetting.ExcelFilePath;
            ReadSheetName = mainSetting.ReadSheetName;
            ReadRowFrom = mainSetting.ReadRowFrom;
            ReadRowTo = mainSetting.ReadRowTo;
            LogicalNameColumn = mainSetting.LogicalNameColumn;
            PhysicsNameColumn = mainSetting.PhysicsNameColumn;
            DataTypeColumn = mainSetting.DataTypeColumn;
            NullableColumn = mainSetting.NullableColumn;
            IsScreenOutput = mainSetting.IsScreenOutput;
            IsFileOutput = mainSetting.IsFileOutput;
            Namespace = mainSetting.Namespace;
            ClassName = mainSetting.ClassName;
            InputText = mainSetting.InputText;
        }
        #endregion

        #region 公開メソッド
        #region Load
        /// <summary>
        /// メイン画面項目値を読み込みます。
        /// </summary>
        /// <returns>メイン画面項目値</returns>
        public static MainSetting Load()
        {
            var setting = new MainSetting();
            var filePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                nameof(EntityGenerator), $"{nameof(MainSetting)}{Constants.Extensions.JSON}");
            if (File.Exists(filePath))
            {
                try
                {
                    var json = File.ReadAllText(filePath, Encoding.UTF8);
                    setting = JsonSerializer.Deserialize<MainSetting>(json) ?? new MainSetting();
                }
                catch { }
            }

            return setting;
        }
        #endregion

        #region Save
        /// <summary>
        /// メイン画面項目値を保存します。
        /// </summary>
        public static void Save(MainSetting setting)
        {
            var dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(EntityGenerator));
            var filePath = Path.Combine(dir, $"{nameof(MainSetting)}{Constants.Extensions.JSON}");
            try
            {
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                var json = JsonSerializer.Serialize(setting, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
                });

                File.WriteAllText(filePath, json, Encoding.UTF8);
            }
            catch { }
        }
        #endregion
        #endregion
    }
}
