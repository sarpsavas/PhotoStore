using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string EMail { get; private set; }
        public string PasswordHash { get; set; }
        
        public User()
        {
            UserId = Guid.NewGuid();
        }

        public void setEMail(string eMail)
        {
            var ver = new MailAddress(eMail);
            if(ver.Address == eMail)
            {
                EMail = eMail;
            }
            else
            {
                throw new Exception("Email formatı değil");
            }
        }
    }
    
}
