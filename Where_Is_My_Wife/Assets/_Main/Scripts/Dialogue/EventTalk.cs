using UnityEngine;

namespace WhereIsMyWife.Dialogue
{
   [CreateAssetMenu (fileName = "EventTalk", menuName = "ScriptableObjects/Dialogue/EventTalk")]
   public class EventTalk : ScriptableObject
   {
      [SerializeField] private StringArray[] _text;

      public string GetText(int index)
      {
         return _text[index].Text[0];
      }

      public bool IsThereAnotherText(int index)
      {
         return index < _text.Length;
      }
   }
}
