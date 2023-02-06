using System.Collections.Generic;
using System.Reflection;
using System.IO;
using AmongUs.Data;

namespace NextMoreRoles.Modules;

class Translator
{
    public static int DefaultLanguage = (int)SupportedLangs.English;
    public static Dictionary<string, string[]> TranslateDatas;

    public static void Load()
    {
        TranslateDatas = new();

        var File = Assembly.GetExecutingAssembly().GetManifestResourceStream("NextMoreRoles.Resources.TranslateDatas.csv");
        StreamReader StreamReader = new(File);

        int CurrentLine = 0;
        while (!StreamReader.EndOfStream)
        {
            CurrentLine++;

            string Line = StreamReader.ReadLine();                          // CSVファイルの一行を読み込む
            if (Line == "" || Line[0] == '#') continue;                     // 読み込んだ一文字目が空白か#ならやり直し
            string[] Values = Line.Split(',');                              // 読み込んだ一行を "," 毎に分ける (ここを変えれば空白で仕切ったりもできる)

            List<string> ValuesList = new();
            foreach (string Value in Values)
            {
                ValuesList.Add(Value.Replace("\\n", "\n").Replace("，", ","));
            }

            TranslateDatas.Add(ValuesList[0], Values);                     // Key、Value(配列)として格納
        }
    }

    public static string GetString(string Key)
    {
        // キーが辞書になければKeyで返す
        if (!TranslateDatas.ContainsKey(Key))
        {
            Logger.Warn($"Keyが見つかりませんでした。Key:{Key}", "Translator");
            return Key;
        }

        // 言語を取得
        var LanguageId = TranslationController.InstanceExists ? TranslationController.Instance.currentLanguage.languageID : DataManager.Settings.Language.CurrentLanguage;

        // return
        return LanguageId switch
        {
            SupportedLangs.English => TranslateDatas[Key][1],
            SupportedLangs.Japanese => TranslateDatas[Key][2],
            SupportedLangs.SChinese => TranslateDatas[Key][3],
            _ => TranslateDatas[Key][1]
        };
    }
}
