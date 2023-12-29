using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dre0Dru.LocalStorage;
using Dre0Dru.LocalStorage.Serialization;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using Debug = UnityEngine.Debug;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Image avatar;

    private CancellationTokenSource _cancellationTokenSource;
    private IFileStorage _storage;

    private void Start()
    {
        ISerializationProvider serializationProvider = new UnityJsonSerializationProvider();

        IFileProvider fileProvider = new FileProvider();
        IFileStorage storage = new FileStorage(serializationProvider, fileProvider);
        _storage = storage;
    }

    [Button]
    public async void GetData()
    {
        var result =
            await UniTaskWebRequest.Instance.GetDataAsync("https://reqres.in/api/users/2",
                this.GetCancellationTokenOnDestroy());

        Debug.Log(result);
    }

    [Button]
    public async void GetDataTimeout()
    {
        var result =
            await UniTaskWebRequest.Instance.GetDataAsyncWithTimeout("https://reqres.in/api/users/2");

        Debug.Log(result);
    }


    [Button]
    public async void GetDataMultiple()
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        var tasks = new List<UniTask<string>>
        {
            UniTaskWebRequest.Instance.GetDataAsync("https://reqres.in/api/users/2",
                this.GetCancellationTokenOnDestroy()),
            UniTaskWebRequest.Instance.GetDataAsync("https://reqres.in/api/users/3",
                this.GetCancellationTokenOnDestroy())
        };

        await UniTask.WhenAll(tasks);

        stopwatch.Stop();
        Debug.Log($"{stopwatch.ElapsedMilliseconds / 1000.0f} seconds");
    }

    [Button]
    public async void GetTexture()
    {
        var sprite =
            await UniTaskWebRequest.Instance.GetTextureAsync(
                "https://www.freecodecamp.org/news/content/images/size/w2000/2021/04/unitybeg.png",
                this.GetCancellationTokenOnDestroy());

        avatar.sprite = sprite;
    }

    [Button]
    public async void SaveAsync()
    {
        const string fileName = "b.json";

        var data = new
        {
            name = "Ryder"
        };

        await _storage.SaveAsync(data, fileName);
        var exists = _storage.FileExists(fileName);
    }

    [Button]
    public async void TestUniDelay()
    {
        _cancellationTokenSource = new CancellationTokenSource();

        await UniTask.Delay(
            TimeSpan.FromSeconds(3f),
            DelayType.DeltaTime,
            PlayerLoopTiming.Update,
            this.GetCancellationTokenOnDestroy()
        );

        Debug.Log("Hello World");
    }

    [Button]
    public async void MovePlayer()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        await player.DOMove(new Vector3(2, 0, 0), 2f).WithCancellation(_cancellationTokenSource.Token);
        await player.DORotate(new Vector3(100, 100, 0), 2f);
    }

    [Button]
    public void StopMovePlayer()
    {
        _cancellationTokenSource?.Cancel();
    }
}