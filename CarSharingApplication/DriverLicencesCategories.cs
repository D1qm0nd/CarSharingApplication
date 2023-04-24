using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSharingApplication
{
    public class DriverLicencesCategories
    {
        public string Name { get; set; }
        
        public string ReceiptDate { get; set; }
        public string EndDate { get; set; }
        
        public DriverLicencesCategories(string name, string receiptdate, string endDate) 
        {       
            Name = name;    
            ReceiptDate = receiptdate;
            EndDate = endDate;
        }
    }
}
