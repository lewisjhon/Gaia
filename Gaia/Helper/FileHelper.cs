using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Helper
{
    public static class FileHelper
    {
        public static bool IsValidImage(this FileInfo item)
        {
            try
            {
                using (Image newImage = Image.FromFile(item.FullName))
                { }
            }
            catch (OutOfMemoryException ex)
            {
                return false;
            }
            catch (NotSupportedException)
            {
                return false;
            }
            return true;
        }
    }
}
