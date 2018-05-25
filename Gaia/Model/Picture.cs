using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Model
{
    public class Picture
    {
        public int No { get; set; }
        public DateTime CreateDate { get; set; }
        public string Path { get; set; }        
        public string Size { get; set; } // ex> 1024 x 768
        public string Extention { get; set; }
        public long DiskVolumn { get; set; } 
        public bool Checked { get; set; }
        public bool Delete { get; set; }
    }
}
