using ClosedXML.Excel;
using EntityGenerator.Models;
using System.IO;
using System.Text;
using static EntityGenerator.Constants;

namespace EntityGenerator.Services
{
    /// <summary>
    /// エンティティ生成サービス
    /// </summary>
    internal sealed class EntityGeneratorService
    {

        #region プロパティ
        /// <summary>
        /// メイン画面項目値を取得します。
        /// </summary>
        private MainSetting MainSetting { get; }

        /// <summary>
        /// 定義書とC#のデータ型ペアを取得します。
        /// </summary>
        private Dictionary<string, string> DataTypePairs { get; }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="mainSetting">メイン画面項目値</param>
        /// <param name="dataTypePairs">定義書とC#のデータ型ペア</param>
        public EntityGeneratorService(MainSetting mainSetting, Dictionary<string, string> dataTypePairs)
        {
            MainSetting = new(mainSetting);
            DataTypePairs = new(dataTypePairs);
        }
        #endregion

        #region 公開メソッド
        /// <summary>
        /// エンティティ生成
        /// </summary>
        /// <returns>エンティティ生成結果</returns>
        public EntityGeneratorResult Generate()
        {
            EntityGeneratorResult result = new();
            var ns = nameof(MainSetting.Namespace);
            result.OutputClassName = nameof(MainSetting.ClassName);

            #region バリデーション
            if (MainSetting.IsSourceExcel)
            {
                // ファイルパス
                if (string.IsNullOrWhiteSpace(MainSetting.ExcelFilePath))
                    result.ErrorMessages.Add("EXCEL ファイルを選択してください。");
                else if (!File.Exists(MainSetting.ExcelFilePath))
                    result.ErrorMessages.Add("EXCEL ファイルが存在しません。");
                else if (Path.GetExtension(MainSetting.ExcelFilePath) != Extensions.EXCEL)
                    result.ErrorMessages.Add("EXCEL ファイルを指定してください。");
                // 読込シート名
                if (string.IsNullOrWhiteSpace(MainSetting.ReadSheetName))
                    result.ErrorMessages.Add("EXCEL 読込シート名を入力してください。");
                // 行数
                if (MainSetting.ReadRowFrom > MainSetting.ReadRowTo)
                    result.ErrorMessages.Add("EXCEL 読込対象行の終了は、開始より大きな値を指定してください。");
            }
            else if (MainSetting.IsSourceInput)
            {
                if (string.IsNullOrWhiteSpace(MainSetting.InputText))
                    result.ErrorMessages.Add("変換対象の文字列を入力してください。");
            }
            else
            {
                result.ErrorMessages.Add("変換対象を選択してください。");
                return result;
            }

            if (MainSetting.IsFileOutput)
            {
                // 名前空間
                if (!string.IsNullOrWhiteSpace(MainSetting.Namespace))
                {
                    var ret = MainSetting.Namespace.IsSafeNamespace();
                    if (!ret.Result)
                        result.ErrorMessages.Add($"名前空間 {ret.ErrorMessage}");
                    else
                        ns = MainSetting.Namespace.Trim();
                }
                // クラス名
                if (!string.IsNullOrWhiteSpace(MainSetting.ClassName))
                {
                    var ret = MainSetting.ClassName.IsSafeCsName();
                    if (!ret.Result)
                        result.ErrorMessages.Add($"クラス名 {ret.ErrorMessage}");
                    else
                        result.OutputClassName = MainSetting.ClassName.Trim();
                }
            }

            if (result.ErrorMessages.Count > 0) return result;
            #endregion

            #region 読み込み
            List<ConvertItem> converts = [];
            if (MainSetting.IsSourceExcel)
            {
                try
                {
                    // EXCEL読み込み
                    using var book = new XLWorkbook(MainSetting.ExcelFilePath);
                    var sheet = book.Worksheet(MainSetting.ReadSheetName);
                    if (sheet == null)
                    {
                        result.ErrorMessages.Add("シートの取得に失敗しました。");
                        return result;
                    }
                    for (var i = MainSetting.ReadRowFrom; i <= MainSetting.ReadRowTo; i++)
                    {
                        ConvertItem item = new()
                        {
                            LogicalName = sheet.Cell(i, MainSetting.LogicalNameColumn).GetValue<string>().Trim(),
                            PhysicsName = sheet.Cell(i, MainSetting.PhysicsNameColumn).GetValue<string>().Trim(),
                            DataType = GetDataType(sheet.Cell(i, MainSetting.DataTypeColumn).GetValue<string>()).Trim(),
                            IsNotNull = !string.IsNullOrWhiteSpace(sheet.Cell(i, MainSetting.NullableColumn).GetValue<string>())
                        };
                        // 物理名なしは処理が成り立たないため対象外
                        if (string.IsNullOrWhiteSpace(item.PhysicsName)) continue;
                        converts.Add(item);
                    }
                }
                catch
                {
                    result.ErrorMessages.Add("EXCELの読み込みに失敗しました。");
                    result.ErrorMessages.Add("開いている場合はファイルを閉じてください。");
                    return result;
                }
            }
            else
            {
                try
                {
                    converts = MainSetting.InputText
                        .Trim()
                        .Split(["\r\n", "\n", "\r"], StringSplitOptions.None)
                        .Where(i => !string.IsNullOrWhiteSpace(i))?
                        .Select(i => new ConvertItem { PhysicsName = i.Trim() })
                        .ToList() ?? [];
                }
                catch
                {
                    result.ErrorMessages.Add("入力値の取得に失敗しました。");
                    return result;
                }
            }

            if (converts.Count <= 0)
            {
                result.ErrorMessages.Add("変換対象が存在しませんでした。");
                return result;
            }
            else
            {
                for (var i = 0; i < converts.Count; i++)
                {
                    var convert = converts[i];
                    var ret = convert.PhysicsName.IsSafeCsName();
                    if (!ret.Result)
                        result.ErrorMessages.Add($"変換対象{i + 1}行目 {ret.ErrorMessage}");
                }

                if (result.ErrorMessages.Count > 0) return result;
            }
            #endregion

            #region 変換
            // 変換ファンクション実装
            Func<string, string> conversionFunc = (pysicsName) => { return pysicsName; };
            if (MainSetting.IsNamingPascal)
                conversionFunc = (pysicsName) => { return pysicsName.SnakeToPascal(); };
            else if (MainSetting.IsNamingCamel)
                conversionFunc = (pysicsName) => { return pysicsName.SnakeToCamel(); };
            else if (MainSetting.IsNamingUpper)
                conversionFunc = (pysicsName) => { return pysicsName.ToUpper(); };
            else if (MainSetting.IsNamingLower)
                conversionFunc = (pysicsName) => { return pysicsName.ToLower(); };
            // 生成アクション実装
            Action<ConvertItem, List<string>> generateAction = (item, outputStrings) => outputStrings.Add(item.ConvertedName);
            if (MainSetting.IsGenerateProperty)
                generateAction = SetProperty;
            else if (MainSetting.IsGenerateViewModel)
                generateAction = SetViewModel;
            List<string> outputStrings = [];
            var isFirst = true;
            foreach (var item in converts)
            {
                if (!isFirst && !MainSetting.IsGenerateConverted) outputStrings.Add(string.Empty);
                isFirst = false;
                item.ConvertedName = conversionFunc(item.PhysicsName);
                generateAction(item, outputStrings);
            }
            #endregion

            #region コード生成
            if (outputStrings.Count > 0)
            {
                if (MainSetting.IsScreenOutput)
                    result.GeneratedCode = string.Join(Environment.NewLine, outputStrings);
                else
                    result.GeneratedCode = CreateCS(outputStrings, ns, result.OutputClassName);
            }
            else
            {
                result.ErrorMessages.Add("変換対象が存在しませんでした。");
            }
            #endregion

            return result;
        }
        #endregion

