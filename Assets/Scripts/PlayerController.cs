using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerController : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField]
    private float jumpforce;

    Rigidbody2D rb;
    Animator animator;
    int counter = 0;
    string path;

    public static bool gameOver = false;
    public static bool canStart = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //path = "/Users/hirokiyanagisawa/Desktop/test.txt";
    }

    void Update()
    {
        if (canStart == true)
        {
            animator.SetBool("isStart", true);

            if (Input.GetKeyDown(KeyCode.UpArrow) && gameOver == false)
            {
                counter++;
                if (counter <= 2)
                {
                    Jump();
                    if (counter == 1)
                    {
                        Run();
                        Invoke("Jump", 0.1f);
                    }
                }
            }
        }

        //string data = File.ReadAllText(path);
        //Debug.Log("Data is " + data);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Jump") && canStart == true)
        {
            Run();
            counter = 0;
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            gameOver = true;
            canStart = false;
            animator.SetBool("isGameOver", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block"))
        {
            Run();
            counter = 0;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(0, jumpforce);
        animator.SetBool("isJump", true);
        audioSource.PlayOneShot(audioSource.clip);
    }

    void Run()
    {
        animator.SetBool("isJump", false);
    }
}
