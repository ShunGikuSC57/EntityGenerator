using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EntityGenerator.Models;
using System.IO;
using System.Text;
using static EntityGenerator.Constants;

namespace EntityGenerator.ViewModels
{
    /// <summary>
    /// メイン画面ViewModel
    /// </summary>
    internal partial class MainViewModel : ViewModelBase
    {
        #region 定数
        /// <summary>
        /// 定義書とC#のデータ型ペア
        /// </summary>
        private Dictionary<string, string> DataTypePairs { get; set; } = DataType.Load();
        #endregion

        #region UIアクション
        /// <summary>
        /// Excelファイルパス選択を実装します。
        /// </summary>
        public Func<string, string, (bool Result, string FilePath)>? SelectFile { get; set; }

        /// <summary>
        /// ファイル保存を実装します。
        /// </summary>
        public Func<string, string, (bool Result, string FilePath)>? SaveFile { get; set; }
        #endregion

        #region ObservableProperty
        #region 生成対象
        /// <summary>
        /// 生成対象 プロパティ
        /// </summary>
        [ObservableProperty]
        private bool _isGenerateProperty;

        /// <summary>
        /// 生成対象 ViewModel
        /// </summary>
        [ObservableProperty]
        private bool _isGenerateViewModel;

        /// <summary>
        /// 生成対象 変換（命名規則に従って変換）
        /// </summary>
        [ObservableProperty]
        private bool _isGenerateConverted;
        #endregion

        #region ViewModel実装方式
        /// <summary>
        /// ViewModel実装方式 ObservableProperty
        /// </summary>
        [ObservableProperty]
        private bool _isViewModelStyleObservableProperty = true;

        /// <summary>
        /// ViewModel実装方式 SetProperty
        /// </summary>
        [ObservableProperty]
        private bool _isViewModelStyleSetProperty;
        #endregion

        #region 命名規則
        /// <summary>
        /// パスカルケース
        /// </summary>
        [ObservableProperty]
        private bool _isNamingPascal = true;

        /// <summary>
        /// キャメルケース
        /// </summary>
        [ObservableProperty]
        private bool _isNamingCamel;

        /// <summary>
        /// 大文字
        /// </summary>
        [ObservableProperty]
        private bool _isNamingUpper;

        /// <summary>
        /// 小文字
        /// </summary>
        [ObservableProperty]
        private bool _isNamingLower;

        /// <summary>
        /// 物理名のまま使用
        /// </summary>
        [ObservableProperty]
        private bool _isNamingAsIs;
        #endregion

        #region 変換対象
        /// <summary>
        /// 入力元 EXCEL
        /// </summary>
        [ObservableProperty]
        private bool _isSourceExcel = false;

        /// <summary>
        /// 入力元 直接入力
        /// </summary>
        [ObservableProperty]
        private bool _isSourceInput = true;
        #endregion

        #region EXCEL
        /// <summary>
        /// EXCEL ファイルパス
        /// </summary>
        [ObservableProperty]
        private string _excelFilePath = string.Empty;

        /// <summary>
        /// EXCEL 読み込む対象のシート名
        /// </summary>
        [ObservableProperty]
        private string _readSheetName = string.Empty;

        /// <summary>
        /// EXCEL 読み込む対象行数（自）
        /// </summary>
        [ObservableProperty]
        private int _readRowFrom;

        /// <summary>
        /// EXCEL 読み込む対象行数（至）
        /// </summary>
        [ObservableProperty]
        private int _readRowTo;

        /// <summary>
        /// EXCEL 論理名列数
        /// </summary>
        [ObservableProperty]
        private int _logicalNameColumn;

        /// <summary>
        /// EXCEL 物理名列数
        /// </summary>
        [ObservableProperty]
        private int _physicsNameColumn;

        /// <summary>
        /// EXCEL データ型列数
        /// </summary>
        [ObservableProperty]
        private int _dataTypeColumn;

        /// <summary>
        /// EXCEL Null制約列数
        /// </summary>
        [ObservableProperty]
        private int _nullableColumn;
        #endregion

