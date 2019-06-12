using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class HttpRequestPost : HttpRequestBase
{
    static HttpRequestPost Instance;

    public static HttpRequestPost getInstance
    {
        get
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("HttpRequestPost");
                DontDestroyOnLoad(go);
                Instance = go.AddComponent<HttpRequestPost>();
            }
            return Instance;
        }
    }

    void Start()
    {
        intro();
    }

    /// <summary>
    /// Requests the post.
    /// </summary>
    /// <returns><c>true</c>, if post was requested, <c>false</c> otherwise.</returns>
    /// <param name="info">Info.</param>
    /// <param name="timeout">Timeout.</param>
    public bool RequestPost(HttpBase info, int timeout = 10)
    {
        if (info.islog)
            Debug.Log("request post; url:" + info.url);
        if (this.CheckInterNet())
        {
            HttpData httpData = new HttpData();
            ++id;
            httpData.id = id;
            httpData.timeout = timeout;
            httpData.requetime = System.DateTime.Now;
            httpData.httpbase = info;
            httpData.httpRequest = HttpPost(httpData);
            httpdatas.Add(id, httpData);
            StartCoroutine(httpData.httpRequest);
            return true;
        }
        else
        {
            return false;
        }
    }

    /**POST*/
    private IEnumerator HttpPost(HttpData httpData)
    {
        UnityWebRequest request = new UnityWebRequest(httpData.httpbase.url, UnityWebRequest.kHttpVerbPOST);
        byte[] data = System.Text.Encoding.Default.GetBytes(httpData.httpbase.GetData());
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");
        Dictionary<string, string> header = httpData.httpbase.GetHeaders();
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
                Debug.Log("post back：" + webData);
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
            Debug.Log("POST END " + httpData.httpbase.url);
        }
    }
}


