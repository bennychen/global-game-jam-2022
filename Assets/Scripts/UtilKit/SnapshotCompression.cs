using System.Collections;
using UnityEngine;

public static class CompressionHelper
{
    public static BitArray WriteInt(this BitArray bits, int startIndex, int number, int numOfDigits)
    {
        for (int i = startIndex; i < startIndex + numOfDigits; i++)
        {
            bits[i] = (number & 1) == 1;
            number = number >> 1;
        }
        return bits;
    }

    public static int ReadInt(this BitArray bits, int startIndex, int numOfDigits)
    {
        int numeral = 0;
        for (int i = startIndex; i < startIndex + numOfDigits; i++)
        {
            int n = i - startIndex;
            if (bits[i])
            {
                numeral = numeral | (1 << n);
            }
        }
        return numeral;
    }

    public static byte[] WriteInt(byte[] bytes, int startIndex, int number, int numOfDigits)
    {
        for (int i = startIndex; i < startIndex + numOfDigits; i++)
        {
            int byteIndex = i / 8;
            int bitInByteIndex = i % 8;

            bytes[byteIndex] = SetBit(bytes[byteIndex], bitInByteIndex, (number & 1) == 1);
            //Debug.Log("Write::byte[" + byteIndex + "][" + bitInByteIndex + "] is " + ((number & 1) == 1));
            number = number >> 1;
        }
        return bytes;
    }

    public static int ReadInt(byte[] bytes, int startIndex, int numOfDigits)
    {
        int numeral = 0;
        for (int i = startIndex; i < startIndex + numOfDigits; i++)
        {
            int byteIndex = i / 8;
            int bitInByteIndex = i % 8;

            bool bit = GetBit(bytes[byteIndex], bitInByteIndex);
            //Debug.Log("Read::byte[" + byteIndex + "][" + bitInByteIndex + "] is " + bit);
            if (bit)
            {
                int n = i - startIndex;
                numeral = numeral | (1 << n);
            }
        }
        return numeral;
    }

    public static byte SetBit(byte oneByte, int bitIndex, bool bit)
    {
        byte mask = (byte)(1 << bitIndex);
        if (bit)
        {
            oneByte |= mask;
        }
        else
        {
            oneByte &= (byte)(~mask);
        }
        return oneByte;
    }

    public static bool GetBit(byte oneByte, int bitIndex)
    {
        return (oneByte & 1 << bitIndex) != 0;
    }
}

// Algorithm 'Bound & Quantize' is based on http://gafferongames.com/networked-physics/snapshot-compression/
public class FloatCompression
{
    public FloatCompression(int integerBound, int fractionBitCount)
        : this(-integerBound, integerBound, fractionBitCount)
    {
    }

    public FloatCompression(int minValue, int maxValue, int fractionBitCount)
    {
        _minInteger = minValue;
        _maxInteger = maxValue;
        _intBitCount = GetBitsRequired(minValue, maxValue);
        _fractionBitCount = fractionBitCount;
        _intMaxValue = (1 << _intBitCount) - 1;
        _fractionScale = (1 << fractionBitCount) - 1;
        _inverseFractionScale = 1.0f / _fractionScale;
    }

    public int GetBitCount()
    {
        return _intBitCount + _fractionBitCount;
    }

    public byte[] CompressAndWrite(byte[] bytes, int startIndex, float value)
    {
        int compressed = Compress(value);
        return CompressionHelper.WriteInt(bytes, startIndex, compressed, GetBitCount());
    }

    public BitArray CompressAndWrite(BitArray bits, int startIndex, float value)
    {
        int compressed = Compress(value);
        bits.WriteInt(startIndex, compressed, GetBitCount());
        return bits;
    }

    public float ReadAndDecompress(byte[] bytes, int startIndex)
    {
        int compressed = CompressionHelper.ReadInt(bytes, startIndex, GetBitCount());
        return Decompress(compressed);
    }

    public float ReadAndDecompress(BitArray bits, int startIndex)
    {
        int compressed = bits.ReadInt(startIndex, GetBitCount());
        return Decompress(compressed);
    }

