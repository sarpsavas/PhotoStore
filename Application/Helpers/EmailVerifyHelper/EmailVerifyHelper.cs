using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers.EmailVerifyHelper
{
    public class EmailVerifyHelper
    {
        public bool EmailVerify(string email)
        {
            try
            {
                var ver = new MailAddress(email);
                return ver.Address == email;
            }
            catch (Exception)
            {

                return false;
            }
            
        }
    }
}
