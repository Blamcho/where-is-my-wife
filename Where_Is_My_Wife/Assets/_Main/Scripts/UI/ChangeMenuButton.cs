using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.UI;

public class ChangeMenuButton : MenuButton
{
    [SerializeField] private RectTransform _nextMenu;
    [SerializeField] private MenuManager.MenuChangeAnimation _menuChangeAnimation;
    [SerializeField] private Button _nextButton;
    
    protected override void Awake()
    {
        base.Awake();
        _button.onClick.AddListener(ChangeMenu);
    }

    private void ChangeMenu()
    {
        MenuManager.Instance.ChangeMenu(_nextMenu, _menuChangeAnimation, _nextButton);
    }
}
