using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W1400.DataAccess.DAO;
using W1400.DataAccess.DAOImpl;
namespace W1400.DataAccess.Factory
{
    public class ADODAOFactory : AbstractDAOFactory
    {
        public override IUsersDAO CreateUsersDAO()
        {
            return new UsersDAOImpl();
        }

        public override IUsersLogDAO CreateUsersLogDAO()
        {
            return new UsersLogDAOImpl();
        }
        public override IErrorLogDAO CreateErrorLogDAO()
        {
            return new ErrorLogDAOImpl();
        }
        public override IFucntionsDAO CreateFunctionDAO()
        {
            return new FunctionsDAOImpl();
        }
        public override IUserRoleDAO CreateUserRoleDAO()
        {
            return new UserRoleDAOImpl();
        }

        public override CmsDAO CreateCmsDAO()
        {
            return new CmsDAOImpI();
        }

        public override ArticleDAO CreateArticleDAO()
        {
            return new ArticleDAOImpl();
        }

        public override ProgramDAO CreateProgramDAO()
        {
            return new ProgramDAOImpl();
        }

        public override MessageDAO CreateMessageDAO()
        {
            return new MessageDAOImpl();
        }

        public override SuggestDAO CreateSuggestDAO()
        {
            return new SuggestDAOImpl();
        }
    }
}
