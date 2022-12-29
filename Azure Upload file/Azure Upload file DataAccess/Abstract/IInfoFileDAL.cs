using Azure_Upload_file_Core.Data_Access;
using Azure_Upload_file_Entities.Concrete.DatabaseFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Upload_file_DataAccess.Abstract
{
    public interface IInfoFileDAL : IEntityRepository<InfoFiles>
    {
    }
}
