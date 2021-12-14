using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNPOSTWebUILibrary.DataAccess;
using VNPOSTWebUILibrary.Model;
using System.Data;
using VNPOSTWebUILibrary.Model.DataSendToClient;

namespace VNPOSTWebUILibrary.BussinessLogic
{
    public class UnitsProcessor
    {
        public SqlNewsRepository _sqlRepo { get; }

        public UnitsProcessor()
        {
            _sqlRepo = new SqlNewsRepository();
        }

        public async Task<IEnumerable<CategoryUnit>> getAllCategoryUnit()
        {
            string sql = "select * from CategoryUnit";

            var result = await _sqlRepo.LoadData<CategoryUnit>(sql);
            return result;
        }

        public async Task<CategoryUnit> getCategoryUnitWithId(string id)
        {
            string sql = "select * from CategoryUnit where Id = @Id";
            var param = new DynamicParameters();
            param.Add("@Id", id, DbType.String);

            var result =  await _sqlRepo.LoadData<CategoryUnit>(sql, param);
            if(result.Count() == 1)
            {
                return result.FirstOrDefault();
            }

            return null;
        }

        public async Task<bool> addCategoryUnit(CategoryUnit categoryUnit)
        {
            string sql = "insert into CategoryUnit values (@Id, @Name, @Status, @UserId)";
            var param = new DynamicParameters();
            param.Add("@Id", categoryUnit.id, DbType.String);
            param.Add("@Name", categoryUnit.name, DbType.String);
            param.Add("@Status", categoryUnit.status, DbType.Boolean);
            param.Add("@UserId", categoryUnit.userid, DbType.String);

            int result = await _sqlRepo.SaveData(sql, param);
            if(result > 0)
            {
                return true;
            }else
            {
                return false;
            }
        }

        public async Task<bool> addUnit(Unit unit)
        {
            if (await isUnitIdExisted(unit.Id))
            {
                return false;
            }

            string sql = "insert into Unit values (@id, @name, @categoryId, @address, @phone, @unitRankId," +
                         "@license, @date, @status, @note, @userid)";
            var param = new DynamicParameters();
            param.Add("@id", unit.Id, DbType.String);
            param.Add("@name", unit.Name, DbType.String);
            param.Add("@categoryId", unit.CategoryUnitId, DbType.String);
            param.Add("@address", unit.Address, DbType.String);
            param.Add("@phone", unit.Phone, DbType.String);
            param.Add("@unitRankId", unit.UnitRankId, DbType.String);
            param.Add("@license", unit.LicenseNumber, DbType.String);
            param.Add("@date", unit.AppointedDate, DbType.DateTime2);
            param.Add("@status", unit.Status, DbType.Boolean);
            param.Add("@note", unit.Note, DbType.String);
            param.Add("@userid", unit.UserId, DbType.String);

            int result = await _sqlRepo.SaveData(sql, param);
            if(result > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> updateCategoryUnit(CategoryUnit categoryUnit)
        {
            string sql = "update CategoryUnit set [Name] = @name, [Status] = @status where Id = @id";
            var param = new DynamicParameters();
            param.Add("@id", categoryUnit.id, DbType.String);
            param.Add("@name", categoryUnit.name, DbType.String);
            param.Add("@status", categoryUnit.status, DbType.Boolean);

            int rowAffected = await _sqlRepo.SaveData(sql, param);
            if(rowAffected == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> deleteCategoryUnit(string id)
        {
            string sql = "update CategoryUnit set [Status] = 0 where id = @id";
            var param = new DynamicParameters();
            param.Add("@id", id, DbType.String);

            int result = await _sqlRepo.SaveData(sql, param);
            if (result == 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> isCategoryUnitIdExisted(string id)
        {
            string sql = "select Id from CategoryUnit";
            var temp = await _sqlRepo.LoadData<string>(sql);
            var ids = temp.ToHashSet();
            if (ids.Contains(id))
            {
                return true;
            }
            return false;
        }

        public async Task<bool> isUnitIdExisted(string id)
        {
            string sql = "select Id from Unit";
            var temp = await _sqlRepo.LoadData<string>(sql);
            var ids = temp.ToHashSet();
            if (ids.Contains(id))
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<CategoryUnitForListUnit>> fillDataForListUnit()
        {
            string sqlForCategoryUnit = "select Id, [Name], [status] from CategoryUnit";
            string sqlForUnit = "select Id, [Name], [status] From Unit where CategoryUnitId = @id";
            var CategoryUnit = await _sqlRepo.LoadData<CategoryUnitForListUnit>(sqlForCategoryUnit);
            DynamicParameters param;
            foreach (var item in CategoryUnit)
            {
                param = new DynamicParameters();
                param.Add("@id", item.id, DbType.String);
                item._children = await _sqlRepo.LoadData<UnitForListUnit>(sqlForUnit, param);
            }
            return CategoryUnit;
        }

        public async Task<bool> deleteUnit(string id)
        {
            string sql = "update Unit set [Status] = 0 where Id = @id";
            var param = new DynamicParameters();
            param.Add("@id", id, DbType.String);

            int result = await _sqlRepo.SaveData(sql, param);
            if(result > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<Unit> getUnitById(string id)
        {
            string sql = "select * from Unit where Id = @id";
            var param = new DynamicParameters();
            param.Add("@id", id, DbType.String);
            var result = await _sqlRepo.LoadData<Unit>(sql,param);
            if(result.Count() == 0)
            {
                return null;
            }
            return result.FirstOrDefault();
        }

        public async Task<bool> updateUnit(Unit unit)
        {
            string sql = "update Unit " +
                "set [Name] = @name, CategoryUnitId = @categoryid, [Address] = @address, Phone = @phone, " +
                "UnitRankId = @rankid, LicenseNumber = @license, AppointedDate = @date, [Status] = @status, " +
                "Note = @note, UserId = @userid where Id = @id";
            var param = new DynamicParameters();
            param.Add("id", unit.Id, DbType.String);
            param.Add("@name", unit.Name , DbType.String);
            param.Add("@categoryid", unit.CategoryUnitId, DbType.String);
            param.Add("@address", unit.Address, DbType.String);
            param.Add("@phone", unit.Address, DbType.String);
            param.Add("@rankid", unit.UnitRankId, DbType.String);
            param.Add("@license", unit.LicenseNumber, DbType.String);
            param.Add("@date", unit.AppointedDate, DbType.DateTime2);
            param.Add("@status", unit.Status, DbType.Boolean);
            param.Add("@note", unit.Note, DbType.String);
            param.Add("@userid", unit.UserId, DbType.String);

            int result = await _sqlRepo.SaveData(sql, param);
            if(result == 0)
            {
                return false;
            } 
            return true;
        }
    }
}
