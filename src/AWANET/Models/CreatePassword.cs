using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.Models
{
    public static class CreatePassword
    {
        static Random rand = new Random();
        public static string CreateNewPassword()
        {
            char[] password = new char[7];
            for (int i = 0; i < 7; i++)
            {
                //caseSwitch roterar över switchen för att generera lösenord. Detta garanterar att vi har liten och stor bokstav, samt siffra
                int caseSwitch = (i % 3) + 1;
                switch (caseSwitch)
                {
                    //Stora bokstäver ligger mellan ascii 65-90
                    case 1:
                        password[i] = (char)rand.Next(65, 90 + 1);
                        break;
                    //Små bokstäver ligger mellan 97-122
                    case 2:
                        password[i] = (char)rand.Next(97, 122 + 1);
                        break;
                    //Siffror ligger mellan 48-57
                    case 3:
                        password[i] = (char)rand.Next(48, 57 + 1);
                        break;
                }
            }
            //Returnerar lösenordet
            return new string(password);
        }
    }
}
