using System.Collections;
using TMPro;
using UnityEngine;

public class GameStartCountdown : MonoBehaviour
{
    public TMP_Text countdownText;
    public BirdScript bird;

    void Start()
    {
        bird.canFlap = false;
        StartCoroutine(StartCountdown());

    }

    IEnumerator StartCountdown()
    {
        countdownText.gameObject.SetActive(true);

        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        countdownText.text = "GO!";

        // Fly beloved bird
        bird.myRigidBody.bodyType = RigidbodyType2D.Dynamic;
        bird.canFlap = true;
        bird.birdsIsAlive = true;

        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);
    }
}
