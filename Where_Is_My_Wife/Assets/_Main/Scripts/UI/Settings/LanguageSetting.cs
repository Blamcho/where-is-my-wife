using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Setting
{
    public class LanguageSetting : SettingSelection
    {
        protected override void SubscribeToActions()
        {
            _uiInputEvent.HorizontalStartedAction += CycleThroughLanguages;
        }

        protected override void SelectedUnsubscribeFromActions()
        {
            _uiInputEvent.HorizontalStartedAction -= CycleThroughLanguages;
        }

        private void CycleThroughLanguages(int direction)
        {
            LanguageManager.Instance.CycleThroughLanguages(direction);
        }
    }
}
