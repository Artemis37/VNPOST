using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNPOSTWebUILibrary.DataAccess;
using VNPOSTWebUILibrary.Model;

namespace VNPOSTWebUILibrary.BussinessLogic
{
    public class NewsProcessor
    {
        private readonly SqlNewsRepository _sqlRepo;

        public NewsProcessor()
        {
            _sqlRepo = new SqlNewsRepository();
        }

        public async Task<IEnumerable<NewsBlock>> LoadLastestAndOldestNewsOfEachMajorAsync()
        {
            string sql = @"select * from (select top 5 * from News where MajorGroupId = @majorGroupId order by CreatedDate DESC) first " +
                         "UNION ALL " +
                         "select * from (select top 1 * from News where MajorGroupId = @majorGroupId order by CreatedDate ASC) last";
            var majorGroup = LoadMajorNewsGroupAsync();
            List<NewsBlock> InitialNewsList = new List<NewsBlock>();//list of NewsBlock is returned
            foreach (var item in majorGroup.Result)
            {
                var param = new DynamicParameters();
                param.Add("majorGroupId", item.Id, DbType.Int32);
                var tempTask = await _sqlRepo.LoadData<News>(sql,param);
                var temp = tempTask.ToList();
                List<News> tempNewsList = new List<News>();
                foreach (var innerItem in temp)
                {
                    tempNewsList.Add(innerItem);
                }
                InitialNewsList.Add(new NewsBlock(){ 
                    MajorGroupId = item.Id,
                    MajorGroupName = item.Name,
                    newsBlock = tempNewsList 
                });
            }
            return InitialNewsList;
        }

        public async Task<IEnumerable<News>> LoadNewsAsync()
        {
            string sql = "select * from News";

            return await _sqlRepo.LoadData<News>(sql);
        }

        public async Task<News> LoadNewsWithIdAsync(int id)
        {
            string sql = "select * from News where Id = @id";
            var param = new DynamicParameters();
            param.Add("id", id, DbType.Int32);

            var tempNews = await _sqlRepo.LoadData<News>(sql, param);
            if (tempNews.ToList().Count > 0) return tempNews.First();
            else return null;

        }

        public async Task<IEnumerable<News>> LoadNewsWithMajorGroupIdAsync(int id)
        {
            string sql = "select * from News where MajorGroupId = @id order by CreatedDate desc";
            var param = new DynamicParameters();
            param.Add("id",id);

            var newsList = await _sqlRepo.LoadData<News>(sql,param);
            return newsList;
        }

        public async Task<IEnumerable<News>> LoadNewsWithMajorGroupIdAsync(int id, int range)
        {
            string sql = "select TOP(@range) * from News where MajorGroupId = @id order by CreatedDate desc";
            var param = new DynamicParameters();
            param.Add("id",id, DbType.Int32);
            param.Add("range", range, DbType.Int32);

            var newsList = await _sqlRepo.LoadData<News>(sql,param);
            return newsList;
        }

        public async Task<IEnumerable<News>> LoadNewsWithGroupIdAsync(int? id)
        {
            if (id == null) return new List<News>();
            string sql = "select * from News where GroupId = @id order by CreatedDate desc";
            var param = new DynamicParameters();
            param.Add("id",id);

            var newsList = await _sqlRepo.LoadData<News>(sql,param);
            return newsList;
        }

        public async Task<IEnumerable<News>> LoadNewsWithGroupIdAsync(int? id, int range)
        {
            if (id == null) return new List<News>();

            string sql = "select TOP(@range) * from News where GroupId = @id order by CreatedDate desc";
            var param = new DynamicParameters();
            param.Add("id", id, DbType.Int32);
            param.Add("range", range, DbType.Int32);

            var newsList = await _sqlRepo.LoadData<News>(sql,param);
            return newsList;
        }

        public async Task<IEnumerable<NewsGroup>> LoadNewsGroupAsync()
        {
            string sql = "select * from NewsGroup";

            return await _sqlRepo.LoadData<NewsGroup>(sql);
        }

        public async Task<IEnumerable<MajorNewsGroup>> LoadMajorNewsGroupAsync()
        {
            string sql = "select * from MajorNewsGroup";

            return await _sqlRepo.LoadData<MajorNewsGroup>(sql);
        }

        public async Task<int> AddNews(News news)
        {
            string sql = @"insert into News(Title, Summary, Content, CreatedDate, CreatedBy, GroupId, MajorGroupId, Views, LabelImage)
                        values(@Title,@Summary,@Content,@CreatedDate,@CreatedBy,@GroupId,@MajorGroupId,@Views, @LabelImage)";
            var param = new DynamicParameters();
            param.Add("Title", news.Title, DbType.String);
            param.Add("Summary", news.Summary, DbType.String);
            param.Add("Content", news.Content, DbType.String);
            param.Add("CreatedDate", news.CreatedDate, DbType.DateTime2);
            param.Add("CreatedBy", news.CreatedBy, DbType.String);
            if(news.GroupId != 0) param.Add("GroupId", news.GroupId, DbType.Int32);
            else param.Add("GroupId", DBNull.Value, DbType.Int32);
            param.Add("MajorGroupId", news.MajorGroupId, DbType.Int32);
            param.Add("Views", news.Views, DbType.Int32);
            param.Add("LabelImage", news.LabelImage, DbType.String);

            return await _sqlRepo.SaveData(sql, param);
        }

        public async Task DeleteNews(int id)
        {
            string sql = "delete from News where Id = @id";

            var param = new DynamicParameters();
            param.Add("id", id, DbType.Int32);

            await _sqlRepo.SaveData(sql, param);
        }

        public async Task UpdateNews(News news)
        {
            string sql = "UPDATE News " +
                         "SET Title = @Title, Content = @Content, " +
                         "CreatedDate = @CreatedDate, CreatedBy = @CreatedBy, GroupId = @GroupId, MajorGroupId = @MajorGroupId, " +
                         "[Views] = @Views, LabelImage = @LabelImage, Summary = @Summary " +
                         "WHERE Id = @Id";

            var param = new DynamicParameters();
            param.Add("Id", news.Id, DbType.Int32);
            param.Add("Title", news.Title, DbType.String);
            param.Add("Content", news.Content, DbType.String);
            param.Add("CreatedDate", news.CreatedDate, DbType.DateTime2);
            param.Add("CreatedBy", news.CreatedBy, DbType.String);
            if(news.GroupId == 0) param.Add("GroupId", DBNull.Value, DbType.Int32);
            else param.Add("GroupId", news.GroupId, DbType.Int32);
            param.Add("MajorGroupId", news.MajorGroupId, DbType.Int32);
            param.Add("Views", news.Views, DbType.Int32);
            param.Add("LabelImage", news.LabelImage, DbType.String);
            param.Add("Summary", news.Summary, DbType.String);

            await _sqlRepo.SaveData(sql, param);
        }

        public async Task AddMajorNewsGroupAsync(string name)
        {
            string sql = "insert into [MajorNewsGroup] values (@name)";

            var param = new DynamicParameters();
            param.Add("name", name, DbType.String);

            await _sqlRepo.SaveData(sql, param);
        }

        public async Task AddNewsGroupAsync(string name, int majorGroupId)
        {
            string sql = "insert into [NewsGroup] values (@name, @MajorGroupId)";

            var param = new DynamicParameters();
            param.Add("name", name, DbType.String);
            param.Add("MajorGroupId", majorGroupId, DbType.Int32);

            await _sqlRepo.SaveData(sql, param);
        }
    }
}
