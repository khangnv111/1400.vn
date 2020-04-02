using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W1400.DataAccess.DAO;
using W1400.DataAccess.DAOImpl;

namespace W1400.DataAccess.Factory
{
    public abstract class AbstractDAOFactory
    {
        public static AbstractDAOFactory Instance()
        {
            try
            {
                return (AbstractDAOFactory)new ADODAOFactory();
            }
            catch (Exception ex)
            {

                throw new Exception("Couldn't create AbstractDAOFactory: ");
            }
        }
        public abstract IUsersDAO CreateUsersDAO();
        public abstract IUsersLogDAO CreateUsersLogDAO();
        public abstract IErrorLogDAO CreateErrorLogDAO();
        public abstract IFucntionsDAO CreateFunctionDAO();
        public abstract IUserRoleDAO CreateUserRoleDAO();
        public abstract ArticleDAO CreateArticleDAO();

        public abstract CmsDAO CreateCmsDAO();
        public abstract ProgramDAO CreateProgramDAO();
        public abstract MessageDAO CreateMessageDAO();
        public abstract SuggestDAO CreateSuggestDAO();
    }
}
