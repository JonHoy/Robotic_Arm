//  This is how an ARGB pixel is stored in CPU memory. Compatible with the memory layout used by BitmapData.
//  For more information see: http://en.wikipedia.org/wiki/RGBA_color_space

#pragma once

struct RgbPixel 
{
    float r;
    float g;
    float b;
	float a;

	void UnpackPixel(const int packedArgb) restrict(amp, cpu) 
	{
		b = (float) (packedArgb & 0xFF);
		g = (float) ((packedArgb & 0xFF00) >> 8);
		r = (float) ((packedArgb & 0xFF0000) >> 16);
	}

};
