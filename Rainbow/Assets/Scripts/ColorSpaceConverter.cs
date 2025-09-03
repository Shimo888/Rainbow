using System.Collections.Generic;
using UnityEngine;

public static class ColorSpaceConverter  
{
    public static Vector3 WaveLengthToXYZ(float waveLength)
    {
        return new Vector3(
            1.056f * PiecewiseGaussianFunction(waveLength, 599.8f, 37.9f,31.0f) + 0.362f * PiecewiseGaussianFunction(waveLength, 442.0f, 16.0f,26.7f) - 0.065f * PiecewiseGaussianFunction(waveLength, 501.1f, 20.4f,26.2f),
            0.821f * PiecewiseGaussianFunction(waveLength, 568.8f, 46.9f,40.5f) + 0.286f * PiecewiseGaussianFunction(waveLength, 530.9f, 16.3f,31.1f),
            1.217f * PiecewiseGaussianFunction(waveLength, 437.0f, 11.8f,36.0f) + 0.681f * PiecewiseGaussianFunction(waveLength, 450.0f, 26.0f,13.8f) 
            );
    }

    public static Vector3 XYZToRGB(Vector3 xyz)
    {
        var conversionMatrix = new Matrix3(2.36461385f, -0.89654067f, -0.46807328f, -0.51516621f, 1.4264081f,
            0.0887581f, 0.0052037f, -0.01440816f, 1.00920446f);
        return conversionMatrix * xyz;
    }

    public static Vector3 WaveLengthToRGB(float waveLength)
    {
        return XYZToRGB(WaveLengthToXYZ(waveLength));
    }
    
    private static float PiecewiseGaussianFunction(float x, float mu, float sigma1, float sigma2)
    {
        if (x < mu)
        {
            return Mathf.Exp(-0.5f * Mathf.Pow(x - mu, 2.0f)/(sigma1*sigma1));
        }
        else
        {
            return Mathf.Exp(-0.5f * Mathf.Pow(x - mu, 2.0f)/(sigma2*sigma2));
        }
    }
}
