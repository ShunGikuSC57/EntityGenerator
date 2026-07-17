using System.Text.RegularExpressions;

namespace EntityGenerator
{
    /// <summary>
    /// 文字列拡張
    /// </summary>
    internal static class StringExtensions
    {
        #region 拡張メソッド
        #region SnakeToPascal
        /// <summary>
        /// スネークケースからパスカルケースへ変換します。
        /// </summary>
        /// <param name="snake">スネークケース文字</param>
        /// <returns>パスカルケース文字</returns>
        public static string SnakeToPascal(this string snake)
        {
            if (string.IsNullOrWhiteSpace(snake)) return string.Empty;
            var pascal = snake
                .ToLower()
                .Split(['_'], StringSplitOptions.RemoveEmptyEntries)
                .Select(s => char.ToUpperInvariant(s[0]) + s[1..])
                .Aggregate(string.Empty, (s1, s2) => s1 + s2);

            return pascal;
        }
        #endregion

        #region SnakeToCamel
        /// <summary>
        /// スネークケースからキャメルケースへ変換します。
        /// </summary>
        /// <param name="snake">スネークケース文字</param>
        /// <returns>キャメルケース文字</returns>
        public static string SnakeToCamel(this string snake)
        {
            if (string.IsNullOrWhiteSpace(snake)) return string.Empty;
            return snake.SnakeToPascal().PascalToCamel();
        }
        #endregion

        #region PascalToCamel
        /// <summary>
        /// パスカルケースからキャメルケースに変換します。
        /// </summary>
        /// <param name="pascal">パスカルケース文字</param>
        /// <returns>キャメルケース文字</returns>
        public static string PascalToCamel(this string pascal)
        {
            if (string.IsNullOrWhiteSpace(pascal)) return string.Empty;
            return char.ToLower(pascal[0]) + pascal[1..];
        }
        #endregion

        #region IsSafeNamespace
        /// <summary>
        /// 名前空間が有効な文字で構成されているか確認します。
        /// </summary>
        /// <param name="name">名前空間</param>
        /// <returns>有効な場合は true、それ以外は false</returns>
        /// <remarks>
        /// 空文字の場合本ロジックは通らずダミーの名前空間にするため、空文字は考慮しない。
        /// </remarks>
        public static (bool Result, string ErrorMessage) IsSafeNamespace(this string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return (false, "名前空間が空です。");
            string[] splitted = name.Trim().Split('.');
            foreach (var s in splitted)
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    return (false, "先頭か末尾もしくは連続してピリオドが使用されています。");
                }
                else
                {
                    var ret = IsSafeCsName(s);
                    if (!ret.Result) return (false, ret.ErrorMessage);
                }
            }

            return (true, string.Empty);
        }
        #endregion

        #region IsSafeClassName
        /// <summary>
        /// 文字列がC#で付与できる名称として有効な文字（a-z, A-Z, 0-9, _）のみで構成されているか検証します。
        /// </summary>
        /// <param name="name">クラス名</param>
        /// <returns>有効な場合は true、それ以外は false</returns>
        /// <remarks>
        /// 空文字の場合本ロジックは通らずダミーの名前空間にするため、空文字は考慮しない。
        /// </remarks>
        public static (bool Result, string ErrorMessage) IsSafeCsName(this string name)
        {
            var result = false;
            var errorMessage = string.Empty;
            if (!string.IsNullOrWhiteSpace(name))
            {
                var s = name.Trim();
                var pattern = @"^[a-zA-Z0-9_]*$";
                // まず大小アルファベットと数字とアンダースコアのみか確認する。
                result = Regex.IsMatch(s, pattern);
                // のみであった場合さらに先頭が数字でないかを確認する。
                if (result)
                {
                    if (char.IsDigit(s[0]))
                    {
                        result = false;
                        errorMessage = "先頭に数字は使用できません。";
                    }
                }
                else
                {
                    errorMessage = "半角英数とアンダーバーのみ入力可能です。";
                }
            }
            else
            {
                errorMessage = "クラス名が空です。";
            }

            return (result, errorMessage);
        }
        #endregion
        #endregion
    }
}
