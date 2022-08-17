using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerDot : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PAC-MAN"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.DotRemain -= 1;
                GameManager.Instance.Score += 150;
                AIPathGuider.Instance.OnNerf();
            }
            Destroy(gameObject);
        }
    }
}
