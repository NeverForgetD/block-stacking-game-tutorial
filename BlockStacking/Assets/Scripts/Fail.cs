using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fail : MonoBehaviour
{
    [SerializeField] private GameManager gm;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gm.playing = false;
        //Debug.Log("Fail");
    }
}
