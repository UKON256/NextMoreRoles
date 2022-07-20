using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HarmonyLib;

namespace NextMoreRoles.Modules
{
    public class ModTranslation
    {
        public static int DefaultLanguage = (int)SupportedLangs.English;
        public static Dictionary<string, Dictionary<int, string>> TranslateDatas;

        //実行元:Main.cs
        public static void Load()
        {
            //変数
            int NowLine = 1;                                                    //現在の行

            //色々
            TranslateDatas = new();
            var Assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var Stream = Assembly.GetManifestResourceStream("NextMoreRoles.Resources.TranslateDatas.csv");
            StreamReader StreamReader = new(Stream);
            string[] Header = StreamReader.ReadLine().Split(',');

            // 末尾まで繰り返す
            while (!StreamReader.EndOfStream)
            {
                NowLine++;
                string Line = StreamReader.ReadLine();                          // CSVファイルの一行を読み込む
                if (Line == "" || Line[0] == '#') continue;                     // 読み込んだ行が空白か最初の文字が#ならやり直し
                string[] Values = Line.Split(',');                              // 読み込んだ一行を "," 毎に分ける (ここを変えれば空白で仕切ったりもできる)
                List<string> LinesLists = new(Values);                          // リストに内容を格納する
                Dictionary<int, string> Template = new();                       // Dictionary格納用のリスト作成
                //リスト内の翻訳を合わせる (List内:...{TestName}{Test}...なら{TestName,Test}にする)
                for (var i = 1; i < LinesLists.Count; ++i)
                {
                    if (LinesLists[i] != string.Empty && LinesLists[i].TrimStart()[0] == '"')
                    {
                        while (LinesLists[i].TrimEnd()[^1] != '"')
                        {
                            LinesLists[i] = LinesLists[i] + "," + LinesLists[i + 1];
                            LinesLists.RemoveAt(i + 1);
                        }
                    }
                }
                //もしリスト内の数と読み込んだ行の数が違えばエラーを出す
                if (LinesLists.Count != Header.Length)
                {
                    Logger.Warn($"CSVファイルの{NowLine}行目に誤りがあります。\n内容:{LinesLists}", "TranslateData");
                    continue;
                }
                //
                for (var i = 1; i < LinesLists.Count; i++)
                {
                    var tmp_str = LinesLists[i].Replace("\\n", "\n").Trim('"');
                    Template.Add(Int32.Parse(Header[i]), tmp_str);
                }
                //もし重複があれば警告
                if (TranslateDatas.ContainsKey(LinesLists[0])) { Logger.Warn($"翻訳用CSVに重複があります。\n{NowLine}行目: \"{LinesLists[0]}\"", "TranslateData"); continue; }

                TranslateDatas.Add(LinesLists[0], Template);
            }
        }

        //普通に取得(メインはこっち)
        public static string GetString(string Key, Dictionary<string, string> DictionaryReplacement = null)
        {
            var LanguageId = (SupportedLangs)SaveManager.LastLanguage;
            string String = GetString(Key, LanguageId);
            if (DictionaryReplacement != null)
            {
                foreach (var rd in DictionaryReplacement)
                {
                    String = String.Replace(rd.Key, rd.Value);
                }
            }
            return String;
        }

        //Dictionaryから取得
        public static string GetString(string Key, SupportedLangs LanguageId)
        {
            var ReturnString = "";
            if (TranslateDatas.TryGetValue(Key, out var Dictionary))
            {
                if (Dictionary.TryGetValue((int)LanguageId, out ReturnString))
                {
                    return ReturnString;
                }
                else
                {
                    if (Dictionary.TryGetValue(0, out ReturnString))
                    {
                        return ReturnString;
                    }
                    else
                    {
                        return $"<NotFound>{Key}";
                    }
                }
            }
            else
            {
                return $"<NotFound>{Key}";
            }
        }
    }

    [HarmonyPatch(typeof(LanguageSetter), nameof(LanguageSetter.SetLanguage))]
    class SetLanguagePatch
    {
        static void Postfix()
        {
            NextMoreRoles.Patches.SystemPatches.ClientOptionsPatch.UpdateTranslations();
        }
    }
}
