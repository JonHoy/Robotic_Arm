using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Robot_Arm
{
    class ImageProcess
    {
        public ImageProcess(Bitmap myBitmap)
        {
            this.myBitmap = myBitmap;
            Rectangle rect = new Rectangle(0, 0, myBitmap.Width, myBitmap.Height);
            bmpData =
                myBitmap.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite,
                myBitmap.PixelFormat);
            Rows = bmpData.Height;
            Columns = bmpData.Width;
            Planes = bmpData.Stride / Columns;
            int elements = Rows * Columns * Planes;
            this.Buffer = new byte[elements];
            SelectedColor = new int[Rows * Columns];
            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, Buffer, 0, elements);
            Image = new int[elements];
            for (int i = 0; i < elements; i++)
                Image[i] = (int)Buffer[i];
        }
        public int[] Colors;
        public int[] Image;
        private byte[] Buffer;
        public int [] SelectedColor;
        unsafe public void Segment_Colors()
        {
            Colors = new int[] { 
                0, 0, 0,   
                255, 255, 255,    
                255, 0, 0,     
                0, 255, 0,     
                0, 0, 255,
                255, 255, 0,     
                0, 255, 255,     
                255, 0, 255,
                122, 122, 0,     
                0, 122, 122,     
                122, 0, 122,
                255, 122, 0,     
                0, 255, 122,     
                255, 0, 122,
                122, 255, 0,     
                0, 122, 255,     
                122, 0, 255,
                122, 0, 0,     
                0, 122, 0,     
                0, 0, 122};
            fixed(int* ImagePtr = &Image[0])
            fixed(int* ColorPtr = &Colors[0])
            fixed(int* SelectedColorPtr = &SelectedColor[0])
            { 
                EqualizeHistogram(ImagePtr, Image.Length);
                //SegmentColors(ImagePtr, ColorPtr, Colors.Length/Planes, Planes, Columns, Rows,  SelectedColorPtr);
            }
            for (int i = 0; i < Image.Length; i++)
                Buffer[i] = (byte)Image[i];
            
            System.Runtime.InteropServices.Marshal.Copy(Buffer, 0, bmpData.Scan0, Image.Length);
            // Unlock the bits.
            myBitmap.UnlockBits(bmpData);

            //this.myBitmap = new Bitmap(Columns, Rows, Planes * Columns, bmpData.PixelFormat, bmpData.Scan0);
        }
        
        public Bitmap myBitmap;
        private System.Drawing.Imaging.BitmapData bmpData;
        private int Planes, Rows, Columns;

        // import the c++ dlls
        [DllImport("ImageProcessingLibrary.dll", CallingConvention=CallingConvention.StdCall)]
        private static unsafe extern void SegmentColors(int* ImagePtr, int* ColorPtr, int ColorCount, int Planes, int Columns, int Rows, int* SelectedColorPtr);
        [DllImport("ImageProcessingLibrary.dll", CallingConvention = CallingConvention.StdCall)]
        private static unsafe extern void EqualizeHistogram(int* ImagePtr, int Image_Length);
    }
}
