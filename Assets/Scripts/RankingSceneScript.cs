using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RankingSceneScript : MonoBehaviour
{
    [SerializeField]
    private Text[] rankingTexts;
    
    private string saveKey = "RankingDataKey"; 

    void Start()
    {
        ShowRanking();
    }

    void ShowRanking()
    {
        List<RankingEntry> rankingList = PlayerPrefsUtility.LoadList<List<RankingEntry>>(saveKey);

        if (rankingList == null)
        {
            rankingList = new List<RankingEntry>();
        }

        for (int i = 0; i < rankingTexts.Length; i++)
        {
            if (i < rankingList.Count)
            {
                rankingTexts[i].text = (i + 1) + "位: " + rankingList[i].score + " - " + rankingList[i].userName;
            }
            else
            {
                rankingTexts[i].text = (i + 1) + "位: ---";
            }
        }
    }

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("Start");
    }
}