using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using Twitch;

namespace NextMoreRoles.Modules.Updater
{
    public class AutoUpdate
    {
        [HarmonyPatch(typeof(AnnouncementPopUp), nameof(AnnouncementPopUp.UpdateAnnounceText))]
        public static class Announcement
        {
            public static bool Prefix(AnnouncementPopUp __instance)
            {
                var text = __instance.AnnounceTextMeshPro;
                text.text = AnounceText;
                return false;
            }
        }
        public static string AnounceText;
        public static GenericPopup InfoPopup;
        private static bool IsLoad = false;
        public static string updateURL = null;
        public static void Load()
        {
            TwitchManager man = DestroyableSingleton<TwitchManager>.Instance;
            InfoPopup = UnityEngine.Object.Instantiate<GenericPopup>(man.TwitchPopup);
            InfoPopup.TextAreaTMP.fontSize *= 0.7f;
            InfoPopup.TextAreaTMP.enableAutoSizing = false;
        }
        public static async Task<bool> Update()
        {
            try
            {
                HttpClient http = new();
                http.DefaultRequestHeaders.Add("User-Agent", "NextMoreRoles Updater");
                var response = await http.GetAsync(new System.Uri(updateURL), HttpCompletionOption.ResponseContentRead);
                if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
                {
                    System.Console.WriteLine("Server returned no data: " + response.StatusCode.ToString());
                    return false;
                }
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                System.UriBuilder uri = new(codeBase);
                string fullname = System.Uri.UnescapeDataString(uri.Path);
                if (File.Exists(fullname + ".old")) // Clear old file in case it wasnt;
                    File.Delete(fullname + ".old");

                File.Move(fullname, fullname + ".old"); // rename current executable to old

                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    using var fileStream = File.Create(fullname);
                    // probably want to have proper name here
                    responseStream.CopyTo(fileStream);
                }
                NextMoreRolesPlugin.IsUpdate = true;
                return true;
            }
            catch (System.Exception ex)
            {
                NextMoreRolesPlugin.Instance.Log.LogError(ex.ToString());
                System.Console.WriteLine(ex);
            }
            return false;
        }
        public static async Task<bool> checkForUpdate(TMPro.TextMeshPro setdate)
        {
            if (!Configs.AutoUpdate.Value)
            {
                return false;
            }
            if (!IsLoad)
            {
                AutoUpdate.Load();
                IsLoad = true;
            }
            try
            {
                HttpClient http = new();
                http.DefaultRequestHeaders.Add("User-Agent", "NextMoreRoles Updater");
                var response = await http.GetAsync(new System.Uri("https://api.github.com/repos/UKON256/NextMoreRoles/releases/latest"), HttpCompletionOption.ResponseContentRead);
                if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
                {
                    System.Console.WriteLine("Server returned no data: " + response.StatusCode.ToString());
                    return false;
                }
                string json = await response.Content.ReadAsStringAsync();
                JObject data = JObject.Parse(json);

                string tagname = data["tag_name"]?.ToString();
                if (tagname == null)
                {
                    return false; // Something went wrong
                }
                string changeLog = data["body"]?.ToString();
                if (changeLog != null) AnounceText = changeLog;
                // check version
                NextMoreRolesPlugin.NewVersion = tagname.Replace("v", "");
                System.Version newver = System.Version.Parse(NextMoreRolesPlugin.NewVersion);
                System.Version Version = NextMoreRolesPlugin.Version;
                AnounceText = string.Format(ModTranslation.getString("MODUpdateAnnouncementMessage"), newver, AnounceText);
                if (newver == Version)
                {
                    NextMoreRolesPlugin.Logger.LogWarning("MODUpdate:最新バージョンです");
                }
                else
                {
                    NextMoreRolesPlugin.Logger.LogWarning("MODUpdate:古いバージョンです");
                    JToken assets = data["assets"];
                    if (!assets.HasValues)
                        return false;
                    for (JToken current = assets.First; current != null; current = current.Next)
                    {
                        string browser_download_url = current["browser_download_url"]?.ToString();
                        if (browser_download_url != null && current["content_type"] != null)
                        {
                            if (current["content_type"].ToString().Equals("application/x-msdownload") &&
                                browser_download_url.EndsWith(".dll"))
                            {
                                updateURL = browser_download_url;
                                await Update();
                                setdate.SetText(ModTranslation.getString("creditsMain") + "\n" + string.Format(ModTranslation.getString("creditsUpdateOk"), NextMoreRolesPlugin.NewVersion));
                            }
                        }
                    }
                }
                return false;
            }
            catch (System.Exception Error)
            {
                NextMoreRolesPlugin.Logger.LogError("MODUpdate:エラーが発生しました:" + Error);
                return false;
            }
        }
    }
}