    public int Compress(float value)
    {
        UnityEngine.Assertions.Assert.IsTrue(value >= _minInteger && value <= _maxInteger);
        float relativeValue = value - _minInteger;
        relativeValue = Mathf.Clamp(relativeValue, 0, _intMaxValue);
        int integer = Mathf.FloorToInt(relativeValue) & _intMaxValue;
        float fraction = relativeValue - integer;

        int integerPart = integer;
        int fractionPart = Mathf.RoundToInt(fraction * _fractionScale);
        return fractionPart << _intBitCount | integerPart;
    }

    public float Decompress(int value)
    {
        int integerPart = value & ((1 << _intBitCount) - 1);
        int fractionPart = value >> _intBitCount;
        float fraction = fractionPart * _inverseFractionScale;
        return integerPart + fraction + _minInteger;
    }

    private int GetBitsRequired(int min, int max)
    {
        return (min == max) ? 0 : Log2(max - min) + 1;
    }

    private int Log2(int x)
    {
        int a = x | (x >> 1);
        int b = a | (a >> 2);
        int c = b | (b >> 4);
        int d = c | (c >> 8);
        int e = d | (d >> 16);
        int f = e >> 1;
        return PopCount(f);
    }

    private int PopCount(int x)
    {
        int a = x - ((x >> 1) & 0x55555555);
        int b = (((a >> 2) & 0x33333333) + (a & 0x33333333));
        int c = (((b >> 4) + b) & 0x0f0f0f0f);
        int d = c + (c >> 8);
        int e = d + (d >> 16);
        return e & 0x0000003f;
    }

    private int _minInteger;
    private int _maxInteger;
    private int _intBitCount;
    private int _fractionBitCount;
    private int _intMaxValue;
    private float _fractionScale;
    private float _inverseFractionScale;
}

// Compressed quaternion takes 29(9+9+9+2) bits in total
// Algorithm 'Smallest Three' is based on http://gafferongames.com/networked-physics/snapshot-compression/
public static class QuaternionCompression
{
    public const int OrientationBits = 9;
    public const int TotalBits = OrientationBits * 3 + 2;
    public const int MaxValue = (1 << OrientationBits) - 1;
    public const float minimum = -1.0f / 1.414214f; // 1.0f / sqrt(2)
    public const float maximum = +1.0f / 1.414214f;
    public const float scale = (float)MaxValue;
    public const float inverse_scale = 1.0f / scale;

    public static BitArray CompressAndWrite(BitArray bits, int startIndex, Quaternion quaternion)
    {
        Compress(quaternion);

        bits.WriteInt(startIndex, integer_a, OrientationBits);
        startIndex += OrientationBits;
        bits.WriteInt(startIndex, integer_b, OrientationBits);
        startIndex += OrientationBits;
        bits.WriteInt(startIndex, integer_c, OrientationBits);
        startIndex += OrientationBits;
        bits.WriteInt(startIndex, largestIndex, 2);
        return bits;
    }

    public static byte[] CompressAndWrite(byte[] bytes, int startIndex, Quaternion quaternion)
    {
        Compress(quaternion);

        bytes = CompressionHelper.WriteInt(bytes, startIndex, integer_a, OrientationBits);
        startIndex += OrientationBits;
        bytes = CompressionHelper.WriteInt(bytes, startIndex, integer_b, OrientationBits);
        startIndex += OrientationBits;
        bytes = CompressionHelper.WriteInt(bytes, startIndex, integer_c, OrientationBits);
        startIndex += OrientationBits;
        bytes = CompressionHelper.WriteInt(bytes, startIndex, largestIndex, 2);
        return bytes;
    }

    public static Quaternion ReadAndDecompress(BitArray bits, int startIndex)
    {
        integer_a = bits.ReadInt(startIndex, OrientationBits);
        startIndex += OrientationBits;
        integer_b = bits.ReadInt(startIndex, OrientationBits);
        startIndex += OrientationBits;
        integer_c = bits.ReadInt(startIndex, OrientationBits);
        startIndex += OrientationBits;
        largestIndex = bits.ReadInt(startIndex, 2);

        return Decompress();
    }