        #region 出力方法
        /// <summary>
        /// 出力方法 画面出力
        /// </summary>
        [ObservableProperty]
        private bool _isScreenOutput;

        /// <summary>
        /// 出力方法 ファイル出力
        /// </summary>
        [ObservableProperty]
        private bool _isFileOutput;

        /// <summary>
        /// ファイルラジオボタンの有効/無効
        /// </summary>
        [ObservableProperty]
        private bool _isFileOutputEnabled = true;

        /// <summary>
        /// 出力方法 名前空間
        /// </summary>
        [ObservableProperty]
        private string _namespace = string.Empty;

        /// <summary>
        /// 出力方法 クラス名
        /// </summary>
        [ObservableProperty]
        private string _className = string.Empty;
        #endregion

        #region 画面出力
        /// <summary>
        /// 画面出力 入力テキスト
        /// </summary>
        [ObservableProperty]
        private string _inputText = string.Empty;

        /// <summary>
        /// 画面出力 出力テキスト
        /// </summary>
        [ObservableProperty]
        private string _outputText = string.Empty;
        #endregion
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainViewModel() => Initialize();
        #endregion

        #region RelayCommand
        #region SelectExcel
        /// <summary>
        /// ファイル選択
        /// </summary>
        [RelayCommand]
        private void SelectExcel()
        {
            if (SelectFile == null) return;
            var dialogResult = SelectFile(ExcelFilePath, FileFilter.EXCEL);
            if (dialogResult.Result) ExcelFilePath = dialogResult.FilePath;
        }
        #endregion

        #region SaveSpec
        /// <summary>
        /// 定義書出力
        /// </summary>
        [RelayCommand]
        private void SaveSpec()
        {
            if (SaveFile == null) return;
            const string SPEC_FILE_NAME = "テーブル定義書.xlsx";
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SPEC_FILE_NAME);
            if (!File.Exists(filePath)) return;

            try
            {
                using var workbook = new XLWorkbook(filePath);
                var dialogResult = SaveFile(SPEC_FILE_NAME, FileFilter.EXCEL);
                if (dialogResult.Result)
                {
                    workbook.SaveAs(dialogResult.FilePath);
                    ShowInfo?.Invoke(new MessageParam { Message = "定義書を出力しました。" });
                }
            }
            catch
            {
                ShowError?.Invoke(new MessageParam { Message = "定義書の出力に失敗しました" });
            }
        }
        #endregion

        #region OutputEntity
        /// <summary>
        /// エンティティ出力
        /// </summary>
        [RelayCommand]
        private void OutputEntity()
        {
            // 画面項目値取得
            MainSetting mainSetting = new()
            {
                IsGenerateProperty = IsGenerateProperty,
                IsGenerateViewModel = IsGenerateViewModel,
                IsGenerateConverted = IsGenerateConverted,
                IsViewModelStyleObservableProperty = IsViewModelStyleObservableProperty,
                IsViewModelStyleSetProperty = IsViewModelStyleSetProperty,
                IsNamingPascal = IsNamingPascal,
                IsNamingCamel = IsNamingCamel,
                IsNamingUpper = IsNamingUpper,
                IsNamingLower = IsNamingLower,
                IsNamingAsIs = IsNamingAsIs,
                IsSourceExcel = IsSourceExcel,
                IsSourceInput = IsSourceInput,
                ExcelFilePath = ExcelFilePath.Trim(),
                ReadSheetName = ReadSheetName.Trim(),
                ReadRowFrom = ReadRowFrom,
                ReadRowTo = ReadRowTo,
                LogicalNameColumn = LogicalNameColumn,
                PhysicsNameColumn = PhysicsNameColumn,
                DataTypeColumn = DataTypeColumn,
                NullableColumn = NullableColumn,
                IsScreenOutput = IsScreenOutput,
                IsFileOutput = IsFileOutput,
                Namespace = Namespace.Trim().Trim('.'),
                ClassName = ClassName.Trim().Trim('.'),
                InputText = InputText.Trim()
            };
            // 生成
            var result = new Services.EntityGeneratorService(mainSetting, DataTypePairs).Generate();
            if (result.ErrorMessages.Count > 0)
            {
                ShowError?.Invoke(new MessageParam { Messages = result.ErrorMessages });
                return;
            }
            // 出力
            var isOutput = false;
            if (IsScreenOutput)
            {
                OutputText = result.GeneratedCode;
                isOutput = true;
            }
            else
            {
                if (SaveFile != null)
                {
                    try
                    {
                        var className = $"{Path.GetFileNameWithoutExtension(result.OutputClassName)}{Extensions.CS}";
                        var dialogResult = SaveFile(className, FileFilter.CS);
                        if (dialogResult.Result)
                        {
                            File.WriteAllText(dialogResult.FilePath, result.GeneratedCode, Encoding.UTF8);
                            ShowInfo?.Invoke(new MessageParam { Message = $"{result.OutputClassName}を出力しました。" });
                            isOutput = true;
                        }
                    }
                    catch
                    {
                        ShowError?.Invoke(new MessageParam { Message = "CSファイルの出力に失敗しました" });
                    }
                }
            }
            // 画面項目値保存
            if (isOutput) MainSetting.Save(mainSetting);
        }
        #endregion

