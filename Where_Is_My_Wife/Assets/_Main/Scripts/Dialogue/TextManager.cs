using TMPro;
using UnityEngine;
using WhereIsMyWife.Dialogue;

namespace WhereIsMyWife.Managers
{
   public class TextManager : Singleton<TextManager>
   {
      [SerializeField] private EventTalk _eventTalk;
      [SerializeField] private TextMeshProUGUI _text;

      private int _currentTextIndex = 0;
   
      private void Start()
      {
         ChangeToNextText();
      }

      private void ChangeToNextText()
      {
         if (_eventTalk.IsThereAnotherText(_currentTextIndex))
         {
            _text.text = _eventTalk.GetText(_currentTextIndex);
            _currentTextIndex++;
         }
         else
         {
            Debug.Log($"There is no text in {_eventTalk} at index {_currentTextIndex}");
         }
      }

      private void Update()
      {
         // TODO: Change to InputSystem using InputEventManager

         if (Input.GetKeyDown(KeyCode.Space))
         {
            ChangeToNextText();
         }
      }
   }
}
