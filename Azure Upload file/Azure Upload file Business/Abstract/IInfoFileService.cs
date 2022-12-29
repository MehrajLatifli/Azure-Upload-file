using Azure_Upload_file_Entities.Concrete.DatabaseFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure_Upload_file_Business.Abstract
{
    public interface IInfoFileService
    {
        List<InfoFiles> GetAll();

        void Add(InfoFiles item);
        void Update(InfoFiles item);
        void Delete(InfoFiles item);
        InfoFiles GetById(int Id);
    }
}
