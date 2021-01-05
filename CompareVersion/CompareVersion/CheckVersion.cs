namespace CompareVersion
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using Xamarin.Essentials;

    public static class CheckVersion
    {

        public static async Task<bool> CheckVerisonIos()
        {

            // https://apps.apple.com/es/app/spotify-música-en-streaming/id324684580

            string url = "https://itunes.apple.com/lookup?id=324684580";

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (var response = await client.SendAsync(request).ConfigureAwait(false))
                {
                    try
                    {
                        string rep = await response.Content.ReadAsStringAsync();
                        var lookup = JsonConvert.DeserializeObject<Dictionary<string, object>>(rep);

                        if (lookup != null
                            && lookup.Count >= 1
                            && lookup["resultCount"] != null
                            && Convert.ToInt32(lookup["resultCount"].ToString()) > 0)
                        {
                            var results = JsonConvert.DeserializeObject<List<object>>(lookup[@"results"].ToString());

                            if (results != null && results.Count > 0)
                            {
                                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(results[0].ToString());
                                var version = values.ContainsKey("version") ? values["version"].ToString() : string.Empty;

                                return !version.Equals(VersionTracking.CurrentVersion);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public static async Task<bool> CheckVerisonAndroid()
        {
            var uri = new Uri($"https://play.google.com/store/apps/details?id=com.amazon.mShop.android.shopping&hl=es_419");

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, uri))
            {
                request.Headers.TryAddWithoutValidation("Accept", "text/html");
                request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                request.Headers.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");

                using (var response = await client.SendAsync(request).ConfigureAwait(false))
                {
                    try
                    {
                        response.EnsureSuccessStatusCode();
                        var responseHTML = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var rx = new Regex(@"(?<=""htlgb"">)(\d{1,3}\.\d{1,3}\.\d{0,1}\.\d{0,3})(?=<\/span>)", RegexOptions.Compiled);

                        MatchCollection matches = rx.Matches(responseHTML);
                        var version = matches.Count > 0 ? matches[0].Value : "Unknown";

                        return !version.Equals(VersionTracking.CurrentVersion);
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
        }
    }
}
