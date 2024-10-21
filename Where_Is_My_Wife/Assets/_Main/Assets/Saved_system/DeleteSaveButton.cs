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
            if (SavedManager.Instance != null)
            {
                PlayerPrefs.DeleteKey("saved-data"); 
                SavedManager.Instance.Load();
                Debug.Log("Saved data deleted.");
            }
            else
            {
                Debug.LogError("SavedManager instance not found.");
            }
        }
    }
}