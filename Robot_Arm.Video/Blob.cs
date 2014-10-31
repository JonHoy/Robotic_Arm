using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot_Arm.Video
{
    // blob class
    public class Blob
    {
        public int BlobNumber = 0;
        public int PixelCount = 0;
        public int Xmax = 0;
        public int Xmin = 0;
        public int Xsum = 0;
        public int Ymax = 0;
        public int Ymin = 0;
        public int Ysum = 0;
        public int X_Centroid = 0;
        public int Y_Centroid = 0;
        public bool[,] BW_Image; // A binary image of the blob 

        public static Blob MergeBlobs(Blob LowerBlob, Blob UpperBlob)
        {
            Blob MergedBlob = new Blob();
            MergedBlob.BlobNumber = Math.Min(UpperBlob.BlobNumber, LowerBlob.BlobNumber);
            MergedBlob.PixelCount = LowerBlob.PixelCount + UpperBlob.PixelCount;
            MergedBlob.Xmax = Math.Max(LowerBlob.Xmax, UpperBlob.Xmax);
            MergedBlob.Xmin = Math.Min(LowerBlob.Xmin, UpperBlob.Xmin);
            MergedBlob.Xsum = LowerBlob.Xsum + UpperBlob.Xsum;
            MergedBlob.Ymax = Math.Max(LowerBlob.Ymax, UpperBlob.Ymax);
            MergedBlob.Ymin = Math.Min(LowerBlob.Ymin, UpperBlob.Ymin);
            MergedBlob.Ysum = LowerBlob.Ysum + UpperBlob.Ysum;
            MergedBlob.X_Centroid = MergedBlob.Xsum / MergedBlob.PixelCount;
            MergedBlob.Y_Centroid = MergedBlob.Ysum / MergedBlob.PixelCount;
            return MergedBlob;
        }
    }
}
