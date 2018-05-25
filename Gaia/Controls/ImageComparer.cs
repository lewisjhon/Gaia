using EyeOpen.Imaging.Processing;
using Gaia.Helper;
using Gaia.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Controls
{
    public class ImageComparer : IDisposable
    {
        //public delegate void OnCalculateCompleteDelegate();
        public Picture Source { get; set; }
        public Picture Target { get; set; }

        public double CalculateSimilarity()
        {
            double similarity = 0.0;

            if (Source.IsNotNull() && Target.IsNotNull())
            {
                ComparableImage pc = new ComparableImage(new FileInfo(Source.Path));
                ComparableImage cc = new ComparableImage(new FileInfo(Target.Path));                              
                
                similarity = pc.CalculateSimilarity(cc);
            }

            //if (OnCalculateComplete.IsNotNull())
                //OnCalculateComplete.Invoke();

            return similarity;
        }

        public void Dispose()
        {
            System.GC.Collect();
        }

        //public event OnCalculateCompleteDelegate OnCalculateComplete;
    }
}