    public static Quaternion ReadAndDecompress(byte[] bytes, int startIndex)
    {
        integer_a = CompressionHelper.ReadInt(bytes, startIndex, OrientationBits);
        startIndex += OrientationBits;
        integer_b = CompressionHelper.ReadInt(bytes, startIndex, OrientationBits);
        startIndex += OrientationBits;
        integer_c = CompressionHelper.ReadInt(bytes, startIndex, OrientationBits);
        startIndex += OrientationBits;
        largestIndex = CompressionHelper.ReadInt(bytes, startIndex, 2);

        return Decompress();
    }

    private static void Compress(Quaternion quaternion)
    {
        float x = quaternion.x;
        float y = quaternion.y;
        float z = quaternion.z;
        float w = quaternion.w;
        float abs_x = Mathf.Abs(quaternion.x);
        float abs_y = Mathf.Abs(quaternion.y);
        float abs_z = Mathf.Abs(quaternion.z);
        float abs_w = Mathf.Abs(quaternion.w);

        largestIndex = 0;
        float largest_value = abs_x;

        if (abs_y > largest_value)
        {
            largestIndex = 1;
            largest_value = abs_y;
        }

        if (abs_z > largest_value)
        {
            largestIndex = 2;
            largest_value = abs_z;
        }

        if (abs_w > largest_value)
        {
            largestIndex = 3;
            largest_value = abs_w;
        }

        float a = 0;
        float b = 0;
        float c = 0;

        switch (largestIndex)
        {
            case 0:
                if (x >= 0)
                {
                    a = y;
                    b = z;
                    c = w;
                }
                else
                {
                    a = -y;
                    b = -z;
                    c = -w;
                }
                break;

            case 1:
                if (y >= 0)
                {
                    a = x;
                    b = z;
                    c = w;
                }
                else
                {
                    a = -x;
                    b = -z;
                    c = -w;
                }
                break;

            case 2:
                if (z >= 0)
                {
                    a = x;
                    b = y;
                    c = w;
                }
                else
                {
                    a = -x;
                    b = -y;
                    c = -w;
                }
                break;

            case 3:
                if (w >= 0)
                {
                    a = x;
                    b = y;
                    c = z;
                }
                else
                {
                    a = -x;
                    b = -y;
                    c = -z;
                }
                break;

            default:
                Debug.LogError("Largest index is invalid, should never been here.");
                break;
        }

        float normal_a = (a - minimum) / (maximum - minimum);
        float normal_b = (b - minimum) / (maximum - minimum);
        float normal_c = (c - minimum) / (maximum - minimum);

        integer_a = Mathf.FloorToInt(normal_a * scale + 0.5f);
        integer_b = Mathf.FloorToInt(normal_b * scale + 0.5f);
        integer_c = Mathf.FloorToInt(normal_c * scale + 0.5f);
    }

    private static Quaternion Decompress()
    {
        float a = integer_a * inverse_scale * (maximum - minimum) + minimum;
        float b = integer_b * inverse_scale * (maximum - minimum) + minimum;
        float c = integer_c * inverse_scale * (maximum - minimum) + minimum;
        float sq = 1 - a * a - b * b - c * c;
        if (sq < 0) { sq = 0; }

        float x, y, z, w;

        switch (largestIndex)
        {
            case 0:
                {
                    x = Mathf.Sqrt(sq);
                    y = a;
                    z = b;
                    w = c;
                }
                break;

            case 1:
                {
                    x = a;
                    y = Mathf.Sqrt(sq);
                    z = b;
                    w = c;
                }
                break;

            case 2:
                {
                    x = a;
                    y = b;
                    z = Mathf.Sqrt(sq);
                    w = c;
                }
                break;

            case 3:
                {
                    x = a;
                    y = b;
                    z = c;
                    w = Mathf.Sqrt(sq);
                }
                break;

            default:
                {
                    x = 0;
                    y = 0;
                    z = 0;
                    w = 1;
                    break;
                }
        }

        return new Quaternion(x, y, z, w);
    }

    private static int largestIndex;
    private static int integer_a;
    private static int integer_b;
    private static int integer_c;
}


