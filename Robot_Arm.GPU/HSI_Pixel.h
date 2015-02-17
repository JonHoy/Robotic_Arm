
#include <amp_graphics.h>
#include <amp_math.h>
#include <limits>

#define PI 3.14159f

using namespace concurrency::fast_math;
using namespace concurrency::graphics;

struct HSI_Pixel {
	float H;
	float S;
	float I;
};

inline HSI_Pixel RGB2HSI(float R, float G, float B) restrict (amp, cpu)
{
	HSI_Pixel Pixel;
	
	R = R/255.0f;
	G = G/255.0f;
	B = B/255.0f;

	Pixel.I = (R + G + B)/3;
	float MinColor = min(min(R, G),B);

	if (Pixel.I == 0)
		Pixel.S = 0;
	else
		Pixel.S = 1 - MinColor / Pixel.I;

	float Theta;
	
	if (R == 0 && G == 0 && B == 0)
		Theta = 0;
	else
		Theta = acosf((R - G/2 - B/2) / sqrtf(R*R + G*G + B*B - R*G - R*B - G*B));
	if (B <= G)
		Pixel.H = Theta / (PI);
	else
		Pixel.H = (2 * PI - Theta)/ (PI);
	return Pixel;
}

// determine the distance between two HSI pixels 
inline float HSI_Distance(HSI_Pixel Pixel1, HSI_Pixel Pixel2) restrict (amp, cpu)
{
	// let distance be weighted more towards hue and saturation than intensity
	float DistS = fabsf(Pixel1.S - Pixel2.S);
	float DistI = fabsf(Pixel1.I - Pixel2.I);
	float DistH_1 = fabsf(Pixel1.H - Pixel2.H);
	float DistH_2 = 2.0f - DistH_1;
	float DistH = min(DistH_1, DistH_2);
	float distance = DistH * DistH + DistI * DistI *DistI * DistI + DistS * DistS;
	return distance;
}