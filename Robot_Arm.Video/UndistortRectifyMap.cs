using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;

namespace Robot_Arm.Video
{
    public partial class StereoSystem
    {
        public static void UndistortRectifyMap(int nr, int nc, double[,] R, double[,] KK, double[,] f, double[,] c, double[,] k, out Matrix<float> mapx, out Matrix<float> mapy)
        {
            /*
            Adapted from MATLAB calibration Toolbox
            [nr,nc] = size(I);
            Irec = 255*ones(nr,nc);
            [mx,my] = meshgrid(1:nc, 1:nr);
            px = reshape(mx',nc*nr,1);
            py = reshape(my',nc*nr,1);
            rays = inv(KK_new)*[(px - 1)';(py - 1)';ones(1,length(px))]; 
            % Rotation: (or affine transformation):
            rays2 = R'*rays;
            x = [rays2(1,:)./rays2(3,:);rays2(2,:)./rays2(3,:)];
            % Add distortion:
            xd = apply_distortion(x,k);
            */
            var pmatrix = new double[3, nr * nc];
            int idx = 0; 
            for (int i = 0; i < nr; i++)
            {
                for (int j = 0; j < nc; j++)
                {
                    pmatrix[0, idx] = j;
                    pmatrix[1, idx] = i;
                    pmatrix[2, idx] = 1;
                    idx++;
                }
            }
            double[,] KKinv = KK.Clone() as double[,];
            double[,] Rinv = R.Clone() as double[,];
            int info;
            alglib.matinvreport report;
            alglib.rmatrixinverse(ref KKinv, out info, out report);
            alglib.rmatrixinverse(ref Rinv, out info, out report);

            var rays = Multiply(KKinv, pmatrix);
            var rays2 = Multiply(Rinv, rays);
            var x = new double[2, nr * nc];
            var xinput = new double[2];
            var knew = new double[5] { k[0, 0], k[1, 0], k[2, 0], k[3, 0], 0 }; 
            for (int i = 0; i < x.GetLength(1); i++)
            {
                double dx, dy;
                xinput[0] = rays2[0, i] / rays2[2, i];
                xinput[1] = rays2[1, i] / rays2[2, i];
                Undistort(xinput, knew, out dx, out dy);
                x[0, i] = dx;
                x[1, i] = dy;
            }
            /*
            % Reconvert in pixels:
            px2 = f(1)*(xd(1,:)+alpha*xd(2,:))+c(1);
            py2 = f(2)*xd(2,:)+c(2);
            % Interpolate between the closest pixels:
            px_0 = floor(px2);
            py_0 = floor(py2);
             */
            mapx = new Matrix<float>(nr, nc);
            mapy = new Matrix<float>(nr, nc);
            idx = 0;
            for (int i = 0; i < nr; i++)
            {
                for (int j = 0; j < nc; j++)
                {
                    mapx[i,j] = (float)(f[0,0]*x[0,idx]+c[0,0]);
                    mapy[i, j] = (float)(f[1,0] * x[1, idx] + c[1,0]);
                    idx++;
                }
            }
        }
       
        private static double[,] Multiply(double[,] A, double[,] B)
        {
            if (A.GetLength(1) != B.GetLength(0))
                throw new Exception("A must be a n x m size matrix and B must be an m by p size matrix");
            int n = A.GetLength(0);
            int m = A.GetLength(1);
            int p = B.GetLength(1);
            var C = new double[n, p];
            for (int i = 0; i < n; i++)
			{
			    for (int j = 0; j < p; j++)
			    {
                    double sum = 0;
                    for (int k = 0; k < m; k++)
                    {
                        sum = sum + A[i, k] * B[k, j];
                    }
                    C[i, j] = sum;
			    }
			}
            return C;
        }
        private static void Undistort(double[] x, double[] k, out double dx, out double dy)
        {
            /*
            [m,n] = size(x);
            % Add distortion:
            r2 = x(1,:).^2 + x(2,:).^2;
            r4 = r2.^2;
            r6 = r2.^3;
            % Radial distortion:
            cdist = 1 + k(1) * r2 + k(2) * r4 + k(5) * r6;
            xd1 = x .* (ones(2,1)*cdist);
            % tangential distortion:
            a1 = 2.*x(1,:).*x(2,:);
            a2 = r2 + 2*x(1,:).^2;
            a3 = r2 + 2*x(2,:).^2;
            delta_x = [k(3)*a1 + k(4)*a2 ; k(3) * a3 + k(4)*a1];
            */
            double r2 = x[0] * x[0] + x[1] * x[1];
            double r4 = r2 * r2;
            double r6 = r4 * r2;
            double cdist = 1 + k[0] * r2 + k[1] * r4 + k[4] * r6;
            double[] xd1 = {x[0] * cdist, x[1] * cdist};
            double a1 = 2 * x[0] * x[1];
            double a2 = r2 + 2 * x[0] * x[0];
            double a3 = r2 + 2 * x[1] * x[1];
            double[] delta_x = { k[2] * a1 + k[3] * a2, k[2] * a3 + k[3] * a1 };
            dx = delta_x[0] + xd1[0];
            dy = delta_x[1] + xd1[1];
        }
    }
}
