using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNPOSTWebUILibrary.Model
{
    public class AllClaim
    {
        public AllClaim()
        {
            ManageApplicationRead = false;
            ManageApplicationUpdate = false;
            ManageApplicationUserAdd = false;
            ManageApplicationUserUpdate = false;
            ManageUserGroupRead = false;
            ManageUserGroupAdd = false;
            ManageUserGroupUpdate = false;
            ManageUserGroupDelete = false;
            ManageUserGroupRolesAdd = false;
            ManageUserRead = false;
            ManageUserAdd = false;
            ManageUserUpdate = false;
            ManageUserUpdateUserGroup = false;
        }


        //Application
        public bool ManageApplicationRead { get; set; }
        public bool ManageApplicationUpdate { get; set; }
        public bool ManageApplicationUserAdd { get; set; }
        public bool ManageApplicationUserUpdate { get; set; }

        //User Group
        public bool ManageUserGroupRead { get; set; }
        public bool ManageUserGroupAdd { get; set; }
        public bool ManageUserGroupUpdate { get; set; }
        public bool ManageUserGroupDelete { get; set; }
        public bool ManageUserGroupRolesAdd { get; set; }
        public bool ManageUserGroupDetail { get; set; }

        //User
        public bool ManageUserRead { get; set; }
        public bool ManageUserDetail { get; set; }
        public bool ManageUserAdd { get; set; }
        public bool ManageUserUpdate { get; set; }
        public bool ManageUserUpdateUserGroup { get; set; }
    }
}
