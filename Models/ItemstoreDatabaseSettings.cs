using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop_Bridge.Models
{
    
        public class ItemstoreDatabaseSettings : IItemstoreDatabaseSettings
        {
            public string ItemsCollectionName { get; set; }
            public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }
        }

        public interface IItemstoreDatabaseSettings
        {
            string ItemsCollectionName { get; set; }
            string ConnectionString { get; set; }
            string DatabaseName { get; set; }
        }


    
}
