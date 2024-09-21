using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Dialogue
{
   [CreateAssetMenu (fileName = "EventTalk", menuName = "ScriptableObjects/Dialogue/EventTalk")]
   public class EventTalk : ScriptableObject
   {
      [SerializeField] private StringArray[] _text;

      public string GetText(int index)
      {
         return _text[index].Text[(int)LanguageManager.Instance.Language];
      }

      public bool IsThereAnotherText(int currentIndex)
      {
         return currentIndex + 1 < _text.Length;
      }
   }
}
