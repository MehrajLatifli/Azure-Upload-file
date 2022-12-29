using Azure_Upload_file_Core.Data_Access.EntityFrameworkCore;
using Azure_Upload_file_DataAccess.Abstract;
using Azure_Upload_file_Entities.Concrete.DatabaseFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Upload_file_DataAccess.Concrete
{
    public class EF_InfoFileDAL : EF_EntityRepositoryBase<InfoFiles, InfoFileContext>, IInfoFileDAL
    {
    }
}
