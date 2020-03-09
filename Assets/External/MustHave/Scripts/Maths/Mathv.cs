﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MustHave
{
    public struct Mathv
    {
        public static Vector3 Random(Vector3 min, Vector3 max)
        {
            return new Vector3(
                UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y),
                UnityEngine.Random.Range(min.z, max.z)
            );
        }

        public static Vector3 Random(float min, float max)
        {
            return new Vector3(
                UnityEngine.Random.Range(min, max),
                UnityEngine.Random.Range(min, max),
                UnityEngine.Random.Range(min, max)
            );
        }

        public static Vector3 Abs(Vector3 v)
        {
            return new Vector3(
                Mathf.Abs(v.x),
                Mathf.Abs(v.y),
                Mathf.Abs(v.z)
            );
        }

        public static Vector2Int Abs(Vector2Int v)
        {
            return new Vector2Int(
                Mathf.Abs(v.x),
                Mathf.Abs(v.y)
            );
        }

        public static Vector3 Mul(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.x * v2.x,
                v1.y * v2.y,
                v1.z * v2.z
            );
        }

        public static Vector3 Div(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                v1.x / v2.x,
                v1.y / v2.y,
                v1.z / v2.z
            );
        }

        public static Vector3 Sin(Vector3 v)
        {
            return new Vector3(
                Mathf.Sin(v.x),
                Mathf.Sin(v.y),
                Mathf.Sin(v.z)
            );
        }

        public static Vector3 Min(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                Mathf.Min(v1.x, v2.x),
                Mathf.Min(v1.y, v2.y),
                Mathf.Min(v1.z, v2.z)
            );
        }

        public static Vector3 Max(Vector3 v1, Vector3 v2)
        {
            return new Vector3(
                Mathf.Max(v1.x, v2.x),
                Mathf.Max(v1.y, v2.y),
                Mathf.Max(v1.z, v2.z)
            );
        }

        public static Vector2Int Min(Vector2Int v1, Vector2Int v2)
        {
            return new Vector2Int(
                Mathf.Min(v1.x, v2.x),
                Mathf.Min(v1.y, v2.y)
            );
        }

        public static Vector2Int Max(Vector2Int v1, Vector2Int v2)
        {
            return new Vector2Int(
                Mathf.Max(v1.x, v2.x),
                Mathf.Max(v1.y, v2.y)
            );
        }

        public static Vector3Int Min(Vector3Int v1, Vector3Int v2)
        {
            return new Vector3Int(
                Mathf.Min(v1.x, v2.x),
                Mathf.Min(v1.y, v2.y),
                Mathf.Min(v1.z, v2.z)
            );
        }

        public static Vector3Int Max(Vector3Int v1, Vector3Int v2)
        {
            return new Vector3Int(
                Mathf.Max(v1.x, v2.x),
                Mathf.Max(v1.y, v2.y),
                Mathf.Max(v1.z, v2.z)
            );
        }

        public static Vector3 Clamp(Vector3 v, float min, float max)
        {
            return new Vector3(
                Mathf.Clamp(v.x, min, max),
                Mathf.Clamp(v.y, min, max),
                Mathf.Clamp(v.z, min, max)
            );
        }

        public static Vector3 Clamp(Vector3 v, Vector3 min, Vector3 max)
        {
            return new Vector3(
                Mathf.Clamp(v.x, min.x, max.x),
                Mathf.Clamp(v.y, min.y, max.y),
                Mathf.Clamp(v.z, min.z, max.z)
            );
        }

        public static Vector3Int RoundToInt(Vector3 v)
        {
            return new Vector3Int(
                Mathf.RoundToInt(v.x),
                Mathf.RoundToInt(v.y),
                Mathf.RoundToInt(v.z)
            );
        }
    }
}
