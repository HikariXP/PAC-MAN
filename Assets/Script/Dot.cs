using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PAC-MAN"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.DotRemain -= 1;
                GameManager.Instance.Score += 100f;
            }
            Destroy(gameObject);
        }
    }
}
