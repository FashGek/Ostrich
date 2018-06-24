using OstrichRenderer.Primitives;
using OstrichRenderer.RenderMath;
using System;

namespace OstrichRenderer.Materials
{
    public abstract class Material
    {
        /// 反射率
        public double Reflectivity;
        /// 折射率
        public double Refractivity;

        public abstract bool Reflect(LineSeg rayIn, HitRecord record, ref Color32 attenuation, ref LineSeg scattered);

        ///获取反射射线
        public static Vector2 Reflect(Vector2 vin, Vector2 normal) => vin - 2 * Vector2.Dot(vin, normal) * normal;

        ///获取折射射线
        public bool Refract(LineSeg rayIn, HitRecord record, ref LineSeg refracted)
        {
            if (Refractivity < double.Epsilon) return false; //判断是不是反射材质
            double idotn = rayIn.NormalP2 * record.Normal;
            double k, a;
            if (idotn > 0) //从内向外折射
            {
                k = 1 - Refractivity * Refractivity * (1 - idotn * idotn);
                if (k < 0) return false;  //全反射
                a = Refractivity * idotn - Math.Sqrt(k);
            }
            else //从外向内折射
            {
                Refractivity = 1 / Refractivity;
                k = 1 - Refractivity * Refractivity * (1 - idotn * idotn);
                a = Refractivity * idotn + Math.Sqrt(k);
            }
            Vector2 refract = rayIn.NormalP2 * Refractivity - record.Normal * a;
            refracted = new LineSeg(record.P + 0.0001 * (record.IsInside ? record.Normal : -record.Normal), refract * 1e7);
            return true;
        }
    }

    public class Light : Material
    {
        /// 强度
        public double Intensity;

        public Color32 Color;

        public Light(Color32 color, double intensity, double reflectivity)
        {
            Color = color;
            Intensity = intensity;
            Reflectivity = reflectivity;
        }

        /// 获取反射后的射线
        /// <param name="rayIn">入射射线</param>
        /// <param name="record">记录</param>
        /// <param name="attenuation">入射点的颜色</param>
        /// <param name="scattered">反射射线</param>
        /// <returns>是否可以反射</returns>
        public override bool Reflect(LineSeg rayIn, HitRecord record, ref Color32 attenuation, ref LineSeg scattered)
        {
            attenuation = Color * Intensity;
            if (record.IsInside) return false;
            if (record.P == rayIn.P1) return false;
            //给起点加上一个法线方向的偏移，防止射中原地
            scattered = new LineSeg(record.P + 0.0001 * record.Normal,
                Reflect(record.P - rayIn.P1, record.Normal) * 1e7);
            return true;
        }
    }

    public class Dielectirc : Material
    {
        public Dielectirc(double refractivity, double reflectivity)
        {
            Refractivity = refractivity;
            Reflectivity = reflectivity;
        }

        public override bool Reflect(LineSeg rayIn, HitRecord record, ref Color32 attenuation, ref LineSeg scattered)
        {
            if (record.P == rayIn.P1) return false;
            //如果起点在物体内则要翻转法线
            scattered = new LineSeg(record.P + 0.0001 * (record.IsInside ? -record.Normal : record.Normal),
                Reflect(record.P - rayIn.P1, record.Normal) * 1e7);
            return true;
        }
    }

    public class Metal : Material
    {
        public Metal(double reflectivity)
        {
            Reflectivity = reflectivity;
        }

        public override bool Reflect(LineSeg rayIn, HitRecord record, ref Color32 attenuation, ref LineSeg scattered) =>
            !record.IsInside && record.P != rayIn.P1;
    }
}
