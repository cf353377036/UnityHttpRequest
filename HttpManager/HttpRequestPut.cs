using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class HttpRequestPut : HttpRequestBase
{
    static HttpRequestPut Instance;

    public static HttpRequestPut getInstance
    {
        get
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("HttpRequestPut");
                DontDestroyOnLoad(go);
                Instance = go.AddComponent<HttpRequestPut>();
            }
            return Instance;
        }
    }

    void Start()
    {
        intro();
    }

    /// <summary>
    /// Requests the put.
    /// </summary>
    /// <returns><c>true</c>, if put was requested, <c>false</c> otherwise.</returns>
    /// <param name="info">Info.</param>
    /// <param name="timeout">Timeout.</param>
    public bool RequestPut(HttpBase info, int timeout = 10)
    {
        if (info.islog)
            Debug.Log("request put; url:" + info.url);
        if (this.CheckInterNet())
        {
            HttpData httpData = new HttpData();
            ++id;
            httpData.id = id;
            httpData.timeout = timeout;
            httpData.requetime = System.DateTime.Now;
            httpData.httpbase = info;
            httpData.httpRequest = HttpPut(httpData);
            httpdatas.Add(id, httpData);
            StartCoroutine(httpData.httpRequest);
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator HttpPut(HttpData httpData)
    {
        UnityWebRequest request = UnityWebRequest.Put(httpData.httpbase.url, httpData.httpbase.GetData());
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        Dictionary<string, string> header = httpData.httpbase.GetHeaders();
        if (!header.ContainsKey("Content-Type"))
        {
            header.Add("Content-Type", "application/json");
        }

        if (header != null)
        {
            foreach (KeyValuePair<string, string> post_arg in header)
            {
                request.SetRequestHeader(post_arg.Key, post_arg.Value);
            }
        }

        DateTime beginTime = DateTime.Now;
        yield return request.Send();
        httpData.backtime = DateTime.Now;
        httpData.durationtime = (httpData.backtime - beginTime).TotalMilliseconds;

        if (httpData.httpbase.islog)
        {
            Debug.Log("request time ：" + (httpData.backtime - beginTime).TotalMilliseconds);
            Debug.Log("responseCode：" + request.responseCode);
            Debug.Log("request web:" + request.downloadHandler.text);
        }
        if (request.isDone)
        {
            string webData = request.downloadHandler.text;
            if (httpData.httpbase.islog)
            {
                Debug.Log("put back：" + webData);
            }
            httpData.webData = webData;
            httpData.responseCode = request.responseCode;
            if (request.responseCode == 200)
            {
                httpData.httpbase.HttpRequesSuccess(httpData);
            }
            else
            {
                httpData.httpbase.HttpRequesFail(httpData);
            }
        }
        request.Dispose();

        if (httpdatas.ContainsKey(id))
        {
            httpdatas.Remove(id);
        }

        if (httpData.httpbase.islog)
        {
            Debug.Log("PUT END " + httpData.httpbase.url);
        }
    }
}


