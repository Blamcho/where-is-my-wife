using UnityEngine;
using UnityEngine.Serialization;
using WhereIsMyWife.Dialogue;
using WhereIsMyWife.Managers;

public class DialogueStarter : MonoBehaviour
{
    [SerializeField] private DialogueSO _dialogueSo;
    
    private DialogueManager _dialogueManager;
    
    void Start()
    {
        _dialogueManager = DialogueManager.Instance;
    }

    public void StartDialogue()
    {
        _dialogueManager.StartDialogue(_dialogueSo);
    }
}
