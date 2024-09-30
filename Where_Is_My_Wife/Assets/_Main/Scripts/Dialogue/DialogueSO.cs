using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Dialogue
{
   [CreateAssetMenu (fileName = "DialogueSO", menuName = "ScriptableObjects/Dialogue/DialogueSO")]
   public class DialogueSO : ScriptableObject
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
