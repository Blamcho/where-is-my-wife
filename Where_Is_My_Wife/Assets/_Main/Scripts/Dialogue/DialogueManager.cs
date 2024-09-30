using TMPro;
using UnityEngine;
using WhereIsMyWife.Dialogue;

namespace WhereIsMyWife.Managers
{
   public class DialogueManager : Singleton<DialogueManager>
   {
      [SerializeField] private GameObject _textContainer;
      [SerializeField] private TextMeshProUGUI _text;

      private DialogueSO _dialogueSo;
         
      private int _currentTextIndex = -1;
      private LanguageManager _languageManager;
      private IUIInputEvent _uiInputEvent;
   
      private void Start()
      {
         _languageManager = LanguageManager.Instance;
         _uiInputEvent = InputEventManager.Instance.UIInputEvent;
         
         _languageManager.OnLanguageChanged += RefreshText;
         _uiInputEvent.SubmitStartAction += ChangeToNextText;
      }

      private void OnDestroy()
      {
         _languageManager.OnLanguageChanged -= RefreshText;
         _uiInputEvent.SubmitStartAction -= ChangeToNextText;
      }

      private void ChangeToNextText()
      {
         if (!_dialogueSo) return;
         
         if (_dialogueSo.IsThereAnotherText(_currentTextIndex))
         {
            _currentTextIndex++;
            RefreshText();
         }
         else
         {
            CloseTextUI();
         }
      }

      private void RefreshText()
      {
         if (!_dialogueSo) return;
         
         _text.text = _dialogueSo.GetText(_currentTextIndex);
      }

      public void StartDialogue(DialogueSO nextDialogueSo)
      {
         _dialogueSo = nextDialogueSo;
         _textContainer.SetActive(true);
         _currentTextIndex = -1;
         ChangeToNextText();
      }

      private void CloseTextUI()
      {
         Debug.Log($"There is no text in {_dialogueSo.name} at index {_currentTextIndex}. Dialogue is closed.");
         _dialogueSo = null;
         _textContainer.SetActive(false);
      }
   }
}
