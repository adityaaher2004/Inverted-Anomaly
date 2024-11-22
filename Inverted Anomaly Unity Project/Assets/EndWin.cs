using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class EndWin : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject player;

    [SerializeField] GameObject gameOver;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            gameOver.SetActive(true);
            player.GetComponent<PlayerInput>().DeactivateInput();
        }
    }
}
