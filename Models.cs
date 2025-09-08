using System;
using System.Collections.Generic;
using System.Text;

public class ImgList//图集列表
{
    public Album[] album;
    public int albumTotalPhoto;
    public int total;
}
public class Album
{
    public string id { get; set; }
    public string title { get; set; }
    public string updatetime { get; set; }
}

public class ImageModel //图集信息
{
    public int ret;
    public string msg { get; set; }
    public ImgData data;
}
public class ImgData
{
    //albuminfo
    public PhotoList[] photolist;
}

public class PhotoList
{
    public string sloc { get; set; }
    public Dictionary<string, photoA> photourl;

}

public class photoA
{
    public string url;
    public int width;
    public int height;
}

