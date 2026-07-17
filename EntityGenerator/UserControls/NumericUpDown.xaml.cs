using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EntityGenerator.UserControls
{
    /// <summary>
    /// NumericUpDownコントロール
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        #region 依存関係プロパティ
        #region 値
        /// <summary>
        /// 値を取得または設定します。
        /// </summary>
        public int? Value
        {
            get { return (int?)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// 値プロパティ
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(int?),
            typeof(NumericUpDown),
            new PropertyMetadata(0, OnValuePropertyChanged, CoerceValue));

        /// <summary>
        /// 値変更直後イベント
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// 定義飲み、実装なし
        /// </remarks>
        private static void OnValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) { }

        /// <summary>
        /// 値の変更直前に値の検証を行い矛盾があれば値を書き換えて解消します。
        /// </summary>
        /// <param name="d">NumericUpDownコントロール</param>
        /// <param name="basaValue">値</param>
        /// <returns>値</returns>
        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            var result = 0;
            if (d is NumericUpDown ud)
            {
                if (int.TryParse(baseValue?.ToString(), out int value))
                {
                    if (value < ud.MinValue) value = ud.MinValue ?? 0;
                    else if (value > ud.MaxValue) value = ud.MaxValue ?? int.MaxValue;
                    result = value;
                }
                else
                {
                    result = ud.MinValue ?? 0;
                }
            }

            return result;
        }
        #endregion

        #region 下限値
        /// <summary>
        /// 下限値を取得または設定します。
        /// </summary>
        public int? MinValue
        {
            get { return (int?)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        /// <summary>
        /// 下限値プロパティ
        /// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            nameof(MinValue),
            typeof(int?),
            typeof(NumericUpDown),
            new PropertyMetadata(0, OnMinValuePropertyChanged, CoerceMinValue));

        /// <summary>
        /// 下限値変更直後イベント
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnMinValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumericUpDown ud && int.TryParse(e.NewValue?.ToString(), out int min))
                if (min > ud.Value)
                    ud.Value = min;
        }

        /// <summary>
        /// 下限値の変更直前に値の検証を行い矛盾があれば値を書き換えて解消します。
        /// </summary>
        /// <param name="d">NumericUpDownコントロール</param>
        /// <param name="basaValue">下限値</param>
        /// <returns>下限値</returns>
        private static object CoerceMinValue(DependencyObject d, object baseValue)
        {
            var result = 0;
            if (d is NumericUpDown ud)
            {
                if (int.TryParse(baseValue?.ToString(), out int min))
                {
                    if (min > ud.MaxValue) result = ud.MaxValue ?? int.MaxValue;
                    else result = min;
                }
                else
                {
                    result = ud.MinValue ?? int.MaxValue;
                }
            }

            return result;
        }
        #endregion

        #region 上限値
        /// <summary>
        /// 上限値を取得または設定します。
        /// </summary>
        public int? MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// 上限値プロパティ
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            nameof(MaxValue),
            typeof(int?),
            typeof(NumericUpDown),
            new PropertyMetadata(int.MaxValue, OnMaxValuePropertyChanged, CoerceMaxValue));

        /// <summary>
        /// 上限値の変更直前に値の検証を行い矛盾があれば値を書き換えて解消します。
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnMaxValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NumericUpDown ud && int.TryParse(e.NewValue?.ToString(), out int max))
                if (max < ud.Value)
                    ud.Value = max;
        }

        /// <summary>
        /// 上限値の変更直前に値の検証を行い矛盾があれば値を書き換えて解消します。
        /// </summary>
        /// <param name="d">NumericUpDownコントロール</param>
        /// <param name="basaValue">上限値</param>
        /// <returns>上限値</returns>
        private static object CoerceMaxValue(DependencyObject d, object baseValue)
        {
            var result = 0;
            if (d is NumericUpDown ud)
            {
                if (int.TryParse(baseValue?.ToString(), out int max))
                {
                    if (max < ud.MinValue) max = ud.MinValue ?? 0;
                    result = max;
                }
                else
                {
                    result = ud.MaxValue ?? int.MaxValue;
                }
            }

            return result;
        }
        #endregion

        #region ボタン変更値
        /// <summary>
        /// ボタン変更値を取得または設定します。
        /// </summary>
        public int ButtonChange
        {
            get { return (int)GetValue(ButtonChangeProperty); }
            set { SetValue(ButtonChangeProperty, value); }
        }

        /// <summary>
        /// ボタン変更値プロパティ
        /// </summary>
        public static readonly DependencyProperty ButtonChangeProperty = DependencyProperty.Register(
            nameof(ButtonChange),
            typeof(int),
            typeof(NumericUpDown),
            new PropertyMetadata(1));
        #endregion

        #region ホイール変更値
        /// <summary>
        /// ホイール変更値を取得または設定します。
        /// </summary>
        public int WheelChange
        {
            get { return (int)GetValue(WheelChangeProperty); }
            set { SetValue(WheelChangeProperty, value); }
        }

        /// <summary>
        /// ホイール変更値プロパティ
        /// </summary>
        public static readonly DependencyProperty WheelChangeProperty = DependencyProperty.Register(
            nameof(WheelChange),
            typeof(int),
            typeof(NumericUpDown),
            new PropertyMetadata(1));
        #endregion
        #endregion

        #region コンストラクタ
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NumericUpDown() => InitializeComponent();
        #endregion

        #region イベントハンドラ
        #region TextBox_PreviewKeyDown
        /// <summary>
        /// テキストボックスキーダウンイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// スペースキーが押されたのを無効にする
        /// </remarks>
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }
        #endregion

        #region TextBox_PreviewTextInput
        /// <summary>
        /// テキストボックスキー入力確定前イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// 入力の制限、数字とハイフンとピリオドだけ通す
        /// </remarks>
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (sender is TextBox)
            {
                var input = e.Text;
                // 数値のみ入力可
                if (!new System.Text.RegularExpressions.Regex("[0-9]").IsMatch(input))
                    e.Handled = true;
            }
        }
        #endregion

        #region TextBox_PreviewExecuted
        /// <summary>
        /// テキスト実行前イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// 整数以外貼り付け無効
        /// </remarks>
        private void TextBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {
                if (int.TryParse(Clipboard.GetText(), out int value))
                {
                    if (value < MinValue || MaxValue < value)
                        e.Handled = true;
                }
                else
                {
                    e.Handled = true;
                }
            }
        }
        #endregion

        #region TextBox_GotFocus
        /// <summary>
        /// テキストボックスフォーカスイン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// フォーカスしたときにテキストを全選択する。
        /// </remarks>
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb) tb.SelectAll();
        }
        #endregion

        #region TextBox_PreviewMouseLeftButtonDown
        /// <summary>
        /// テキスト左クリック前イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// クリックしたときにテキストを全選択する。
        /// </remarks>
        private void TextBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.Focus();
                e.Handled = true;
            }
        }
        #endregion

        #region RepeatButtonUp_Click
        /// <summary>
        /// カウントアップボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeatButtonUp_Click(object sender, RoutedEventArgs e) => Value += ButtonChange;
        #endregion

        #region RepeatButtonDown_Click
        /// <summary>
        /// カウントダウンボタンクリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeatButtonDown_Click(object sender, RoutedEventArgs e) => Value -= ButtonChange;
        #endregion

        #region RepeatButton_MouseWheel
        /// <summary>
        /// アップダウンボタンマウスホイールイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RepeatButton_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0) Value -= WheelChange;
            else Value += WheelChange;
        }
        #endregion

        #region TextBox_MouseWheel
        /// <summary>
        /// テキストボックスマウスホイールイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0) Value -= WheelChange;
            else Value += WheelChange;
        }
        #endregion
        #endregion
    }
}
