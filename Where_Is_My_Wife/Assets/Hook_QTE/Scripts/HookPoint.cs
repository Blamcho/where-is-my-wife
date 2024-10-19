using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WhereIsMyWife.UI
{
    public class HookPoint : MonoBehaviour
    {
        [SerializeField] private GameObject _redArrowGameObject;

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {

            }
        }
    }
}