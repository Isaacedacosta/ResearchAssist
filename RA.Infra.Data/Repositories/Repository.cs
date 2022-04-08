using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.Infra.Data.Repositories
{
    public class Repository<TypeEntity> where TypeEntity : class
    {

        public bool insert(TypeEntity entity)
        {

            using (RADBContext.RADBConnection)
            {

            }

            return true;
        }
    }
}
