using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.Managers;
using WhereIsMyWife.UI;

public class InputGoBackMenu : MonoBehaviour
{
    [SerializeField] private RectTransform _nextMenu;
    [SerializeField] private Button _nextButton;
    
    private void OnEnable()
    {
        InputEventManager.Instance.CancelStartAction += ExitMenu;
    }

    private void OnDisable()
    {
        InputEventManager.Instance.CancelStartAction -= ExitMenu;
    }

    private void ExitMenu()
    {
        MenuManager.Instance.ChangeMenu(_nextMenu, MenuManager.MenuChangeAnimation.MoveOut, _nextButton).Forget();
    }
}