        #region 内部メソッド
        #region GetDataType
        /// <summary>
        /// 定義書のデータ型からC#のデータ型を取得します。
        /// </summary>
        /// <param name="defDataType">定義書のデータ型</param>
        /// <returns>C#データ型</returns>
        private string GetDataType(string defDataType)
        {
            DataTypePairs.TryGetValue(defDataType, out string? dataType);
            return dataType ?? DEFAULT_DATA_TYPE;
        }
        #endregion

        #region SetProperty
        /// <summary>
        /// プロパティに必要な文字列を出力文字列に追加します。
        /// </summary>
        /// <param name="item">変換項目</param>
        /// <param name="outputStrings">出力文字列</param>
        private void SetProperty(ConvertItem item, List<string> outputStrings)
        {
            outputStrings.Add("/// <summary>");
            outputStrings.Add($"/// {item.LogicalName}を取得または設定します。");
            outputStrings.Add("/// </summary>");
            var nullable = item.IsNotNull ? string.Empty : "?";
            var initialize = string.Empty;
            if (item.IsNotNull) initialize = GetDefaultValueString(item.DataType);
            outputStrings.Add($"public {item.DataType}{nullable} {item.ConvertedName} {{ get; set; }}{initialize}");
        }
        #endregion

        #region GetDefaultValue
        /// <summary>
        /// C#データ型から初期化代入値文字列を取得します。
        /// </summary>
        /// <param name="dataType">C#データ型文字列</param>
        /// <returns>初期化代入値文字列</returns>
        private string GetDefaultValueString(string dataType)
        {
            DEFAULT_VALUE_STRING_PAIRS.TryGetValue(dataType, out string? defaultValue);
            return defaultValue ?? string.Empty;
        }
        #endregion

