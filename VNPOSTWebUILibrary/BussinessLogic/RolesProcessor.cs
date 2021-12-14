using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNPOSTWebUILibrary.DataAccess;
using VNPOSTWebUILibrary.Model;

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

        //public async Task SaveRelativeRoles(Roles roles)
        //{

        //}
    }
}
