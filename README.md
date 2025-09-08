# 群相册爬取
- 在<code>Program.cs</code>中设置这些参数
```csharp
var result = await service.GetGroupPhotosAsync(
          gTk: "", 
          qunId: "",
          uin: "",
          qzoneToken: "",
          albumId: imgs.id,
          cookie: ""
        );
```
- 可通过访问<code>https://h5.qzone.qq.com/groupphoto/index?inqq=1&groupId=[要抓取相册的群号]</code>获取
- 群相册列表接口参数解析：
- URL: <code>https://h5.qzone.qq.com/proxy/domain/u.photo.qzone.qq.com/cgi-bin/upp/qun_list_album_v2</code>
- GET参数: ```g_tk,qzonetoken,callback,t,qunId,uin,start,num,getMemberRole,format,platform,inCharset,outCharset,source,cmd,qunid,```
- 可修改参数后携带COOKIE直接请求：https://h5.qzone.qq.com/proxy/domain/u.photo.qzone.qq.com/cgi-bin/upp/qun_list_album_v2?g_tk=[GTK]&qzonetoken=[空间TOKEN]&callback=shine0_Callback&t=238709507&qunId=[群号]&uin=[QQ号]&start=0&num=[获取的相册数]&getMemberRole=0&format=jsonp&platform=qzone&inCharset=utf-8&outCharset=utf-8&source=qzone&cmd=qunGetAlbumList&qunid=812978469&attach_info=start_count%3D20&callbackFun=shine0&_=1757320770891
