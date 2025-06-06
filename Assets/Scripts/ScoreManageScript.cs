using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Linq;

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

    //Dictionary<int, string> rank = new Dictionary<int, string>()
    //  {
    //    {0, ""},
    //    {1, ""},
    //    {2, ""},
    //    {3, ""},
    //    {4, ""}
    //  };

    List<int> saveScore = new List<int>()
    {
        0, 0, 0, 0, 0
    };

    List<string> saveName = new List<string>()
    {
        "", "", "", "", ""
    };

    float timer = 0.0f;

    private void Awake()
    {
        //Dictionary<int, string> loadDict = PlayerPrefsUtility.LoadDict<int, string>("DictSaveKey");
        //rank = PlayerPrefsUtility.LoadDict<int, string>("DictSaveKey");
    }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }


    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 7.0f)
        {
            readyText.SetActive(false);
            PlayerController.canStart = true;
        }

        if (PlayerController.gameOver == false && PlayerController.canStart == true)
        {
            score += 10 * Time.deltaTime;
            scoreText.text = "Score : " + score.ToString("f0");
        }

        if (PlayerController.gameOver == true)
        {
            gameOverText.SetActive(true);
            retryText.SetActive(true);
            titleText.SetActive(true);

            if (Input.GetKey(KeyCode.R))
            {
                Reset();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            if (Input.GetKey(KeyCode.T))
            {
                Reset();
                StartScript.isStart = false;
                SceneManager.LoadScene("Load");
            }
        }
    }

    private void Reset()
    {
        var savescore = (int)score;

        for(int i = 0; i <= 4; i++)
        {
            if(saveScore[i] <= savescore)
            {
                saveScore[i] = savescore;
                saveName[i] = StartScript.playerName;
                break;
            }
        }

        for (int k = 0; k <= 4; k++)
        {
            Debug.Log(saveScore[k]);
            Debug.Log(saveName[k]);
        }

        //var keys = rank.Keys.ToList();

        ////foreach(var i in keys)
        ////{
        ////    Debug.Log(i);
        ////}
        
        //foreach (var kv in keys)
        //{
        //    if (kv <= savescore)
        //    { 
        //        rank[kv] = StartScript.playerName;
        //        rank[savescore] = rank[kv];
        //        rank.Remove(kv);
        //        break;
        //    }
        //}

        ////saveDict.OrderByDescending(x => x.Key);

        ////saveDict = rank.OrderByDescending(x => x.Key);
        ////saveDict.ToDictionary(pair => pair.Key, pair => pair.Value);

        //IOrderedEnumerable<KeyValuePair<int, string>> saveDict = rank.OrderByDescending(x => x.Key);
        //rank = saveDict.ToDictionary(pair => pair.Key, pair => pair.Value);
        ////PlayerPrefsUtility.SaveDict<int, string>("DictSaveKey", rank);

        //foreach (KeyValuePair<int, string> item in rank)
        //{
        //    Debug.Log("キーは" + item.Key + "です。  バリューは" + item.Value + "です。");
        //}

        score = 0.0f;
        timer = 0.0f;
        PlayerController.canStart = false;
        PlayerController.gameOver = false;
    }
}

public static class PlayerPrefsUtility
{

    //=================================================================================
    //保存
    //=================================================================================

    /// <summary>
    /// ディクショナリーを保存
    /// </summary>
    public static void SaveDict<Key, Value>(string key, Dictionary<Key, Value> value)
    {
        string serizlizedDict = Serialize<Dictionary<Key, Value>>(value);
        PlayerPrefs.SetString(key, serizlizedDict);
    }

    //=================================================================================
    //読み込み
    //=================================================================================

    /// <summary>
    /// ディクショナリーを読み込み
    /// </summary>
    public static Dictionary<Key, Value> LoadDict<Key, Value>(string key)
    {
        //keyがある時だけ読み込む
        if (PlayerPrefs.HasKey(key))
        {
            string serizlizedDict = PlayerPrefs.GetString(key);
            return Deserialize<Dictionary<Key, Value>>(serizlizedDict);
        }

        return new Dictionary<Key, Value>();
    }

    //=================================================================================
    //シリアライズ、デシリアライズ
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
