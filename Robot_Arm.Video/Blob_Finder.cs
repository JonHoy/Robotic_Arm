using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;

namespace Robot_Arm.Video
{
    

    public class BlobFinder
    {
        public bool[,] BW {get; private set;}
        private int BlobCount;
        public Blob[] Blobs {get; private set;}
        public int[,] AssignedBlob {get; private set;}

        public BlobFinder(bool[,] BW)
        {
            this.BW = BW;
            // Turns a Black and White Image to a bunch of blobs
            Get_1D_Blobs(); // turn the image to 1d blobs
            MatchBlobs(); // start lining up the 1d blobs and if they connect give them the same blob#
            Assign_Blobs();  // make an image that has
            FixBlobs(); // check to make sure the algorithm worked completely (if not, recursively use it until it does)
            ConsolidateBlobs(); // merge all the blob objects with the same blob number
        }

        private void Get_1D_Blobs()
        {
            this.Blobs = new Blob[BW.Length];
            BlobCount = 0;
            Blobs[0] = new Blob();
            for (int j = 0; j < BW.GetLength(1); j++)
            {
                for (int i = 0; i < BW.GetLength(0); i++)
                {
                    if (BW[i,j])
                    {
                        if (i == 0 || BW[i - 1, j] == false)
                        {
                            BlobCount++;
                            Blobs[BlobCount] = new Blob();
                            Blobs[BlobCount].BlobNumber = BlobCount + 1;
                            Blobs[BlobCount].Ymin = i;
                            Blobs[BlobCount].Xmin = j;
                            Blobs[BlobCount].Xmax = j;
                        }
                        Blobs[BlobCount].PixelCount++;
                        Blobs[BlobCount].Ymax = i;
                        Blobs[BlobCount].Ysum += i;
                        Blobs[BlobCount].Xsum += j;
                    }
       
                }
            }
            Blob[] BlobsResize = this.Blobs;
            Array.Resize(ref BlobsResize, BlobCount);
            this.Blobs = BlobsResize;
            
        }

        private void Assign_Blobs() {
            this.AssignedBlob = new int[BW.GetLength(0), BW.GetLength(1)];
            for (int iBlob = 0; iBlob < Blobs.Length; iBlob++)
            {
                for (int i = Blobs[iBlob].Ymin; i <= Blobs[iBlob].Ymax; i++)
                {
                    for (int j = Blobs[iBlob].Xmin; j <= Blobs[iBlob].Xmax; j++)
                    {
                        AssignedBlob[i, j] = Blobs[iBlob].BlobNumber;
                    }
                }
            }
        }

        private void MatchBlobs()
        {
            if (BlobCount > 0)
            {
                int LowerEnd = 0;
                int UpperEnd = 0;
                int LowerStart = 0;
                int UpperStart = 0;

                int[] XstartPts = new int[BW.GetLength(1)];
                int[] XendPts = new int[XstartPts.Length];
                for (int iPt = 0; iPt < XstartPts.Length; iPt++)
                {      
		            XstartPts[iPt] = Array.FindIndex(Blobs, x => x.Xmin == iPt);
                    XendPts[iPt] = Array.FindLastIndex(Blobs, x => x.Xmin == iPt);
                }

                for (int iCol = 1; iCol < XstartPts.Length; iCol++)
                {
                    LowerStart = XstartPts[iCol - 1];
                    UpperStart = XstartPts[iCol];
                    LowerEnd = XendPts[iCol - 1];
                    UpperEnd= XendPts[iCol];

                    if (LowerStart == -1 || UpperStart == -1)
	                {
		                continue;
	                }

                    Blob[] LowerRowBlobs = new Blob[LowerEnd - LowerStart + 1];
                    Blob[] UpperRowBlobs = new Blob[UpperEnd - UpperStart + 1];

                    // Assign the blobs
                    for (int iUpper = 0; iUpper < UpperRowBlobs.Length; iUpper++)
                    {
                         UpperRowBlobs[iUpper] = Blobs[UpperStart + iUpper];
                    }
                    for (int iLower = 0; iLower < LowerRowBlobs.Length; iLower++)
                    {
                        LowerRowBlobs[iLower] = Blobs[LowerStart + iLower];
                    }

                    for (int iLower = 0; iLower < LowerRowBlobs.Length; iLower++)
                    {
                        int yp1 = LowerRowBlobs[iLower].Ymin;
                        int yp2 = LowerRowBlobs[iLower].Ymax;
                        for (int iUpper = 0; iUpper < UpperRowBlobs.Length; iUpper++)
                        {
                            int y1 = UpperRowBlobs[iUpper].Ymin;
                            int y2 = UpperRowBlobs[iUpper].Ymax;
                            bool BlobMatch = Overlap(y1, y2, yp1, yp2);
                            if (BlobMatch)
                            {
                                int MinBlobNumber = Math.Min(LowerRowBlobs[iLower].BlobNumber, UpperRowBlobs[iUpper].BlobNumber);
                                LowerRowBlobs[iLower].BlobNumber = MinBlobNumber;
                                UpperRowBlobs[iUpper].BlobNumber = MinBlobNumber;
                            }
                        }
                    }
                    // Reassign the blobs
                    for (int iUpper = 0; iUpper < UpperRowBlobs.Length; iUpper++)
                    {
                        Blobs[UpperStart + iUpper] = UpperRowBlobs[iUpper];
                    }
                    for (int iLower = 0; iLower < LowerRowBlobs.Length; iLower++)
                    {
                        Blobs[LowerStart + iLower] = LowerRowBlobs[iLower];
                    }
                }

            }
            
 
        }

