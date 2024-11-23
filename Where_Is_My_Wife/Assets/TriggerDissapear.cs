using UnityEngine;

public class TriggerDissapear : MonoBehaviour
{ 
    [SerializeField] private GameObject _gameObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        _gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _gameObject.SetActive(false);
    }
}
