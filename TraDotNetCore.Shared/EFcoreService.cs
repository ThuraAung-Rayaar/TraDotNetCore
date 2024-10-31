using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TraDotNetCore.Shared
{
    public class EFcoreService
    {
        private readonly string _connectionString;
        public EFcoreService(string connection) {
            _connectionString = connection; 
        
        
        }

        public void Query(string query) { 
        
            
        
        }


    }
}
