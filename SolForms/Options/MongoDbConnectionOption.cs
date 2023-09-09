using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolForms.Options
{
    public class MongoDbConnectionOption
    {
        public string ConnectionString { get; set; } = "mongodb://localhost:27017/";
        public string DataBaseName { get; set; } = "SolForms";
        
    }
}
