//using DotNetBatch5.RestAPI.ViewModels;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraDotNetCore.Shared;
using DotNetCoreTraining.Domain.DataModels;
using DotNetCoreTraining.Domain.ViewModels;
namespace DotNetCoreTraining.Domain.Features.Blogs
{
    public class BlogServices
    {
        private readonly string _connectionString = "Data Source = .;Initial Catalog = DotNetTraningBatch5;User ID =sa; Password = sasa@123;TrustServerCertificate  = True";
        private readonly AdoDotNetService _AdoDotNetService;

        public BlogServices()
        {

            _AdoDotNetService = new AdoDotNetService(_connectionString);
        }
        public List<BlogDataModel> GetBlogs(string query)
        {
            List<BlogDataModel> B_list = new List<BlogDataModel>();
            var BlogTable = _AdoDotNetService.Query(query);
            foreach (DataRow dr in BlogTable.Rows)
            {

                B_list.Add(new BlogDataModel
                {
                    BlogId = Convert.ToInt32(dr["BlogId"]),
                    BlogTitle = Convert.ToString(dr["BlogTitle"]),
                    BlogAuthor = Convert.ToString(dr["BlogAuthor"]),
                    BlogContent = Convert.ToString(dr["BlogContent"]),
                    DeleteFlag = Convert.ToBoolean(dr["DeleteFlag"]),


                });
            }

            return B_list;
        }
        public BlogDataModel? GetBlog(string query,int id) {

            BlogDataModel item = new BlogDataModel();
            DataTable table = _AdoDotNetService.Query(query, new SqlParameterModel("@ID", id.ToString()));


            if (table.Rows.Count == 0) return null;

            item = new BlogDataModel()
            { BlogId = id,
                BlogTitle = table.Rows[0]["BlogTitle"].ToString(),
                BlogAuthor = table.Rows[0]["BlogAuthor"].ToString(),
                BlogContent = table.Rows[0]["BlogContent"].ToString(),
                DeleteFlag = Convert.ToBoolean(table.Rows[0]["DeleteFlag"])

            };

            return item;

        }

        public int DeleteBlog(string query,int id)
        {

           
            int result = _AdoDotNetService.Excute(query, new SqlParameterModel("@ID", id.ToString()));

            return result;
        }


        public BlogDataModel CreateBlog(string query,BlogDataModel model,params SqlParameterModel[] sqlParameters)
        {

            // string query = $@"insert into Tbl_Blog values (@BlogTitle,@BlogAuthor,@BlogContent,0)";
            int result = _AdoDotNetService.Excute(query, sqlParameters);
               
            if (result == 0) { return model; }
            else { return BlogDataModel.GetDefault(); }
        }



    }
}
