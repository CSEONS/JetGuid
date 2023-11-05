namespace GGuid.Views.Services.Extensions
{
    public static class Extenions
    {
        public static string CutController(this string text)
        {
            return text.Replace("Controller", "");
        }
    }
}
