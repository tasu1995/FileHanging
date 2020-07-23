using System;
using System.IO;
using System.Text;

namespace GetLastLine
{
    public class Logic
    {
        /// <summary> 今回出力されるデータの１行分の長さ </summary>
        public const int bufferSize = 1022;

        /// <summary> 分割用の識別文字コード </summary>
        public string[] separate = { "\r\n" };

        ///----------------------------------------------------
        /// <summary>
        /// ファイルの末尾から指定行数分、データを取得します。
        /// ファイルサイズが取得行数未満だった場合、当該ファイルから全行取得します。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="rowCnt">行数</param>
        /// <returns>指定行数分の文字列配列</returns>
        ///----------------------------------------------------
        private string[] GetEndLines(string filePath, int rowCnt)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // 直接参照すると値がばらつく為、変数に保持する
                var fsLength = (int)fs.Length;

                // 取得するオフセット長を取得
                var offset = fsLength < bufferSize * rowCnt ? fsLength : bufferSize * rowCnt;

                // ファイル末尾を開始位置にする
                fs.Seek(-offset, SeekOrigin.End);

                // 取得した行数分のデータをエンコードする
                using (var sr = new StreamReader(fs, Encoding.GetEncoding("Shift-Jis")))
                {
                    var getLines = sr.ReadToEnd();

                    // 改行コードで文字データを分割する
                    var lines = getLines.Split(separate, StringSplitOptions.None);
                    return lines;
                }
            }
        }
    }
}