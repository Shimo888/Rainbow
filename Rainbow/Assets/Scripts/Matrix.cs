using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Matrix3
{
    private float[,] _matrix;

    public Matrix3(float v11, float v12, float v13, float v21, float v22, float v23, float v31, float v32, float v33)
    {
        _matrix = new float[3, 3];
        _matrix[0, 0] = v11;
        _matrix[0, 1] = v12;
        _matrix[0, 2] = v13;
        _matrix[1, 0] = v21;
        _matrix[1, 1] = v22;
        _matrix[1, 2] = v23;
        _matrix[2, 0] = v31;
        _matrix[2, 1] = v32;
        _matrix[2, 2] = v33;
    }

    public float this[int x,int y]
    {
        get
        {
            return _matrix[x, y];
        }
        set
        {
            _matrix[x, y] = value;
        }
    }

    public static Vector3 operator *(Matrix3 matrix,Vector3 vector)
    {
        float x = matrix[0, 0] * vector[0] + matrix[0, 1] * vector[1] + matrix[0, 2] * vector[2];
        float y = matrix[1, 0] * vector[0] + matrix[1, 1] * vector[1] + matrix[1, 2] * vector[2];
        float z = matrix[2, 0] * vector[0] + matrix[2, 1] * vector[1] + matrix[2, 2] * vector[2];
        return new Vector3(x, y, z);
    }
}