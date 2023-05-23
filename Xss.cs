public static class Xss
{
    public static bool Check_Mobile_Number(string value)
    {
        const string pattern = "09\\d{9}$";
        return !string.IsNullOrEmpty(value) && Regex.IsMatch(value.Trim(), pattern);
    }
}
