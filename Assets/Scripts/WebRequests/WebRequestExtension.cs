using System;

using UnityEngine.Networking;

namespace WebRequests
{
    public static class WebRequestExtension
    {
        public static bool Succeeded(this UnityWebRequest webRequest)
        {
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                case UnityWebRequest.Result.ProtocolError:
                    return false;
                case UnityWebRequest.Result.Success:
                    return true;
                case UnityWebRequest.Result.InProgress:
                    throw new Exception("Request is somehow still in progress.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}