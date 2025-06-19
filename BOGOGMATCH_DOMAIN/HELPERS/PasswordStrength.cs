using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BOGOMATCH_DOMAIN.HELPERS
{
    public static class PasswordStrength
    {
        public static string CheckPasswordStrength(string pass)
        {
            var sb = new StringBuilder();
            if (pass.Length < 9)
                sb.AppendLine("Minimum password length should be 9.");
            if (!(Regex.IsMatch(pass, "[a-z]") && Regex.IsMatch(pass, "[A-Z]") && Regex.IsMatch(pass, "[0-9]")))
                sb.AppendLine("Password must contain uppercase, lowercase, and numeric characters.");
            if (!Regex.IsMatch(pass, "[<,>,@,!,#,$,%,^,&,*,(,),_,+,\\[,\\],{,},?,:,;,|,',\\,.,/,~,`,-,=]"))
                sb.AppendLine("Password should contain at least one special character.");

            return sb.ToString();
        }
    }
}
