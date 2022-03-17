namespace CodeGen
{
    public static class StringExtensions
    {
        public static string ToLowerFirst(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Substring(0, 1).ToLower() + s.Substring(1);
        }

        public static string ToUpperFirst(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Substring(0, 1).ToUpper() + s.Substring(1);
        }

        public static string ToLower(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.ToLower();
        }

        public static string ToUpper(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.ToUpper();
        }

        public static string ToEntityName(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            string[] arr = s.Split('_');

            if (arr.Length > 0)
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    //arr[i] = UpFirst(arr[i]);
                    arr[i] = arr[i].ToUpperFirst();
                }
                return string.Join("", arr);
            }
            return s;
        }

        public static string ToentityName(this string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return ToLowerFirst(ToEntityName(s));
        }
    }
}