using System.Collections;
using System.Collections.Generic;

public class HttpData
{
    /// <summary>
    /// 请求的id
    /// </summary>
    public int id = -1;
    /// <summary>
    /// 请求发送的时间
    /// </summary>
    public System.DateTime requetime;
    /// <summary>
    /// 请求返回时间
    /// </summary>
    public System.DateTime backtime;
    /// <summary>
    /// 请求时长
    /// </summary>
    public double durationtime;
    /// <summary>
    /// 超时时间
    /// </summary>
    public int timeout;
    /// <summary>
    /// 请求协程
    /// </summary>
    public IEnumerator httpRequest;
    /// <summary>
    /// 请求的信息
    /// </summary>
    public HttpBase httpbase;
    /// <summary>
    /// 返回原始数据
    /// </summary>
    public string webData;
    /// <summary>
    /// Http返回码
    /// </summary>
    public long responseCode;
}

