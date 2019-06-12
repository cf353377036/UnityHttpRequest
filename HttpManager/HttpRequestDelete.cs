using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class HttpRequestDelete : HttpRequestBase
{
    static HttpRequestDelete Instance;

    public static HttpRequestDelete getInstance
    {
        get
        {
            if (Instance == null)
            {
                GameObject go = new GameObject("HttpRequestDelete");
                DontDestroyOnLoad(go);
                Instance = go.AddComponent<HttpRequestDelete>();
            }
            return Instance;
        }
    }

    void Start()
    {
        intro();
    }

    /// <summary>
    /// Requests the delete.
    /// </summary>
    /// <returns><c>true</c>, if delete was requested, <c>false</c> otherwise.</returns>
    /// <param name="httpBase">Http base.</param>
    /// <param name="timeout">Timeout.</param>
    public bool RequestDelete(HttpBase httpBase, int timeout = 10)
    {
        if (httpBase.islog)
        {
            Debug.Log("request delete; url" + httpBase.url);
        }
        if (this.CheckInterNet())
        {
            HttpData httpData = new HttpData();
            ++id;
            httpData.id = id;
            httpData.timeout = timeout;
            httpData.requetime = System.DateTime.Now;
            httpData.httpbase = httpBase;
            httpData.httpRequest = HttpDelete(httpData);
            httpdatas.Add(id, httpData);
            StartCoroutine(httpData.httpRequest);
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator HttpDelete(HttpData httpData)
    {
        UnityWebRequest request = new UnityWebRequest(httpData.httpbase.url, UnityWebRequest.kHttpVerbDELETE);
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
                Debug.Log("delete data：" + webData);
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
            Debug.Log("DELETE END " + httpData.httpbase.url);
        }
    }
}


