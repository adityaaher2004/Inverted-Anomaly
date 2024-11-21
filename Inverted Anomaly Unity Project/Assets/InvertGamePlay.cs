using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InvertGamePlay : MonoBehaviour
{
    [SerializeField] GameObject ZoneA;
    [SerializeField] GameObject ZoneB;
    GlobalIsRewindingScript globalRewinder;
    GameObject currentPlayer;

    bool instantiated = false;

    [SerializeField] GameObject PlayerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        globalRewinder = GameObject.FindFirstObjectByType<GlobalIsRewindingScript>();
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (globalRewinder.fireStartRewind && !instantiated)
        {
            GameObject.FindGameObjectWithTag("CamFollower").SetActive(false);
            GameObject.FindGameObjectWithTag("PlayerUIEventSystem").SetActive(false);
            GameObject.FindGameObjectWithTag("CinemachineTarget").SetActive(false);

            GameObject newPlayer = Instantiate(PlayerPrefab, ZoneB.transform.position, ZoneB.transform.rotation);
            newPlayer.GetComponent<PlayerInput>().camera = currentPlayer.GetComponent<PlayerInput>().camera;
            newPlayer.GetComponent<PlayerInput>().ActivateInput();
            newPlayer.GetComponent<PlayerAction>().Crosshair = currentPlayer.GetComponent<PlayerAction>().Crosshair;
            newPlayer.GetComponent<ThirdPersonShooterController>().aimColliderMask = currentPlayer.GetComponent<ThirdPersonShooterController>().aimColliderMask;
            newPlayer.GetComponent<ThirdPersonShooterController>().aimTransform = currentPlayer.GetComponent<ThirdPersonShooterController>().aimTransform;

            currentPlayer.GetComponent<StarterAssetsInputs>().enabled = false;
            currentPlayer.GetComponent<PlayerInput>().enabled =  false;
            currentPlayer.GetComponent<CharacterController>().enabled = false;
            currentPlayer.GetComponent<ThirdPersonController>().enabled = false;
            currentPlayer.GetComponent<PlayerAction>().enabled = false;

            newPlayer.GetComponent<PlayerInput>().enabled = false;
            newPlayer.GetComponent<PlayerInput>().enabled = true;
            newPlayer.GetComponent<PlayerRewinder>().enabled = false;

            instantiated = true;

        }
    }
}
