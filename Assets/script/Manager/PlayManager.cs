using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    public Transform player;
    public Transform EndPoint;
    public GameObject timeline;

    public float MinRange = 1;
    public bool DebugMode;

    public string NextSceneName;

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(EndPoint.position, player.position);

        if (DebugMode)
            print(distance);

        if (distance < MinRange)
        {
            timeline.SetActive(true);

            //Ä³¸¯ÅÍ¸¦ ¸ØÃä´Ï´Ù.
            var playerController = player.GetComponent<Player>();
            playerController.OnTriggerStop();           
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene(NextSceneName);
    }
}
