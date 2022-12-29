using Azure_Upload_file_Business.Abstract;
using Azure_Upload_file_DataAccess.Abstract;
using Azure_Upload_file_Entities.Concrete.DatabaseFirst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Azure_Upload_file_Business.Concrete
{
    public class InfoFileManager : IInfoFileService
    {

        private IInfoFileDAL  _infoFileDAL;


        public InfoFileManager(IInfoFileDAL infoFileDAL)
        {
            _infoFileDAL = infoFileDAL;
        }

        public void Add(InfoFiles item)
        {
            _infoFileDAL.Add(item);
        }

        public void Delete(InfoFiles item)
        {
            _infoFileDAL.Delete(item);
        }

        public List<InfoFiles> GetAll()
        {
            return _infoFileDAL.GetList();
        }

        public InfoFiles GetById(int Id)
        {
            return _infoFileDAL.Get(p => p.IdFile == Id);
        }

        public void Update(InfoFiles item)
        {
            _infoFileDAL.Update(item);
        }
    }
}
