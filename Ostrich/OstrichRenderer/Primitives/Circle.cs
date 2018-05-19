using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OstrichRenderer.Materials;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    public class Circle : Hitable
    {
        public Vector2 Center;
        public double R, R2;
        public Material Material;

        public Circle(Vector2 cen, double r, Material material)
        {
            Center = cen;
            Material = material;
            R = r;
            R2 = r * r;
        }

        public override bool Hit(Ray ray, double tMin, double tMax, ref HitRecord rec)
        {
            Vector2 oc = Center - ray.Origin; //圆心到射线起点的向量
            if (oc.Magnitude() <= R)//点在圆内
            {
                rec.Material = Material;
                return true;
            }

            double proj = oc * ray.Direction; //圆心在射线上的投影
            if (proj < 0&& oc.Magnitude() > R) return false; //反向射线

            double dis2 = oc.X * oc.X + oc.Y * oc.Y - proj * proj; //勾股定理求出圆心到直线的距离（未开根
            if (dis2 > R2) return false; //距离大于半径，无交点

            double tangent = R2 - dis2; //求出半弦长（未开根
            tangent = Math.Sqrt(tangent);
            Vector2 inter = ray.Origin + ray.Direction * (proj - tangent);
            double t = (inter - ray.Origin).Magnitude();
            if (t > tMax) return false;//确认是不是最近的交点
            rec.P = inter;
            rec.T = t;
            rec.Normal = (inter - Center).Normalize();//计算法线
            rec.Material = Material;

            return true;
        }
    }
}
