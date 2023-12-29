using System;
using System.Threading;
using BZ_FIG_SDK.Scripts;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Utils
{
    public class UniTaskWebRequest : Singleton<UniTaskWebRequest>
    {
        // Start is called before the first frame update
        private void Start()
        {
        }

        public async UniTask<string> GetDataAsync(string url, CancellationToken token)
        {
            var op = await UnityWebRequest.Get(url).SendWebRequest().WithCancellation(token);
            return op.downloadHandler.text;
        }

        public async UniTask<string> GetDataAsyncWithTimeout(string url)
        {
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfterSlim(TimeSpan.FromSeconds(20));

            var result = "";

            try
            {
                var op = await UnityWebRequest.Get(url).SendWebRequest().WithCancellation(tokenSource.Token);
                result = op.downloadHandler.text;
            }
            catch (OperationCanceledException ex)
            {
                if (ex.CancellationToken == tokenSource.Token) Debug.Log("Timeout Request");
            }

            return result;
        }

        public async UniTask<Sprite> GetTextureAsync(string url, CancellationToken token)
        {
            var requestTexture = await UnityWebRequestTexture.GetTexture(url).SendWebRequest().WithCancellation(token);

            var texture = ((DownloadHandlerTexture)requestTexture.downloadHandler).texture;
            var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2());

            return sprite;
        }
    }
}