using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;

namespace OstrichRenderer.Primitives
{
    public class Circle : Hitable
    {
        public Vector2 Center;
        public double R, R2, D, E, F;

        public Circle(Vector2 cen, double r)
        {
            Center = cen;
            R = r;
            R2 = r * r;
            D = 2 * cen.x;
            E = 2 * cen.y;
            F = cen.x * cen.x + cen.y * cen.y;
        }

        public override bool Hit(Ray ray, double t_min, double t_max, ref HitRecord rec)
        {
            Vector2 oc = Center - ray.Origin; //圆心到射线起点的向量
            if (oc.Magnitude() < R)
                return true;

            double proj = oc * ray.Direction; //圆心在射线上的投影
            if (proj < 0) return false; //反向射线,并且起射点在圆外

            double dis2 = oc.x * oc.x + oc.y * oc.y - proj * proj; //勾股定理求出圆心到直线的距离（未开根
            if (dis2 > R2) return false; //距离大于半径，无交点

            Vector2 foot = ray.Origin + ray.Direction * proj; //圆心到射线的垂足
            double tangent = R2 - dis2; //求出半弦长
            if (tangent < double.Epsilon)//只有一个交点，相切
            {
                rec.t = proj; 
                if (foot.Magnitude() > t_max) return false;//确认是不是最近的交点
                rec.p = foot;
            }
            else
            { 
                tangent = Math.Sqrt(tangent);
                Vector2 inter = foot - ray.Direction * tangent;
                if (inter.Magnitude() > t_max) return false;//确认是不是最近的交点
                rec.p = inter;
                rec.t = inter.Magnitude();
            }

            return true;
        }
    }
}
