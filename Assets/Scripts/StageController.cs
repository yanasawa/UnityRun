using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    GameObject stageManager; 

    void Start()
    {
        stageManager = GameObject.Find("StageManager");
    }

    void FixedUpdate()
    {
        if (PlayerController.gameOver == false && PlayerController.canStart == true)
        {
            transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
            if (transform.position.x <= -20)
            {
                stageManager.SendMessage("GenerateStage");
                Destroy(this.gameObject);
            }
        }
    }
}
