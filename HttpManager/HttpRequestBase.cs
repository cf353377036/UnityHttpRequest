using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HttpRequestBase : MonoBehaviour
{
    /// <summary>
    /// The identifier.
    /// </summary>
    protected int id = 0;
    /// <summary>
    /// The httpdatas.
    /// </summary>
    protected Dictionary<int, HttpData> httpdatas = new Dictionary<int, HttpData>();

    protected void intro()
    {
        StartCoroutine(Timer());
    }

    /// <summary>
    /// Timer this instance. check timeout
    /// </summary>
    /// <returns>The timer.</returns>
    protected IEnumerator Timer()
    {
        while (true)
        {
            System.TimeSpan ts1;
            Dictionary<int, HttpData> removeMap = new Dictionary<int, HttpData>();
            foreach (KeyValuePair<int, HttpData> item in httpdatas)
            {
                ts1 = System.DateTime.Now - item.Value.requetime;
                if (ts1.Seconds > item.Value.timeout)
                {
                    Debug.Log("request timeout：" + item.Value.httpbase.url);
                    StopCoroutine(item.Value.httpRequest);
                    removeMap.Add(item.Key, item.Value);
                    item.Value.httpbase.HttpRequesTimeout(item.Value);
                }
            }
            foreach (KeyValuePair<int, HttpData> item in removeMap)
            {
                httpdatas.Remove(item.Key);
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    /// <summary>
    /// 检查网络
    /// </summary>
    /// <returns></returns>
    protected bool CheckInterNet()
    {
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            return true;
        }
        else if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}


