using System;
using System.Text;

namespace pindwin.development
{
    public static class Conversions
    {
        public static string ToPrettyString(this Type t)
        {
            if (t.IsGenericParameter)
            {
                return t.Name;
            }
            
            if (!t.IsGenericType)
            {
                return t.FullName;
            }

            Type[] args = t.GenericTypeArguments;
            StringBuilder sb = new StringBuilder();
            string[] parts = t.FullName.Split('`');
            sb.Append(parts[0]);
            sb.Append("<");
            for (int i = 0; i < args.Length; i++)
            {
                sb.Append(ToPrettyString(args[i]));
                if (i < args.Length - 1)
                {
                    sb.Append(",");
                }
            }

            sb.Append(">");
            return sb.ToString();
        }
    }
}
