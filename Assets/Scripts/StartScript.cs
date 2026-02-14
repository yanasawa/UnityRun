using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class StartScript : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject stage;

    [SerializeField]
    private RectTransform text;

    [SerializeField]
    private GameObject startText;

    [SerializeField] 
    private GameObject rankingText;

    [SerializeField]
    private GameObject TutorialText;

    [SerializeField]
    private GameObject goText;

    [SerializeField]
    private GameObject titleText;

    [SerializeField]
    private GameObject input;

    [SerializeField]
    private InputField inputField;

    float alfa = 1.0f;    //A値を操作するための変数
    float red, green, blue;    //RGBを操作するための変数

    public float alfaSpeed;  //透明化の速さ
    public float playerSpeed;
    public float textSpeed;

    public static bool isStart = true;
    public static string playerName;

    private GameObject pos;
    private GameObject temp;

    int counter = 0;
    bool canText = false;
    bool canStart = true;
    bool textCounter = false;
    bool canStageMove = true;
    bool isRanking = false;
    int textCount = 0;

    void Start()
    {
        red = panel.GetComponent<Image>().color.r;
        green = panel.GetComponent<Image>().color.g;
        blue = panel.GetComponent<Image>().color.b;

        inputField = inputField.GetComponent<InputField>();

        pos = GameObject.Find("GeneratePos");
        isRanking = false;
    }


    void FixedUpdate()
    {
        panel.GetComponent<Image>().color = new Color(red, green, blue, alfa);
        if (text.anchoredPosition.y <= 89)
        {
            if (alfa >= 0.0f)
            {
                alfa -= alfaSpeed;
            }
        }

        Invoke("Text", 1);

        if (player.transform.position.x <= 0.0f)
        {
            player.transform.position += new Vector3(playerSpeed * Time.deltaTime, 0, 0);
        }
        else
        {
            if (canStageMove)
            {
                stage.transform.position += new Vector3(-playerSpeed * Time.deltaTime, 0, 0);

                if (stage.transform.position.x <= 0)
                {
                    if (counter <= 0)
                    {
                        temp = Instantiate(stage, pos.transform.position, transform.rotation);
                        counter++;
                    }
                    temp.transform.position += new Vector3(-playerSpeed * Time.deltaTime, 0, 0);
                }
                if (stage.transform.position.x <= -20)
                {
                    Destroy(stage.gameObject);
                    stage = temp;
                    counter = 0;
                }
            }
        }
        
        if (canText)
        {
            text.DOAnchorPosY(90f, textSpeed);
        }

        if (text.anchoredPosition.y >= 89)
        {
            if (canStart)
            {
                startText.SetActive(true);
                rankingText.SetActive(true);
            }

            if (Input.GetKey(KeyCode.S))
            {
                panel.SetActive(false);
                titleText.SetActive(false);
                startText.SetActive(false);
                rankingText.SetActive(false);
                canStart = false;
                input.SetActive(true);
            }

            if (Input.GetKey(KeyCode.R) && !isRanking)
            {
                panel.SetActive(true);
                StartCoroutine(RankCoroutine());
            }

            if (textCounter && textCount <= 7)
            {
                textCounter = false;
                goText.SetActive(true);
                Invoke("GoText", 0.5f);
            }

            if (textCount >= 3)
            {
                canStageMove = false;
                player.transform.position += new Vector3(playerSpeed * Time.deltaTime, 0, 0);
            }

            if (textCount >= 7)
            {
                panel.SetActive(true);
                alfa += alfaSpeed;
                if (alfa >= 1.1f)
                {
                    isStart = true;
                    SceneManager.LoadScene("Load");
                }
            }
        }
    }

    void Text()
    {
        canText = true;
    }

    void GoText()
    {
        goText.SetActive(false);
        Invoke("EndText", 0.5f);
    }

    void EndText()
    {
        textCount++;
        textCounter = true;
    }

    public void InputName()
    {
        input.SetActive(false);
        textCounter = true;
        Debug.Log(playerName);
    }

    public void InputText()
    {
        playerName = inputField.text;
    }

    IEnumerator RankCoroutine()
    {
        isRanking = true; 

        float duration = 1.0f; 
        float startAlfa = alfa;
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;

            alfa = Mathf.Lerp(startAlfa, 1.1f, time / duration);

            yield return null; 
        }

        alfa = 1.1f;
        
        yield return new WaitForSeconds(0.5f);
        
        SceneManager.LoadScene("Ranking"); 
    }
}
