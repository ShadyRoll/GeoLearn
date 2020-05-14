namespace GeoLearn.Extensions
{
    public static class ExtensionsString
    {
        public static string UpperFirstLetter(this string str)
                    => str[0].ToString().ToUpper() + str.Substring(1);
    }
}
