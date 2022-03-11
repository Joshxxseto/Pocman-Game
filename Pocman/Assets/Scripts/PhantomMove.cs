using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomMove : MonoBehaviour
{
    //En este caso el fantasma contará con un movimiento basado en rutas
    //Así implementaré un sistema en donde el fantasma seguira una serie de puntos sobre el mapa
    public Transform[] waypoints;
    private Transform home;
    int currentWaypoint = 0;
    public float speed = 0.1f;
    public bool isAlive=true;

    private void Update()
    {
        if (GameManager.sharedInstance.powerUpTime > 0)
        {
            this.GetComponent<Animator>().SetBool("Run", true);
        }
        else
        {
            this.GetComponent<Animator>().SetBool("Run", false);
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.sharedInstance.gameOver&&GameManager.sharedInstance.gameStarted&&!GameManager.sharedInstance.gamePaused)
        {
            if (isAlive)
            {
                GetComponent<AudioSource>().volume = 0.03f;
                float distanceToWaypoint = Vector2.Distance((Vector2)this.transform.position, (Vector2)waypoints[currentWaypoint].position);
                //Distancia entre el fantasma y el punto de destino

                if (distanceToWaypoint < 0.1f)
                {
                    currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
                    Vector2 newDir = waypoints[currentWaypoint].position - this.transform.position;
                    this.GetComponent<Animator>().SetFloat("DirX", newDir.x);
                    this.GetComponent<Animator>().SetFloat("DirY", newDir.y);
                }
                else
                {
                    Vector2 newPos = Vector2.MoveTowards(this.transform.position, waypoints[currentWaypoint].position, speed);
                    GetComponent<Rigidbody2D>().MovePosition(newPos);
                }
            }
            else
            {
                float distanceToHome = Vector2.Distance((Vector2)this.transform.position, (Vector2)home.position);
                //Distancia entre el fantasma y el punto de destino (Casa)

                if (distanceToHome < 0.1f)//LLegue a casa
                {
                    StartCoroutine("AwakeFromHome");
                    
                }
                else //Seguir a Casa
                {
                    Vector2 newPos = Vector2.MoveTowards(this.transform.position, home.position, speed);
                    GetComponent<Rigidbody2D>().MovePosition(newPos);
                }
            }
        }
        else
        {
            GetComponent<AudioSource>().volume = 0;
        }
    }

    private void goHome()
    {
        GameObject findHome = GameObject.Find("PhantomHome");
        home = findHome.gameObject.transform;
        isAlive = false;
        this.speed *= 1.2f;
        this.currentWaypoint = 0;
        this.GetComponent<Animator>().SetBool("GoingHome",true);
    }

    IEnumerator AwakeFromHome()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        this.GetComponent<Animator>().SetBool("GoingHome", false);
        //Ahora volvereos al fantasma un 20% mas veloz cada vez que muera
        isAlive = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag=="Player"&&isAlive)
        {
            if (!(GameManager.sharedInstance.powerUpTime > 0))
            {
                other.GetComponent<PocmanMovement>().deadAnimation();
                Destroy(other.gameObject.GetComponent<Collider2D>());
                GameManager.sharedInstance.pocmanDead = true;
            }
            else
            {
                UIManager.sharedInstance.addScore(10);
                goHome();
            }
        }
    }
}
