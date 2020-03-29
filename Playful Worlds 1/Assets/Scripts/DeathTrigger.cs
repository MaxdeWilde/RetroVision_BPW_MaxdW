using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathTrigger : MonoBehaviour
{
    public bool playerDead;
    public Image blackScreen;

    // Use this for initialization
    void Awake()
    {
        Color blackColor = blackScreen.GetComponent<Image>().color;
        blackColor.a = 1f;
        blackScreen.GetComponent<Image>().color = blackColor;
    }


    void Start()
    {
        playerDead = false;
        
    }

    void Update()
    {
        if (playerDead == false)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!playerDead && col.CompareTag("Player"))
            PlayerDeath();
    }

    void PlayerDeath()
    {
        playerDead = true;

        Debug.Log("ded lol");

        Invoke("RestartScene", 1f);
    }

    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void FadeIn()
    {
        Color blackColor = blackScreen.GetComponent<Image>().color;
        blackColor.a += 1f * -1 * Time.deltaTime;
        blackScreen.GetComponent<Image>().color = blackColor;
    }

    void FadeOut()
    {
        Color blackColor = blackScreen.GetComponent<Image>().color;
        blackColor.a += 10f * 1 * Time.deltaTime;
        blackScreen.GetComponent<Image>().color = blackColor;

        if (blackColor.a == 1f)
        {
            Invoke("RestartScene", 0.1f);
        }
    }

}