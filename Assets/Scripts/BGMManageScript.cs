using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManageScript : MonoBehaviour
{
    [SerializeField]
    private AudioSource stage;

    [SerializeField]
    private AudioSource gameOver;


    bool isPlaying = false;


    void Start()
    {
        stage.Play();
    }

    void Update()
    {
        if (PlayerController.gameOver == true && isPlaying == false)
        {
            stage.Stop();
            gameOver.Play();
            isPlaying = true;
        }

        if (PlayerController.gameOver == false)
        {
            isPlaying = false;
        }
    }
}