        #region ShowDataTypes
        /// <summary>
        /// データ型定義ファイルを表示します。
        /// </summary>
        [RelayCommand]
        private void ShowDataTypes()
        {
            try
            {
                var filePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    nameof(EntityGenerator),
                    DataType.DATA_DEF_FILE_NAME);
                if (File.Exists(filePath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = filePath,
                        UseShellExecute = true,
                        CreateNoWindow = true
                    });
                }
            }
            catch
            {
                ShowError?.Invoke(new MessageParam { Message = "型定義ファイルの表示に失敗しました。" });
            }
        }
        #endregion

        #region ReloadDataTypes
        /// <summary>
        /// データ型定義ファイルを再読み込みします。
        /// </summary>
        [RelayCommand]
        private void ReloadDataTypes()
        {
            DataTypePairs = DataType.Load();
            ShowInfo?.Invoke(new MessageParam { Message = "型定義ファイルの再読み込みが終了しました。" });
        }
        #endregion

        #region Exit
        /// <summary>
        /// 画面終了
        /// </summary>
        [RelayCommand]
        private void Exit() => Close?.Invoke();
        #endregion
        #endregion

        #region 内部メソッド
        #region Initialize
        /// <summary>
        /// 初期化
        /// </summary>
        private void Initialize()
        {
            var setting = MainSetting.Load();
            IsGenerateProperty = setting.IsGenerateProperty;
            IsGenerateViewModel = setting.IsGenerateViewModel;
            IsGenerateConverted = setting.IsGenerateConverted;
            IsViewModelStyleObservableProperty = setting.IsViewModelStyleObservableProperty;
            IsViewModelStyleSetProperty = setting.IsViewModelStyleSetProperty;
            IsNamingPascal = setting.IsNamingPascal;
            IsNamingCamel = setting.IsNamingCamel;
            IsNamingUpper = setting.IsNamingUpper;
            IsNamingLower = setting.IsNamingLower;
            IsNamingAsIs = setting.IsNamingAsIs;
            IsSourceExcel = setting.IsSourceExcel;
            IsSourceInput = setting.IsSourceInput;
            ExcelFilePath = setting.ExcelFilePath;
            ReadSheetName = setting.ReadSheetName;
            ReadRowFrom = setting.ReadRowFrom;
            ReadRowTo = setting.ReadRowTo;
            LogicalNameColumn = setting.LogicalNameColumn;
            PhysicsNameColumn = setting.PhysicsNameColumn;
            DataTypeColumn = setting.DataTypeColumn;
            NullableColumn = setting.NullableColumn;
            IsScreenOutput = setting.IsScreenOutput;
            IsFileOutput = setting.IsFileOutput;
            Namespace = setting.Namespace;
            ClassName = setting.ClassName;
        }
        #endregion
        #endregion
    }
}