        #region SetViewModel
        /// <summary>
        /// ViewModelに必要な文字列を出力文字列に追加します。
        /// </summary>
        /// <param name="item">変換項目</param>
        /// <param name="outputStrings">出力文字列</param>
        private void SetViewModel(ConvertItem item, List<string> outputStrings)
        {
            var initialize = string.Empty;
            if (item.IsNotNull) initialize = GetDefaultValueString(item.DataType);
            var nullable = item.IsNotNull ? string.Empty : "?";
            var backingField = MainSetting.IsNamingPascal ? item.ConvertedName.PascalToCamel() : item.ConvertedName;
            backingField = $"_{backingField}";
            if (MainSetting.IsViewModelStyleObservableProperty)
            {
                outputStrings.Add("/// <summary>");
                outputStrings.Add($"/// {item.LogicalName}");
                outputStrings.Add("/// </summary>");
                outputStrings.Add("[ObservableProperty]");
                if (string.IsNullOrEmpty(initialize))
                    outputStrings.Add($"private {item.DataType}{nullable} {backingField};");
                else
                    outputStrings.Add($"private {item.DataType}{nullable} {backingField}{initialize}");
            }
            else
            {
                outputStrings.Add("/// <summary>");
                outputStrings.Add($"/// {item.LogicalName}を取得または設定します。");
                outputStrings.Add("/// </summary>");
                outputStrings.Add($"public {item.DataType}{nullable} {item.ConvertedName}");
                outputStrings.Add("{");
                outputStrings.Add($"    get => {backingField};");
                outputStrings.Add($"    set");
                outputStrings.Add("    {");
                outputStrings.Add($"        if ({backingField} == value) return;");
                outputStrings.Add($"        if (SetProperty(ref {backingField}, value))");
                outputStrings.Add("        {");
                outputStrings.Add("        }");
                outputStrings.Add("    }");
                outputStrings.Add("}");
                if (string.IsNullOrEmpty(initialize))
                    outputStrings.Add($"private {item.DataType}{nullable} {backingField};");
                else
                    outputStrings.Add($"private {item.DataType}{nullable} {backingField}{initialize}");
            }
        }
        #endregion

        #region CreateCS
        /// <summary>
        /// CSファイル形式の文字列を作成します。
        /// </summary>
        /// <param name="outputStrings">出力文字列</param>
        /// <param name="ns">名前空間</param>
        /// <param name="className">クラス名</param>
        /// <returns></returns>
        private string CreateCS(List<string> outputStrings, string ns, string className)
        {
            // クラス内容組み立て
            StringBuilder cs = new();
            if (MainSetting.IsGenerateViewModel)
            {
                cs.AppendLine("using CommunityToolkit.Mvvm.ComponentModel;");
                cs.AppendLine("using CommunityToolkit.Mvvm.Input;");
                cs.AppendLine(string.Empty);
                cs.AppendLine($"namespace {ns}");
                cs.AppendLine("{");
                cs.AppendLine($"    public partial class {className}");
                cs.AppendLine("    {");
                cs.AppendLine("        #region ObservableProperty");
            }
            else
            {
                cs.AppendLine($"namespace {ns}");
                cs.AppendLine("{");
                cs.AppendLine($"    public class {className}");
                cs.AppendLine("    {");
                cs.AppendLine("        #region プロパティ");
            }
            foreach (var str in outputStrings)
            {
                if (!string.IsNullOrWhiteSpace(str))
                    cs.AppendLine($"        {str}");
                else
                    cs.AppendLine(string.Empty);
            }
            cs.AppendLine("        #endregion");
            cs.AppendLine("    }");
            cs.AppendLine("}");

            return cs.ToString();
        }
        #endregion
        #endregion
    }
}
