using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Http base.
/// </summary>
public abstract class HttpBase
{
    /// <summary>
    /// url
    /// </summary>
    public string url;

    /// <summary>
    /// print log
    /// </summary>
    public bool islog;

    public delegate void HttpRequestSuccessDelegate(HttpData httpData);
    /// <summary>
    /// Http request success
    /// </summary>
    public event HttpRequestSuccessDelegate HttpRequestSuccessEvent;

    public delegate void HttpRequestFailDelegate(HttpData httpData);
    /// <summary>
    /// Http request fail
    /// </summary>
    public event HttpRequestFailDelegate HttpRequestFailEvent;

    public delegate void HttpRequestTimeoutDelegate(HttpData httpData);
    /// <summary>
    /// Http request timeout
    /// </summary>
    public event HttpRequestTimeoutDelegate HttpRequestTimeoutEvent;

    /// <summary>
    /// The headers.
    /// </summary>
    protected Dictionary<string, string> headers = new Dictionary<string, string>();
    /// <summary>
    /// The parameters.
    /// </summary>
    protected Dictionary<string, string> parameters = new Dictionary<string, string>();

    /// <summary>
    /// Gets the headers.
    /// </summary>
    /// <returns>The headers.</returns>
    public Dictionary<string, string> GetHeaders()
    {
        return headers;
    }

    /// <summary>
    /// Adds the header.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    public void AddHeader(string key, string value)
    {
        headers.Add(key, value);
    }

    /// <summary>
    /// Gets the request data.
    /// </summary>
    /// <returns>The data.</returns>
    public virtual string GetData()
    {
        return "";
    }

    /// <summary>
    /// Gets the parameters.
    /// </summary>
    /// <returns>The parameters.</returns>
    public virtual Dictionary<string, string> GetParameters()
    {
        return parameters;
    }


    /// <summary>
    /// Https the reques success. send HttpRequestSuccessEvent 
    /// </summary>
    /// <param name="httpData">Http data.</param>
    public virtual void HttpRequesSuccess(HttpData httpData)
    {
        if (HttpRequestSuccessEvent != null)
        {
            HttpRequestSuccessEvent(httpData);
        }
    }

    /// <summary>
    /// Https the reques fail. send HttpRequestFailEvent
    /// </summary>
    /// <param name="httpData">Http data.</param>
    public virtual void HttpRequesFail(HttpData httpData)
    {
        if (HttpRequestFailEvent != null)
        {
            HttpRequestFailEvent(httpData);
        }
    }

    /// <summary>
    /// Https the reques timeout. send HttpRequestTimeoutEvent
    /// </summary>
    /// <param name="httpData">Http data.</param>
    public virtual void HttpRequesTimeout(HttpData httpData)
    {
        if (HttpRequestTimeoutEvent != null)
        {
            HttpRequestTimeoutEvent(httpData);
        }
    }
}

