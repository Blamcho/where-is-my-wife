using System;
using TMPro;
using UnityEngine;
using WhereIsMyWife.Dialogue;

namespace WhereIsMyWife.Managers
{
   public class TextManager : Singleton<TextManager>
   {
      [SerializeField] private GameObject _textContainer;
      [SerializeField] private TextMeshProUGUI _text;

      private EventTalk _eventTalk;
      
      private int _currentTextIndex = -1;
   
      private void Start()
      {
         LanguageManager.Instance.OnLanguageChanged += RefreshText;
      }

      private void OnDestroy()
      {
         LanguageManager.Instance.OnLanguageChanged -= RefreshText;
      }
      
      private void Update()
      {
         // TODO: Change to InputSystem using InputEventManager

         if (Input.GetKeyDown(KeyCode.Space))
         {
            ChangeToNextText();
         }
      }

      private void ChangeToNextText()
      {
         if (_eventTalk.IsThereAnotherText(_currentTextIndex))
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
         _text.text = _eventTalk.GetText(_currentTextIndex);
      }

      public void ShowUIText(EventTalk nextEventTalk)
      {
         _eventTalk = nextEventTalk;
         _textContainer.SetActive(true);
         _currentTextIndex = -1;
         ChangeToNextText();
      }

      private void CloseTextUI()
      {
         Debug.Log($"There is no text in {_eventTalk} at index {_currentTextIndex}");
         _textContainer.SetActive(false);
      }
   }
}
