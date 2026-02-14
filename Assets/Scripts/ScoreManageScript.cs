using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;


[System.Serializable]
public class RankingEntry
{
    public int score;
    public string userName;

    public RankingEntry(int s, string n)
    {
        score = s;
        userName = n;
    }
}

public class ScoreManageScript : MonoBehaviour
{
    float score = 0.0f;

    [SerializeField]
    private GameObject gameOverText;
    [SerializeField]
    private GameObject retryText;
    [SerializeField]
    private GameObject titleText;
    [SerializeField]
    private GameObject readyText;

    public Text scoreText;

    // ランキングのリスト
    private List<RankingEntry> rankingList = new List<RankingEntry>();
    private string saveKey = "RankingDataKey"; // 保存用のキー

    float timer = 0.0f;

    private void Awake()
    {
        rankingList = PlayerPrefsUtility.LoadList<List<RankingEntry>>(saveKey);
        
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 7.0f)
        {
            if(readyText != null) readyText.SetActive(false);
            PlayerController.canStart = true;
        }

        if (PlayerController.gameOver == false && PlayerController.canStart)
        {
            score += 10 * Time.deltaTime;
            if(scoreText != null) scoreText.text = "Score : " + score.ToString("f0");
        }

        if (PlayerController.gameOver)
        {
            if(gameOverText != null) gameOverText.SetActive(true);
            if(retryText != null) retryText.SetActive(true);
            if(titleText != null) titleText.SetActive(true);

            // Rキーでリトライ
            if (Input.GetKeyDown(KeyCode.R))
            {
                SaveCurrentScore(); // 保存処理
                ResetGame();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            // Tキーでタイトルへ
            if (Input.GetKeyDown(KeyCode.T))
            {
                SaveCurrentScore(); // 保存処理
                ResetGame();
                StartScript.isStart = false;
                SceneManager.LoadScene("Load");
            }
        }
    }

    private void SaveCurrentScore()
    {
        int currentScore = (int)score;
        string currentName = StartScript.playerName;

        rankingList.Add(new RankingEntry(currentScore, currentName));

        rankingList.Sort((x, y) => y.score.CompareTo(x.score));

        if (rankingList.Count > 5)
        {
            rankingList.RemoveRange(5, rankingList.Count - 5);
        }

        PlayerPrefsUtility.SaveList(saveKey, rankingList);
    }

    private void ResetGame()
    {
        score = 0.0f;
        timer = 0.0f;
        PlayerController.canStart = false;
        PlayerController.gameOver = false;
    }
}


public static class PlayerPrefsUtility
{
    //=================================================================================
    // 汎用的な保存・読み込み (Listや独自のクラス用)
    //=================================================================================

    /// <summary>
    /// オブジェクト（List<T>など）を保存
    /// </summary>
    public static void SaveList<T>(string key, T obj)
    {
        string serializedObj = Serialize<T>(obj);
        PlayerPrefs.SetString(key, serializedObj);
        PlayerPrefs.Save(); 
    }

    /// <summary>
    /// オブジェクト（List<T>など）を読み込み
    /// </summary>
    public static T LoadList<T>(string key) where T : new()
    {
        if (PlayerPrefs.HasKey(key))
        {
            string serializedObj = PlayerPrefs.GetString(key);
            return Deserialize<T>(serializedObj);
        }

        return new T(); // キーがない場合は新しいインスタンスを返す
    }

    //=================================================================================
    // 既存のDictionary用
    //=================================================================================
    
    public static void SaveDict<Key, Value>(string key, Dictionary<Key, Value> value)
    {
        SaveList(key, value); // 実質同じ処理なのでSaveListに流してもOK
    }

    public static Dictionary<Key, Value> LoadDict<Key, Value>(string key)
    {
        return LoadList<Dictionary<Key, Value>>(key);
    }

    //=================================================================================
    // シリアライズ、デシリアライズ
    //=================================================================================

    private static string Serialize<T>(T obj)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream();
        binaryFormatter.Serialize(memoryStream, obj);
        return Convert.ToBase64String(memoryStream.GetBuffer());
    }

    private static T Deserialize<T>(string str)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(str));
        return (T)binaryFormatter.Deserialize(memoryStream);
    }
}