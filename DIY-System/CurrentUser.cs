using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIY_System
{
    public static class CurrentUser
    {
        public static int UserID { get; set; }
        public static int RoleID { get; set; }

        public static bool IsAdmin
        {
            get
            {
                return RoleID == 2;
            }
        }
    }
}
