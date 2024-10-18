using WhereIsMyWife.Managers;

public static class LocalizationExtensions 
{
    public static string Localize(this string key)
    {
        return LocalizationManager.Instance.GetLocalizedValue(key);
    }
}
