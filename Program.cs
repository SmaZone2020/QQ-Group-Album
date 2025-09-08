using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;

public class QzonePhotoService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://h5.qzone.qq.com/groupphoto/inqq";

    public QzonePhotoService()
    {
        _httpClient = new HttpClient(new HttpClientHandler
        {
            UseCookies = true,
            AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
        });
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/javascript, */*; q=0.01");
        _httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8,en-GB;q=0.7,en-US;q=0.6");
        _httpClient.DefaultRequestHeaders.Add("sec-ch-ua", "\"Chromium\";v=\"140\", \"Not=A?Brand\";v=\"24\", \"Microsoft Edge\";v=\"140\"");
        _httpClient.DefaultRequestHeaders.Add("sec-ch-ua-mobile", "?0");
        _httpClient.DefaultRequestHeaders.Add("sec-ch-ua-platform", "\"Windows\"");
        _httpClient.DefaultRequestHeaders.Add("x-requested-with", "XMLHttpRequest");
    }

    public async Task<ImageModel> GetGroupPhotosAsync(
        string gTk,
        string qzoneToken,
        string albumId,
        string qunId,
        string uin,
        string cookie,
        int start = 0,
        int num = 36
        )
    {
        try
        {
            var url = $"{BaseUrl}?g_tk={gTk}&qzonetoken={qzoneToken}";

            var requestBody = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"qunId", qunId},
                {"albumId", albumId},
                {"uin", uin},
                {"start", start.ToString()},
                {"num", num.ToString()},
                {"getCommentCnt", "0"},
                {"getMemberRole", "1"},
                {"hostUin", uin},
                {"getalbum", "1"},
                {"platform", "qzone"},
                {"inCharset", "utf-8"},
                {"outCharset", "utf-8"},
                {"source", "qzone"},
                {"cmd", "qunGetPhotoList"},
                {"qunid", qunId},
                {"albumid", albumId},
                {"attach_info", ""}
            });

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = requestBody
            };

            request.Headers.Referrer = new Uri("https://h5.qzone.qq.com/groupphoto/index?inqq=1&groupId=812978469");

            if (!string.IsNullOrEmpty(cookie))
            {
                request.Headers.Add("Cookie", cookie);
            }

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            var imageModel = JsonConvert.DeserializeObject<ImageModel>(responseContent);

            return imageModel;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"请求失败: {ex.Message}");
            return null;
        }
    }
}

class Program
{
    public async static Task Main(string[] args)
    {
        var service = new QzonePhotoService();
        var imgsList = JsonConvert.DeserializeObject<ImgList>(File.ReadAllText("list.json"));
        string saveDir = @"img";
        Directory.CreateDirectory(saveDir);
        Console.WriteLine($"获取到 {imgsList.album.Length} 个图集");

        List<Task> downloadTasks = new();

        var i = 0; //图片计数器
        var a = 0; //图集计数器
        foreach (var imgs in imgsList.album)
        {
            Console.WriteLine($"开始抓取图集 [{imgs.title}],序号[{a++}]");
            var result = await service.GetGroupPhotosAsync(
                gTk: "",
                qunId: "",
                uin: "",
                qzoneToken: "",
                albumId: imgs.id,
                cookie: ""
            );

            if (result != null)
            {
                Console.WriteLine($"第{a}个图集,数量[{result.data.photolist.Length}]");
                i = 0;
                foreach (var photo in result.data.photolist)
                {
                    int photoIndex = 0;
                    foreach (var url in photo.photourl)
                    {
                        if (url.Value.width > 600 && url.Value.height > 600)
                        {
                            if (photoIndex % 3 == 0)
                            {
                                Console.WriteLine($"[图集{a}][第{i++}张][{url.Value.width},{url.Value.height}]");
                                _ = Task.Run(() =>
                                {
                                    WebClient wc = new();
                                    try
                                    {
                                        wc.DownloadFile(url.Value.url, $@"E:\Pictures\pcsuite\训练图集\all\{a}-{i}.jpg");
                                    }
                                    catch
                                    {

                                    }
                                });
                                await Task.Delay(50);
                            }
                            photoIndex++;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"请求失败");
            }
        }

    }
}