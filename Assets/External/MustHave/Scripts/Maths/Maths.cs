﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MustHave
{
    public enum RotationOrder
    {
        XYZ, XZY, YXZ, YZX, ZXY, ZYX
    }

    public enum TransitionType
    {
        LINEAR,
        PARABOLIC_ACC,
        PARABOLIC_DEC,
        HYPERBOLIC_ACC,
        HYPERBOLIC_DEC,
        SIN_IN_PI2_RANGE,
        SIN_IN_PI_RANGE,
        SIN_IN_2PI_RANGE,
        COS_IN_PI2_RANGE,
        COS_IN_PI_RANGE,
        COS_IN_2PI_RANGE,
        ASYMETRIC_NORMALISED,
        ASYMETRIC_INFLECTED,
        SYMMETRIC_INFLECTED,
    };

    public struct Maths
    {
        public const float M_2PI = 2 * Mathf.PI;
        public const float M_PI2 = Mathf.PI / 2;
        public const float M_PI = Mathf.PI;

        public static float LerpInverse(float min, float max, float value)
        {
            float delta = max - min;
            return delta > float.MinValue ? (Mathf.Clamp(value, min, max) - min) / delta : 0f;
        }

        public static short ConvertFloatToShort(float value, float max)
        {
            value = Mathf.Clamp01(value / max) * short.MaxValue;
            return (short)value;
        }

        public static float ConvertShortToFloat(short value, float max)
        {
            return value * max / short.MaxValue;
        }

        public static float ConvertInchToCm(float inches)
        {
            return inches * 2.54f;
        }

        public static float ConvertCmToInch(float centimeters)
        {
            return centimeters / 2.54f;
        }

        public static Quaternion GetRotationWithOrder(Vector3 euler, RotationOrder order)
        {
            Quaternion qx, qy, qz;
            GetRotationComponents(out qx, out qy, out qz, euler);
            switch (order)
            {
                case RotationOrder.XYZ: return qx * qy * qz;
                case RotationOrder.XZY: return qx * qz * qy;
                case RotationOrder.YXZ: return qy * qx * qz;
                case RotationOrder.YZX: return qy * qz * qx;
                case RotationOrder.ZXY: return qz * qx * qy;
                case RotationOrder.ZYX: return qz * qy * qx;
                default:
                    return Quaternion.identity;
            }
        }

        public static Vector3 GetEulerAnglesWithOrder(Vector3 euler, RotationOrder order)
        {
            float x = euler.x;
            float y = euler.y;
            float z = euler.z;
            switch (order)
            {
                case RotationOrder.XYZ: return new Vector3(x, y, z);
                case RotationOrder.XZY: return new Vector3(x, z, y);
                case RotationOrder.YXZ: return new Vector3(y, x, z);
                case RotationOrder.YZX: return new Vector3(y, z, x);
                case RotationOrder.ZXY: return new Vector3(z, x, y);
                case RotationOrder.ZYX: return new Vector3(z, y, x);
                default:
                    return euler;
            }
        }

        public static void GetRotationComponents(out Quaternion qx, out Quaternion qy, out Quaternion qz, Vector3 euler)
        {
            qx = Quaternion.AngleAxis(euler.x, Vector3.right);
            qy = Quaternion.AngleAxis(euler.y, Vector3.up);
            qz = Quaternion.AngleAxis(euler.z, Vector3.forward);
        }

        public static Quaternion GetRotationXYZ(Vector3 euler)
        {
            Quaternion qx, qy, qz;
            GetRotationComponents(out qx, out qy, out qz, euler);
            return qx * qy * qz;
        }

        public static Quaternion GetRotationXZY(Vector3 euler)
        {
            Quaternion qx, qy, qz;
            GetRotationComponents(out qx, out qy, out qz, euler);
            return qx * qz * qy;
        }

        public static Quaternion GetRotationYZX(Vector3 euler)
        {
            Quaternion qx, qy, qz;
            GetRotationComponents(out qx, out qy, out qz, euler);
            return qy * qz * qx;
        }

        public static Quaternion GetRotationYXZ(Vector3 euler)
        {
            Quaternion qx, qy, qz;
            GetRotationComponents(out qx, out qy, out qz, euler);
            return qy * qx * qz;
        }

        public static Quaternion GetRotationZXY(Vector3 euler)
        {
            Quaternion qx, qy, qz;
            GetRotationComponents(out qx, out qy, out qz, euler);
            return qz * qx * qy;
        }

        public static Quaternion GetRotationZYX(Vector3 euler)
        {
            Quaternion qx, qy, qz;
            GetRotationComponents(out qx, out qy, out qz, euler);
            return qz * qy * qx;
        }

        public static Vector3 GetEulerAnglesInBounds(Vector3 eulerAngles, Vector3 eulerMin, Vector3 eulerMax)
        {
            eulerAngles.x = Mathf.Clamp(AngleModulo360(eulerAngles.x), -eulerMax.x, eulerMax.x);
            eulerAngles.y = Mathf.Clamp(AngleModulo360(eulerAngles.y), -eulerMax.y, eulerMax.y);
            eulerAngles.z = Mathf.Clamp(AngleModulo360(eulerAngles.z), -eulerMax.z, eulerMax.z);
            float xMax = Mathf.Lerp(eulerMin.x, eulerMax.x, 1f - (Mathf.Abs(eulerAngles.y) + Mathf.Abs(eulerAngles.z)) / (eulerMax.y + eulerMax.z));
            float yMax = Mathf.Lerp(eulerMin.y, eulerMax.y, 1f - (Mathf.Abs(eulerAngles.x) + Mathf.Abs(eulerAngles.z)) / (eulerMax.x + eulerMax.z));
            float zMax = Mathf.Lerp(eulerMin.z, eulerMax.z, 1f - (Mathf.Abs(eulerAngles.x) + Mathf.Abs(eulerAngles.y)) / (eulerMax.x + eulerMax.y));
            eulerAngles.x = Mathf.Clamp(eulerAngles.x, -xMax, xMax);
            eulerAngles.y = Mathf.Clamp(eulerAngles.y, -yMax, yMax);
            eulerAngles.z = Mathf.Clamp(eulerAngles.z, -zMax, zMax);
            return eulerAngles;
        }

        /// <summary></summary>
        /// <param name="angle">amgle in degrees</param>
        /// <param name="midZeroRange">true for (-180,180) output range, false for (0,360)</param>
        /// <returns></returns>
        public static float AngleModulo360(float angle, bool midZeroRange = true)
        {
            if (midZeroRange)
            {
                angle = angle % 360;
                if (Mathf.Abs(angle) > 180)
                {
                    angle -= Mathf.Sign(angle) * 360;
                }
            }
            else
            {
                angle = ((angle % 360) + 360) % 360;
            }
            return angle;
        }

        /// <summary></summary>
        /// <param name="angle">amgle in degrees</param>
        /// <param name="midZeroRange">true for (-180,180) output range, false for (0,360)</param>
        /// <returns></returns>
        public static Vector3 AnglesModulo360(Vector3 v, bool midZeroRange = true)
        {
            return new Vector3(
                AngleModulo360(v.x, midZeroRange),
                AngleModulo360(v.y, midZeroRange),
                AngleModulo360(v.z, midZeroRange)
                );
        }

        public static float PowF(float a, int n)
        {
            float z = 1f;

            for (int i = 0; i < Math.Abs(n); i++)
            {
                z *= a;
            }
            if (n < 0) z = 1f / z;
            return z;
        }

        public static int PowI(int a, int n)
        {
            int z = 1;
            for (int i = 0; i < Math.Abs(n); i++)
            {
                z *= a;
            }
            return z;
        }

        public static float GetTransitionAsymPolynom32(float elapsedTime, float duration, int n, int m)
        {
            float x = elapsedTime / duration;
            return -2f * PowF(x, n) + 3f * PowF(x, m);
        }

        public static float GetTransitionAsymNormalised(float elapsedTime, float duration, float inflectionPointNormalized, int n, int m)
        {
            float x0 = inflectionPointNormalized * duration;
            float denomPart = m * x0 - n * (x0 - duration);
            float denom, shift;

            //    float a = m/(PowF(x0, n-1)*denomPart);
            //    float b = n/(PowF(x0-duration, m-1)*denomPart);
            //    printf("n=%i m=%i x0=%f duration=%f a=%.10f b=%.10f\n",n,m,x0,duration,a,b);

            if (elapsedTime < x0)
            {
                denom = PowF(x0, n - 1) * denomPart;
                shift = m * PowF(elapsedTime, n) / denom;
            }
            else
            {
                denom = PowF(x0 - duration, m - 1) * denomPart;
                shift = n * PowF(elapsedTime - duration, m) / denom + 1;
            }
            return shift;
        }

        public static float GetTransitionSymmInflected(float elapsedTime, float duration, float inflPtNorm, int n, int m)
        {
            float halfDuration = duration * 0.5f;
            float shift;
            if (elapsedTime < halfDuration)
            {
                shift = GetTransitionAsymNormalised(elapsedTime, halfDuration, inflPtNorm, n, m);
            }
            else
            {
                shift = (1.0f - GetTransitionAsymNormalised(elapsedTime - halfDuration, halfDuration, 1.0f - inflPtNorm, m, n));
            }
            return shift;
        }

        public static float GetTransitionAsymInflected(float elapsedTime, float duration,
                                                       int N, int M, int n, int m, float xMax, float yMax,
                                                       float inflPtNorm0, float inflPtNorm1)
        {
            // N=2;
            // M=2;
            // n=2;
            // m=2;
            // xMax = 0.6f;
            // yMax = 1.4f;
            // inflPtNorm0 = inflPtNorm1 = 0.5f;
            float shift;
            float t0 = duration * xMax;
            if (elapsedTime < t0)
            {
                shift = yMax * GetTransitionAsymNormalised(elapsedTime, t0, inflPtNorm0, N, M);
            }
            else
            {
                shift = 1.0f + (yMax - 1.0f) * (1.0f - GetTransitionAsymNormalised(elapsedTime - t0, duration * (1.0f - xMax), inflPtNorm1, n, m));
            }
            return shift;
        }

        public static float GetTransition(TransitionType transitionType, float elapsedTime, float duration, int power = 1)
        {
            return GetTransition(transitionType, elapsedTime / duration, power);
        }

        public static float GetTransition(TransitionType transitionType, float t, int power = 1)
        {
            float shift;
            switch (transitionType)
            {
                case TransitionType.LINEAR:
                    shift = t;
                    break;
                case TransitionType.PARABOLIC_ACC:
                    shift = PowF(Mathf.Abs(t), power);
                    break;
                case TransitionType.PARABOLIC_DEC:
                    shift = -PowF(Mathf.Abs(t - 1f), power) + 1f;
                    break;
                case TransitionType.HYPERBOLIC_ACC:
                    shift = -1f / (0.5f * PowF(Mathf.Abs(t), power) - 1f) - 1f;
                    break;
                case TransitionType.HYPERBOLIC_DEC:
                    shift = 1f / (0.5f * PowF(Mathf.Abs(t - 1f), power) - 1f) + 2f;
                    break;
                case TransitionType.SIN_IN_PI2_RANGE:
                    {
                        float omegaT = t * M_PI2;
                        float sinOmegaT = Mathf.Sin(omegaT);
                        shift = sinOmegaT;
                    }
                    break;
                case TransitionType.SIN_IN_PI_RANGE:
                    {
                        float omegaT = t * M_PI;
                        float sinOmegaT = Mathf.Sin(omegaT);
                        shift = sinOmegaT;
                    }
                    break;
                case TransitionType.SIN_IN_2PI_RANGE:
                    {
                        float omegaT = t * 2f * M_PI;
                        float sinOmegaT = Mathf.Sin(omegaT);
                        shift = sinOmegaT;
                    }
                    break;
                case TransitionType.COS_IN_PI2_RANGE:
                    {
                        float omegaT = t * M_PI2;
                        float cosOmegaT = Mathf.Cos(omegaT);
                        shift = -cosOmegaT + 1f;
                    }
                    break;
                case TransitionType.COS_IN_PI_RANGE:
                    {
                        float omegaT = t * M_PI;
                        float cosOmegaT = Mathf.Cos(omegaT);
                        shift = (-cosOmegaT + 1f) / 2f;
                    }
                    break;
                case TransitionType.COS_IN_2PI_RANGE:
                    {
                        //            float omegaT=timePassed*M_PI/duration;
                        //            float cosOmegaT=cosf(omegaT);
                        //            // cos(2x)=2*cos(x)^2-1
                        //            shift = (-PowF(cosOmegaT,2)+1.f);
                        float omegaT = t * 2f * M_PI;
                        float cosOmegaT = Mathf.Cos(omegaT);
                        shift = (-cosOmegaT + 1f) / 2f;
                    }
                    break;
                default:
                    {
                        shift = 0;
                    }
                    break;
            }
            return shift;
        }

        public static int GetClosestPowerOf2(int i, bool upper)
        {
            //    int x = 1;
            //    while (x<i) {
            //        x<<=1;
            //    }
            //    return greater ? x : (x>>1);
            int x = i;
            if (x < 0)
                return 0;
            x--;
            x |= x >> 1;
            x |= x >> 2;
            x |= x >> 4;
            x |= x >> 8;
            x |= x >> 16;
            x++;
            return upper ? x : (x >> 1);
        }

        public static bool IsPowerOf2(int i)
        {
            //return i > 0 && (i & (i - 1)) == 0;
            return Mathf.IsPowerOfTwo(i);
        }

        public static int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        public static short Clamp(short value, short min, short max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        public static bool GetRayIntersectionWithPlane(Ray ray, Vector3 planeUp, Vector3 planePos, out Vector3 isecPt, out float distance)
        {
            Plane plane = new Plane(planeUp, planePos);
            if (plane.Raycast(ray, out float rayDistance))
            {
                distance = rayDistance;
                isecPt = ray.GetPoint(rayDistance);
                return true;
            }
            else
            {
                distance = 0f;
                isecPt = Vector3.zero;
                return false;
            }
        }

        public static bool GetRayIntersectionWithPlane(Ray ray, Vector3 planeUp, Vector3 planePos, out Vector3 isecPt)
        {
            return GetRayIntersectionWithPlane(ray, planeUp, planePos, out isecPt, out _);
        }

        public static bool GetRayIntersectionWithPlane(Vector3 rayOrig, Vector3 rayDir, Vector3 planeUp, Vector3 planePos, out Vector3 isecPt)
        {
            Ray ray = new Ray(rayOrig, rayDir);
            return GetRayIntersectionWithPlane(ray, planeUp, planePos, out isecPt, out _);
        }

        public static bool GetLineIntersectionWithPlane(Vector3 pt1, Vector3 pt2, Vector3 planeUp, Vector3 planePos, out Vector3 isecPt)
        {
            Ray ray = new Ray(pt1, pt2 - pt1);
            return GetRayIntersectionWithPlane(ray, planeUp, planePos, out isecPt, out _);
        }

        public static bool GetTouchRayIntersectionWithPlane(Camera camera, Vector3 touchPos, Vector3 planeUp, Vector3 planePos, out Vector3 isecPt)
        {
            Ray ray = camera.ScreenPointToRay(touchPos);
            return GetRayIntersectionWithPlane(ray, planeUp, planePos, out isecPt, out _);
        }

        public static bool GetTouchRayIntersectionWithPlane(Camera camera, Vector3 touchPos, Transform planeTransform, out Vector3 isecPt)
        {
            return GetTouchRayIntersectionWithPlane(camera, touchPos, planeTransform.up, planeTransform.position, out isecPt);
        }

        public static float GetGeometricSum(float a, float q, int n)
        {
            return a * (1f - PowF(q, n)) / (1f - q);
        }
    }
}