using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rainbow : MonoBehaviour
{
    [SerializeField] private int pixelSizeX;
    [SerializeField] private int pixelSizeY;
    [SerializeField] private Light light;
    private Material _rainbowMaterial;
    private Texture2D _rainbowTexture;
    private Vector3[,] _pixelToPosition;
    void Start()
    {
        //マテリアルの取得
        _rainbowMaterial = GetComponent<MeshRenderer>().material;
        
        //テクスチャの作成とマテリアルへの登録
        _rainbowTexture = new Texture2D(pixelSizeX,pixelSizeY);
        _rainbowMaterial.mainTexture = _rainbowTexture;

        //ピクセル座標をグローバル座標に変換するリストの作成
        _pixelToPosition = CalcPixelToPosition();
    }

    // Update is called once per frame
    void Update()
    { 
        SetRainbowColorToPixel(_pixelToPosition); 
    }

    private Vector3[,] CalcPixelToPosition()
    {
        var rotation = transform.rotation;
        Vector3 baseVectorX = rotation * Vector3.left;
        Vector3 baseVectorY = rotation * Vector3.back;
        float dx = transform.localScale.x / pixelSizeX;
        float dy = transform.localScale.z / pixelSizeY;

        var pixelToPosition = new Vector3[pixelSizeX,pixelSizeY];
        var originPosition = transform.position + (- 0.5f * transform.localScale.x + 0.5f * dx) * baseVectorX + 
                             (- 0.5f * transform.localScale.y + 0.5f * dy) * baseVectorY ;
        for (var x = 0; x < pixelSizeX; x++)
        {
            for (var y = 0; y < pixelSizeY; y++)
            {
                pixelToPosition[x, y] = originPosition + dx * x * baseVectorX + dy * y * baseVectorY;
            }
        }
        return pixelToPosition;
    }

    //光が水滴(グリッドの拡張点に差し込む方向)と、水滴とカメラのベクトルがなす角度を求める。
    private void SetRainbowColorToPixel(Vector3[,] pixelToPosition)
    {
        var lightDirection = light.transform.forward;
        for (var x = 0; x < pixelSizeX; x++)
        {
            for (var y = 0; y < pixelSizeY; y++)
            {
                //光が水滴(グリッドの拡張点に差し込む方向)と、水滴とカメラのベクトルがなす角度を求める。
                var cameraToPixelDirection = (pixelToPosition[x, y] - Camera.main.transform.position).normalized;
                var rainbowAngle = 180.0f * Mathf.Acos(Vector3.Dot(lightDirection, cameraToPixelDirection))/Mathf.PI;
                //角度を波長に線型補完
                var waveLength = Fit(rainbowAngle, 40.4f, 42.3f, 400f, 700f);
                var color = new Color();
                    var rgb = ColorSpaceConverter.WaveLengthToRGB(waveLength);
                    color = new Color(rgb.x, rgb.y, rgb.z, 0);
                _rainbowTexture.SetPixel(x, y, color);
            }
        }
        _rainbowTexture.Apply();
    }
    public static float Fit(float value, float oMin, float oMax, float nMin, float nMax)
    {
        if (value < oMin) return nMin;
        if (value > oMax) return nMax;
        return nMin + (value - oMin) * (nMax - nMin) / (oMax - oMin);
    }
}