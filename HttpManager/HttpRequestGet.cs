using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class HttpRequestGet : HttpRequestBase
{
    static HttpRequestGet Instance;

    public static HttpRequestGet getInstance
    {
        get
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("HttpRequestGet");
                DontDestroyOnLoad(go);
                Instance = go.AddComponent<HttpRequestGet>();
            }
            return Instance;
        }
    }

    void Start()
    {
        intro();
    }

    /// <summary>
    /// Requests the get.
    /// </summary>
    /// <returns><c>true</c>, if get was requested, <c>false</c> otherwise.</returns>
    /// <param name="info">Info.</param>
    /// <param name="timeout">Timeout.</param>
    public bool RequestGet(HttpBase info, int timeout = 10)
    {
        if (info.islog)
            Debug.Log("begin request get; url:" + info.url);
        if (this.CheckInterNet())
        {
            HttpData httpData = new HttpData();
            ++id;
            httpData.id = id;
            httpData.timeout = timeout;
            httpData.requetime = System.DateTime.Now;
            httpData.httpbase = info;
            httpData.httpRequest = HttpGet(httpData);
            httpdatas.Add(id, httpData);
            StartCoroutine(httpData.httpRequest);
            return true;
        }
        else
        {
            return false;
        }
    }

    /**GET*/
    private IEnumerator HttpGet(HttpData httpData)
    {
        string Parameters;
        bool first = true;

        Dictionary<string, string> paramterBegin = httpData.httpbase.GetParameters();
        if (paramterBegin != null && paramterBegin.Count > 0)
        {
            Parameters = "?";
            foreach (KeyValuePair<string, string> post_arg in paramterBegin)
            {
                if (first)
                    first = false;
                else
                    Parameters += "&";
                Parameters += post_arg.Key + "=" + post_arg.Value;
            }
            Parameters = "?data=" + httpData.httpbase.GetData();
        }
        else
        {
            Parameters = httpData.httpbase.GetData();
            Parameters = "?data=" + Parameters;
        }

        UnityWebRequest request = new UnityWebRequest(httpData.httpbase.url + Parameters, UnityWebRequest.kHttpVerbGET);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        Dictionary<string, string> header = httpData.httpbase.GetHeaders();
        if (!header.ContainsKey("Accept"))
        {
            header.Add("Accept", "application/json");
        }

        if (!header.ContainsKey("Content-Type"))
        {
            header.Add("Content-Type", "application/json;charset=UTF-8");
        }
        if (!header.ContainsKey("If-None-Match"))
        {
            header.Add("If-None-Match", "");
        }

        if (httpData.httpbase.islog)
        {
            Debug.Log("request get :" + httpData.httpbase.url + Parameters);
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
                Debug.Log("get back：" + webData);
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
            Debug.Log("get end " + httpData.httpbase.url);
        }
    }
}


