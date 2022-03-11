using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager sharedInstance;
    public bool gameStarted = false;
    public bool gamePaused = false;
    public bool pocmanDead = false;
    public bool gameOver = false;
    public float powerUpTime = 0.0f;
    public float fruitTimer = 0.0f;

    public AudioClip statAudio;
    public AudioClip pauseAudio;
    public AudioClip diedAudio;

    public GameObject[] fruits ={ };
    public Transform fruitSpawn;
    public bool hasFruit = false;


    void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        StartCoroutine("StartGame");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !gameOver)
        {
            gamePaused = !gamePaused;
            if (gamePaused)
            {
                this.GetComponent<AudioSource>().clip = pauseAudio;
                this.GetComponent<AudioSource>().loop = true;
                this.GetComponent<AudioSource>().Play();
                GetComponent<AudioSource>().volume = 0.05f;
            }
            else
            {
                this.GetComponent<AudioSource>().Stop();
            }
        }
        if (pocmanDead && !gameOver)
        {
            this.GetComponent<AudioSource>().Stop();
            this.GetComponent<AudioSource>().clip = diedAudio;
            this.GetComponent<AudioSource>().loop = false;
            this.GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().volume = 0.05f;
            gameOver = true;
        }
        if (powerUpTime > 0)
        {
            powerUpTime -= Time.deltaTime;
        }
        if (!hasFruit)
        {
            hasFruit = true;
            StartCoroutine("SpawnFruit");
        }
        if (gameOver)
        {
            if (Input.GetKey(KeyCode.Return))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    public void givePowerUpFor(float numberOfSeconds)
    {
        this.powerUpTime += numberOfSeconds;
    }

    public void createFruit()
    {
        int x;
        x = Random.Range(0, fruits.Length - 1);
        Instantiate(fruits[x],fruitSpawn.position,fruitSpawn.rotation);
    }

    public void eatFruit()
    {
        hasFruit = false;
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSecondsRealtime(4.0f);
        gameStarted = true;
    }

    IEnumerator SpawnFruit()
    {
        yield return new WaitForSecondsRealtime(10f);
        createFruit();
    }
}
