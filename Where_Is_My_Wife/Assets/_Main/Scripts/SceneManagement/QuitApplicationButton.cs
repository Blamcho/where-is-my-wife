using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.UI;

namespace WhereIsMyWife.SceneManagement
{
    public class QuitApplicationButton : MenuButton
    {
        protected override void Awake()
        {
            base.Awake();
            _button.onClick.AddListener(QuitApplication);
        }

        private void QuitApplication()
        {
            Application.Quit();
        }
    }
}
