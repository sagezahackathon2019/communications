using Communications.API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Communications.API.Helpers
{
    public class VendorHelper
    {
        private MainDbContext _context;
        
        public VendorHelper(MainDbContext context)
        {
            _context = context;
        }

        public bool VendorExists(Guid id)
        {
            if (id == Guid.Parse("7c15722b-89dd-4a85-9ed0-de1aaee79301"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
