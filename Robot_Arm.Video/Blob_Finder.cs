using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Robot_Arm.Video
{
    public class BlobFinder
    {
        public bool[,] BW {get; private set;}
        private int BlobCount;
        public Blob[] Blobs {get; private set;}
        public int[,] AssignedBlob {get; private set;}

        public BlobFinder(Image<Gray, Byte> GrayFrame)
        {
            BW = BW_Converter(GrayFrame);
            makeBlobs();
        }

        public static bool[,] BW_Converter(Image<Gray, Byte> GrayFrame)
        {
            byte[, ,] ByteData = GrayFrame.Data;
            int Rows = ByteData.GetLength(0);
            int Cols = ByteData.GetLength(1);
            bool[,] BW_Out = new bool[Rows, Cols];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (ByteData[i, j, 0] == 255)
                    {
                        BW_Out[i, j] = true;
                    }
                }
            }
            return BW_Out;
        }

        public static Image<Gray, Byte> Gray_Converter(ref bool[,] BW_In)
        {
            int Rows = BW_In.GetLength(0);
            int Cols = BW_In.GetLength(1);
            byte[, ,] FrameData = new byte[Rows, Cols, 1];
            
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (BW_In[i,j])
                    {
                        FrameData[i, j, 0] = 255;
                    }
                }
            }
            Image<Gray, Byte> GrayFrame_Out = new Image<Gray, byte>(FrameData);
            return GrayFrame_Out;
        }

        public BlobFinder(bool[,] BW)
        {
            this.BW = BW;
            makeBlobs();
        }
        // take a BW image and generate an array of blob objects from it
        private void makeBlobs()
        {
            // Turns a Black and White Image to a bunch of blobs
            Get_1D_Blobs(); // turn the image to 1d blobs
            MatchBlobs(); // start lining up the 1d blobs and if they connect give them the same blob#
            Assign_Blobs();  // make an image that has
            FixBlobs(); // check to make sure the algorithm worked completely (if not, recursively use it until it does)
            ConsolidateBlobs(); // merge all the blob objects with the same blob number
        }
        // create 1D line blob objects
        private void Get_1D_Blobs()
        {
            this.Blobs = new Blob[BW.Length];
            BlobCount = 0;
            Blobs[0] = new Blob();
            int Cols = BW.GetLength(1);
            int Rows = BW.GetLength(0);
            for (int j = 0; j < Cols; j++)
            {
                for (int i = 0; i < Rows; i++)
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
        // assign the blob number on image where a blob is present
        private void Assign_Blobs() {
            this.AssignedBlob = new int[BW.GetLength(0), BW.GetLength(1)];
            int NumBlobs = Blobs.Length;
            for (int iBlob = 0; iBlob < NumBlobs; iBlob++)
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
        // match up connected 1d blobs and assign them the same blob number
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
        // remove redundant blobs so that only unique blob objects remain
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
        // helper function to determine if two line blobs are touching each other
        static public bool Overlap(int x1, int x2, int xp1, int xp2)
        {
            if (xp1 > x2 || x1 > xp2)
            {
                return false;
            }
            return true;
        }
        // subroutine used to iteratively correct any mistakes made by the blob matchign algorithm
        private void FixBlobs() //
        {
            bool PassFail = true;
            int RemoveNumber = 0;
            int ReplaceNumber = 0;
            int ColStart = this.AssignedBlob.GetLength(1) - 1;
            int Rows = this.AssignedBlob.GetLength(0);
            for (int iCol = ColStart; iCol > 0; iCol--)
            {
                for (int iRow = 1; iRow < Rows; iRow++)
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
        // replaces blobs with the remove number with the replace number
        private void ReplaceBlobs(int RemoveNumber, int ReplaceNumber)
        {
            // "RemoveNumber" is the blob number that will be replaced with "ReplaceNumber"
            int[] ReplaceIndices = this.Blobs.Select((b, i) => b.BlobNumber == RemoveNumber ? i : -1).Where(i => i != -1).ToArray();
            for (int iReplace = 0; iReplace < ReplaceIndices.Length; iReplace++)
            {
                this.Blobs[ReplaceIndices[iReplace]].BlobNumber = ReplaceNumber;
            }
        }
        // draw the bounding box of the blob objects onto the host image
        public void DrawBlobOutline(Image HostImage)
        {
            Graphics myGraphics = Graphics.FromImage(HostImage);
            var myRectangles = GetRectangles();
            Pen myPen = new Pen(Color.Red);
            if (myRectangles.Length > 0)
            {
                myGraphics.DrawRectangles(myPen, myRectangles);    
            }
        }

        public static void DrawBlobOutline(Image HostImage, Rectangle Box)
        {
            Graphics myGraphics = Graphics.FromImage(HostImage);
            var myRectangles = new Rectangle[] { Box };
            Pen myPen = new Pen(Color.Red);
            if (Box.IsEmpty != true)
            {
                myGraphics.DrawRectangles(myPen, myRectangles);
            }
        }

        private Rectangle[] GetRectangles()
        {
            Rectangle[] myRectangles = new Rectangle[Blobs.Length];
            for (int i = 0; i < Blobs.Length; i++)
            {
                int x = Blobs[i].Xmin;
                int y = Blobs[i].Ymin;
                int width = Blobs[i].Xmax - Blobs[i].Xmin;
                int height = Blobs[i].Ymax - Blobs[i].Ymin;
                myRectangles[i] = new Rectangle(x, y, width, height);
            }
            return myRectangles;
        }

        public Blob PickBestBlob()
        {
            double minXCoverage = .03; // define min percentage coverage a blob can have not not be rejected
            double maxXCoverage = .2; // define maximum percent coverage a blob can have and not be rejected 
            double maxPercentCoverage = .1;

            try
            {
                var rectBounds = GetRectangles();
                double frameWidth = (double)BW.GetLength(1);
                double frameHeight = (double)BW.GetLength(0);
                var FiltBlobs = Array.FindAll(Blobs, (x) => minXCoverage < (double)((double)(x.Xmax - x.Xmin) / frameWidth));
                FiltBlobs = Array.FindAll(FiltBlobs, (x) => maxXCoverage > (double)((double)(x.Xmax - x.Xmin) / frameWidth));
                FiltBlobs = Array.FindAll(FiltBlobs, (x) => maxPercentCoverage > (double)x.PixelCount / (frameWidth * frameHeight));
                double max = FiltBlobs.Max(t => t.PixelCount);
                int index = Array.FindIndex(FiltBlobs, t => t.PixelCount == max);
                var BestBlob = FiltBlobs[index];
                return BestBlob;
            }
            catch (Exception)
            {
                return null;
            }



        }

        // remove blobs with a given criteria
        public void RemoveSmallBlobs(int minPixelCount)
        {
            Blobs = Array.FindAll(Blobs, (Blob x) => x.PixelCount > minPixelCount);
            this.BlobCount = Blobs.Length;
        }
        // fill in the blob bounding box with true values
        public bool [,] FillBlobBoundingBox()
        {
            bool [,] BW_Filled = new bool[BW.GetLength(0),BW.GetLength(1)];
            for (int iBlob = 0; iBlob < Blobs.Length; iBlob++)
            {
                int Xmin = Blobs[iBlob].Xmin;
                int Ymin = Blobs[iBlob].Ymin;
                int Xmax = Blobs[iBlob].Xmax;
                int Ymax = Blobs[iBlob].Ymax;
                for (int i = Ymin; i <= Ymax ; i++)
                {
                    for (int j = Xmin; j <= Xmax; j++)
                    {
                        BW_Filled[i, j] = true;
                    }
                }
            }
            return BW_Filled;
        }

        // takes the AND operation of two input BW images
        public static bool[,] AND(ref bool[,] BW_1, ref bool[,] BW_2)
        {
            
            if ((BW_1.GetLength(0) != BW_2.GetLength(0))
                || (BW_1.GetLength(1) != BW_2.GetLength(1))
                )
            {
                throw new Exception("BW_1 and BW_2 have to be the same size");
            }
            int Rows = BW_1.GetLength(0);
            int Cols = BW_1.GetLength(1);
            bool[,] BW_Out = new bool[Rows, Cols];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (BW_1[i,j] && BW_2[i,j])
                    {
                        BW_Out[i, j] = true;
                    }
                }
            }
            return BW_Out;
        }

    }
}
