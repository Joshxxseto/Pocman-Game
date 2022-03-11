using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocmanMovement : MonoBehaviour
{
    public float speed = 0.2f;
    public bool isDead = false;
    Vector2 destination = Vector2.zero;

    void Start()
    {
        destination = this.transform.position;
    }

    private void Update()
    {
        if (GameManager.sharedInstance.powerUpTime > 0)
        {
            this.speed = 0.25f;
        }
        else
        {
            this.speed = 0.2f;
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.sharedInstance.gameOver && GameManager.sharedInstance.gameStarted && !GameManager.sharedInstance.gamePaused)
        {
            GetComponent<AudioSource>().volume = 0.05f;
            //Calculamos el nuevo punto donde hay que ir en base a la variable de destino
            Vector2 newPos = Vector2.MoveTowards(this.transform.position, destination, speed);
            //Usamos el rigidbody2d para transportar a pacman hasta discha posicion
            GetComponent<Rigidbody2D>().MovePosition(newPos);
            float distanceToDestination = Vector2.Distance((Vector2)this.transform.position, destination);
            if (distanceToDestination <= 2)
            {
                if (Input.GetKey(KeyCode.UpArrow) && CanMoveTo(Vector2.up))
                {
                    destination = (Vector2)this.transform.position + Vector2.up;
                }
                if (Input.GetKey(KeyCode.LeftArrow) && CanMoveTo(Vector2.left))
                {
                    destination = (Vector2)this.transform.position + Vector2.left;
                }
                if (Input.GetKey(KeyCode.RightArrow) && CanMoveTo(Vector2.right))
                {
                    destination = (Vector2)this.transform.position + Vector2.right;
                }
                if (Input.GetKey(KeyCode.DownArrow) && CanMoveTo(Vector2.down))
                {
                    destination = (Vector2)this.transform.position + Vector2.down;
                }
            }
            Vector2 dir = destination - (Vector2)this.transform.position;
            GetComponent<Animator>().SetFloat("DirX", dir.x);
            GetComponent<Animator>().SetFloat("DirY", dir.y);
            //Debug.Log(distanceToDestination);
        }
        else
        {
            GetComponent<AudioSource>().volume = 0;
        }
    }

    //Validar el proximo moviiento es decir si hay una pared
    //Y asi evita que nos movamos en esa direccion
    bool CanMoveTo(Vector2 dir)
    {
        Vector2 pocmanPos = this.transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pocmanPos+dir, pocmanPos);
        //Aqui se dibuja un raycast desde donde queremos ir hacia pocman
        //Entonces si el collider2D que golpea el raycast es el de pocman regresaremos un valor verdadero
        Collider2D pocmanCollider = this.GetComponent<Collider2D>();
        Collider2D hitCollider = hit.collider;
        return hitCollider == pocmanCollider;
    }

    public void deadAnimation()
    {
        this.transform.GetComponent<Animator>().SetBool("IsDead",true);
    }
}
