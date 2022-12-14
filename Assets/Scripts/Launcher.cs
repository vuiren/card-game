using Domain.DTO;
using Firebase;
using Firebase.Database;
using Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviour
{
    [SerializeField] private Configuration _configuration;
    [SerializeField] private TextMeshProUGUI statusText;
    private DatabaseReference _databaseReference;

    private void Start()
    {
        statusText.text = "Connecting...";
        _databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        var app = FirebaseApp.DefaultInstance;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                app = FirebaseApp.DefaultInstance;
                statusText.text = "Connected";
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
                statusText.text = "Fail to connect";
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void SetName(string name)
    {
        _configuration.playerName = name;
    }

    public void SetGameId(string gameId)
    {
        _configuration.gameId = gameId;
    }

    public void ConnectButton()
    {
        Connect();
    }

    private async void Connect()
    {
        var value = await _databaseReference.Child(_configuration.gameId).Child("game").GetValueAsync();
        var gameData = JsonUtility.FromJson<GameData>(value.GetRawJsonValue());


        var playerData = new PlayerData();

        if (gameData == null)
        {
            playerData.id = 0;
            _configuration.isHost = true;
            gameData = new GameData();
        }
        else
        {
            _configuration.isHost = false;
            playerData.id = gameData.lastId + 1;
            gameData.lastId++;
        }

        _configuration.playerId = playerData.id;
        playerData.name = _configuration.playerName;

        var playerDataJson = JsonUtility.ToJson(playerData);

        await _databaseReference.Child(_configuration.gameId).Child("players").Child(_configuration.playerName)
            .SetRawJsonValueAsync(playerDataJson);

        var gameDataJson = JsonUtility.ToJson(gameData);

        await _databaseReference.Child(_configuration.gameId).Child("game")
            .SetRawJsonValueAsync(gameDataJson);

        SceneManager.LoadScene(1);
    }
}