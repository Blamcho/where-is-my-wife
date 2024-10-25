using UnityEngine;
using UnityEngine.UI;

namespace WhereIsMyWife.Managers
{
    public class DeleteSaveButton : MonoBehaviour
    {
        [SerializeField] private Button deleteButton;

        private void Start()
        {
            deleteButton?.onClick.AddListener(DeleteSave);
        }

        private void DeleteSave()
        {
            
        }
    }
}