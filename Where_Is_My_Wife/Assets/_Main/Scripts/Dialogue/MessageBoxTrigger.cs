using UnityEngine;
using WhereIsMyWife.Controllers;

public class MessageBoxTrigger : MonoBehaviour
{
    [SerializeField] private string _text;
    [SerializeField] private MessageBoxButtonType _buttonType;

    public string Text => _text;
    public MessageBoxButtonType ButtonType => _buttonType;
}
