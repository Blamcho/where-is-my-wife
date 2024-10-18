using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RespawnTransition : MonoBehaviour
{
    public Animator transitionAnimator;  
    public Transform respawnPoint;       
    public GameObject player;           
    public float transitionDuration = 1f;  

    public void TriggerRespawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {

        transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(transitionDuration);
        player.transform.position = respawnPoint.position;
        yield return new WaitForSeconds(0.5f);
        transitionAnimator.SetTrigger("StartTransition");
        yield return new WaitForSeconds(transitionDuration);
    }
}