        private void ConsolidateBlobs()
        {
            if (Blobs.Length <= 1)
            {
                return;
            }
            Array.Sort(Blobs, (x, y) => x.BlobNumber - y.BlobNumber);
            int BlobCount = 1;
            for (int iBlob = 1; iBlob < Blobs.Length; iBlob++)
            {
                if (Blobs[iBlob].BlobNumber != Blobs[iBlob-1].BlobNumber)
                {
                    BlobCount++; // count number of unique blobs (if blob number is the same do not add to the blob count
                }
            }
            Blob[] MergedBlobs = new Blob[BlobCount];
            int CurrentBlob = 0;
            MergedBlobs[CurrentBlob] = Blobs[0];
            for (int iBlob = 1; iBlob < Blobs.Length; iBlob++)
            {
                if ((Blobs[iBlob].BlobNumber == Blobs[iBlob - 1].BlobNumber))
                {
                    MergedBlobs[CurrentBlob] = Blob.MergeBlobs(Blobs[iBlob],MergedBlobs[CurrentBlob]);
                }
                else 
                {
                    CurrentBlob++;
                    MergedBlobs[CurrentBlob] = Blobs[iBlob]; // reset the merge process
                }
            }
            Blobs = MergedBlobs;
            this.BlobCount = Blobs.Length;
            // now add in the blob bw image
            for (int iBlob = 0; iBlob < Blobs.Length; iBlob++)
            {
                int Width = Blobs[iBlob].Xmax - Blobs[iBlob].Xmin + 1;
                int Height = Blobs[iBlob].Ymax - Blobs[iBlob].Ymin + 1;
                int Xmin = Blobs[iBlob].Xmin;
                int Ymin = Blobs[iBlob].Ymin;
                Blobs[iBlob].BW_Image = new bool[Height, Width];
                for (int i = Blobs[iBlob].Ymin; i <= Blobs[iBlob].Ymax; i++)
                {
                    for (int j = Blobs[iBlob].Xmin; j <= Blobs[iBlob].Xmax; j++)
                    {
                        if (AssignedBlob[i, j] == Blobs[iBlob].BlobNumber)
                        {
                            Blobs[iBlob].BW_Image[i - Ymin, j - Xmin] = true;
                        }
                    }
                }
            }       
        }

        static public bool Overlap(int x1, int x2, int xp1, int xp2)
        {
            if (xp1 > x2 || x1 > xp2)
            {
                return false;
            }
            return true;
        }

        private void FixBlobs() //
        {
            bool PassFail = true;
            int RemoveNumber = 0;
            int ReplaceNumber = 0;
            for (int iCol = this.AssignedBlob.GetLength(1) - 1; iCol > 0; iCol--)
            {
                for (int iRow = 1; iRow < this.AssignedBlob.GetLength(0); iRow++)
                {
                    int ValLeft = this.AssignedBlob[iRow, iCol - 1];
                    int ValRight = this.AssignedBlob[iRow, iCol];
                    if ((ValLeft != 0) && (ValRight != 0) && (ValRight != ValLeft)) // failure
                    {
                        int TempRemoveNumber = Math.Max(ValRight, ValLeft); // 
                        int TempReplaceNumber = Math.Min(ValRight, ValLeft);
                        if (TempReplaceNumber != ReplaceNumber || TempRemoveNumber != RemoveNumber) // if replacement made dont doit over again
                        {
                            RemoveNumber = TempRemoveNumber;
                            ReplaceNumber = TempReplaceNumber;
                            this.ReplaceBlobs(RemoveNumber, ReplaceNumber); // do repair on blob
                            PassFail = false;
                        }
                    }
                }
            }
            if (PassFail == false)
            {
                this.Assign_Blobs();
                this.FixBlobs(); // now call the algorithm again (uses recursion to fix until not broken)
            }
            return;
        }

        private void ReplaceBlobs(int RemoveNumber, int ReplaceNumber)
        {
            // "RemoveNumber" is the blob number that will be replaced with "ReplaceNumber"
            int[] ReplaceIndices = this.Blobs.Select((b, i) => b.BlobNumber == RemoveNumber ? i : -1).Where(i => i != -1).ToArray();
            for (int iReplace = 0; iReplace < ReplaceIndices.Length; iReplace++)
            {
                this.Blobs[ReplaceIndices[iReplace]].BlobNumber = ReplaceNumber;
            }
        }

        public void DrawBlobOutline(Image HostImage)
        {
            Graphics myGraphics = Graphics.FromImage(HostImage);
            Rectangle[] myRectangles = new Rectangle[Blobs.Length];
            for (int i = 0; i < Blobs.Length; i++)
			{
                int x = Blobs[i].Xmin;
                int y = Blobs[i].Ymin;
                int width = Blobs[i].Xmax - Blobs[i].Xmin;
                int height = Blobs[i].Ymax - Blobs[i].Ymin;
                myRectangles[i] = new Rectangle(x, y, width, height);
			}
            Pen myPen = new Pen(Color.Red);
            myGraphics.DrawRectangles(myPen, myRectangles);
        }
        
        public void RemoveSmallBlobs(int minPixelCount)
        {
            Blobs = Array.FindAll(Blobs, (Blob x) => x.PixelCount > minPixelCount);
            this.BlobCount = Blobs.Length;
        }

    }

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
