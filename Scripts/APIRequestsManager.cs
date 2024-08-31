using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace APIRequests
{
    public class APIRequestsManager : MonoBehaviour
    {
        public static APIRequestsManager Instance;
        private string userIdToken;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void SetUserIdToken(string _userIdToken)
        {
            userIdToken = _userIdToken;
        }

        public void Get(string _uri, Action<string> _onSuccess=null, Action<string> _onError=null)
        {
            StartCoroutine(Routine());
            IEnumerator Routine()
            {
                if (userIdToken != null)
                {
                    _uri = $"{_uri}?auth={userIdToken}";
                }

                using UnityWebRequest _webRequest = UnityWebRequest.Get(_uri);
                yield return _webRequest.SendWebRequest();

                if (_webRequest.result == UnityWebRequest.Result.Success)
                {
                    _onSuccess?.Invoke(_webRequest.downloadHandler.text);
                }
                else
                {
                    _onError?.Invoke(WebRequestToErrorMessage(_webRequest));;
                }

                _webRequest.Dispose();
            }
        }

        public void Post(string _uri, string _jsonData, Action<string> _onSuccess=null, Action<string> _onError=null, bool _includeHeader = true)
        {
            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                if (userIdToken != null)
                {
                    if (_includeHeader)
                    {
                        _uri = $"{_uri}?auth={userIdToken}";
                    }
                }

                using UnityWebRequest _webRequest = UnityWebRequest.PostWwwForm(_uri, _jsonData);
                byte[] _jsonToSend = new System.Text.UTF8Encoding().GetBytes(_jsonData);
                _webRequest.uploadHandler = new UploadHandlerRaw(_jsonToSend);
                _webRequest.downloadHandler = new DownloadHandlerBuffer();

                yield return _webRequest.SendWebRequest();

                if (_webRequest.result == UnityWebRequest.Result.Success)
                {
                    _onSuccess?.Invoke(_webRequest.downloadHandler.text);
                }
                else
                {
                    _onError?.Invoke(WebRequestToErrorMessage(_webRequest));
                }

                _webRequest.Dispose();
            }
        }

        public void Put(string _uri, string _jsonData, Action<string> _onSuccess=null, Action<string> _onError=null)
        {
            StartCoroutine(Routine());

            IEnumerator Routine()
            {

                if (userIdToken != null)
                {
                    _uri = $"{_uri}?auth={userIdToken}";
                }

                using UnityWebRequest _webRequest = UnityWebRequest.Put(_uri, _jsonData);
                byte[] _jsonToSend = new System.Text.UTF8Encoding().GetBytes(_jsonData);
                _webRequest.uploadHandler = new UploadHandlerRaw(_jsonToSend);
                _webRequest.downloadHandler = new DownloadHandlerBuffer();

                yield return _webRequest.SendWebRequest();

                if (_webRequest.result == UnityWebRequest.Result.Success)
                {
                    _onSuccess?.Invoke(_webRequest.downloadHandler.text);
                }
                else
                {
                    _onError?.Invoke(WebRequestToErrorMessage(_webRequest));
                    ;
                }

                _webRequest.Dispose();
            }
        }

        public void Patch(string _uri, string _jsonData, Action<string> _onSuccess=null, Action<string> _onError=null)
        {
            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                if (userIdToken != null)
                {
                    _uri = $"{_uri}?auth={userIdToken}";
                }

                using UnityWebRequest _webRequest = new UnityWebRequest(_uri, "PATCH");
                byte[] _jsonToSend = new System.Text.UTF8Encoding().GetBytes(_jsonData);
                _webRequest.uploadHandler = new UploadHandlerRaw(_jsonToSend);
                _webRequest.downloadHandler = new DownloadHandlerBuffer();


                yield return _webRequest.SendWebRequest();

                if (_webRequest.result == UnityWebRequest.Result.Success)
                {
                    _onSuccess?.Invoke(_webRequest.downloadHandler.text);
                }
                else
                {
                    _onError?.Invoke(WebRequestToErrorMessage(_webRequest));
                }

                _webRequest.Dispose();
            }
        }

        private string WebRequestToErrorMessage(UnityWebRequest _webRequest)
        {
            return $"Error: {_webRequest.error}\nMessage: {_webRequest.downloadHandler.text}";
        }
    }
}