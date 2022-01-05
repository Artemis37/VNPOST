using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNPOSTWebUILibrary.DataAccess;
using VNPOSTWebUILibrary.Model;
using VNPOSTWebUILibrary.Model.DataSendToClient;

namespace VNPOSTWebUILibrary.BussinessLogic
{
    public class RolesProcessor
    {
        private readonly SqlNewsRepository _sqlRepo;

        public RolesProcessor()
        {
            _sqlRepo = new SqlNewsRepository();
        }

        public async Task<Roles> LoadRelativeRoles(string id)
        {
            Roles tempRoles = new Roles()
            {
                userid = id,
                newsAdd = false,
                newsDelete = false,
                newsEdit = false,
                unitAdd = false,
                unitRead = false,
                unitDelete = false,
                unitEdit = false
            };

            DynamicParameters param = new DynamicParameters();
            param.Add("@userid", id, DbType.String);
            var roles = await _sqlRepo.ExecStoredProcedure<string>("GetUserRoleList", param);
            foreach (var role in roles)
            {
                switch (role)
                {
                    case "NewsAdd":
                        tempRoles.newsAdd = true;
                        break;
                    case "NewsDelete":
                        tempRoles.newsDelete = true;
                        break;
                    case "NewsEdit":
                        tempRoles.newsEdit = true;
                        break;
                    case "UnitRead":
                        tempRoles.unitRead = true;
                        break;
                    case "UnitAdd":
                        tempRoles.unitAdd = true;
                        break;
                    case "UnitEdit":
                        tempRoles.unitEdit = true;
                        break;
                    case "UnitDelete":
                        tempRoles.unitDelete = true;
                        break;
                }
            }
            var ob = tempRoles;
            return tempRoles;
        }

        public async Task<IEnumerable<RolesForList>> loadRoles()
        {
            string sql = "select Id, [Name] from AspNetRoles";

            return await _sqlRepo.LoadData<RolesForList>(sql);
        }

        public async Task<IEnumerable<RolesForList>> loadAppoinedRolesById(string id)
        {
            string sql = "select b.Id, b.Name from AspNetUserRoles as a inner join AspNetRoles as b on a.RoleId = b.Id where UserId = @Id";
            var param = new DynamicParameters();
            param.Add("@Id", id, DbType.String);

            return await _sqlRepo.LoadData<RolesForList>(sql, param);
        }

        public async Task<AllClaim> loadClaim(string id)
        {
            //Returned Model
            AllClaim result = new AllClaim(); 

            string sql = "select ClaimValue from AspNetRoleClaims where RoleId = @id";
            var param = new DynamicParameters();
            param.Add("@id", id, DbType.String);
            var allClaimOfId = await _sqlRepo.LoadData<string>(sql, param);
            foreach (var item in allClaimOfId)
            {
                //Application
                if(item == "ManageApplication.Read") result.ManageApplicationRead = true;
                if(item == "ManageApplication.Update") result.ManageApplicationUpdate = true;
                if(item == "ManageApplication.User.Add") result.ManageApplicationUserAdd = true;
                if(item == "ManageApplication.User.Update") result.ManageApplicationUserUpdate = true;

                //UserGroup
                if(item == "ManageUserGroup.Read") result.ManageUserGroupRead = true;
                if(item == "ManageUserGroup.Add") result.ManageUserGroupAdd = true;
                if(item == "ManageUserGroup.Update") result.ManageUserGroupUpdate = true;
                if(item == "ManageUserGroup.Delete") result.ManageUserGroupDelete = true;
                if(item == "ManageUserGroup.Roles.Add") result.ManageUserGroupRolesAdd = true;
                if(item == "ManageUserGroup.Detail") result.ManageUserGroupDetail = true;

                //User
                if(item == "ManageUser.Read") result.ManageUserRead = true;
                if(item == "ManageUser.Detail") result.ManageUserDetail = true;
                if(item == "ManageUser.Add") result.ManageUserAdd = true;
                if(item == "ManageUser.Update") result.ManageUserUpdate = true;
                if(item == "ManageUser.UpdateUserGroup") result.ManageUserUpdateUserGroup = true;
            }

            return result;
        }

        public async Task removeRoleClaimById(string roleId)
        {
            string sql = "truncate table AspNetRoleClaims";

            await _sqlRepo.SaveData(sql);
        }
    }
}
