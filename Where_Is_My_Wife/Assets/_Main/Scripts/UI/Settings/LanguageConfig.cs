using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Config
{
    public class LanguageConfig : ConfigSelection
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
