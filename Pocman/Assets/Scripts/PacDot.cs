using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacDot : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.tag == "Player" && this.gameObject.tag == "PacDot")
        {
            ///Me comio Pocman -> Asignar un punto
            Destroy(this.gameObject);
            sumarPuntos(1);
        }
        if (otherCollider.tag == "Player" && this.gameObject.tag == "Fruit")
        {
            ///Me comio Pocman -> Asignar 20 puntos
            Destroy(this.gameObject);
            sumarPuntos(20);
            eatFruit();
        }
        if (otherCollider.tag == "Player" && this.gameObject.tag == "PowerUp")
        {
            ///Me comio Pocman -> Darle PowerUp
            Destroy(this.gameObject);
            givePowerUp(7);
        }
    }

    public void sumarPuntos(int x)
    {
        UIManager.sharedInstance.addScore(x);
    }

    public void eatFruit()
    {
        GameManager.sharedInstance.eatFruit();
    }

    public void givePowerUp(float x)
    {
        GameManager.sharedInstance.givePowerUpFor(x);
    }
   
}
