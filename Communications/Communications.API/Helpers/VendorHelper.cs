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
            return true;
            //return _context.Vendors.Any(x => x.Id == id);
        }
    }
}
