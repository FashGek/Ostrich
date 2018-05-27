using OstrichRenderer.Materials;
using OstrichRenderer.Rendering;
using OstrichRenderer.RenderMath;
using System;

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
            bool isInside = IsInside(ray.Origin);
            Vector2 oc = Center - ray.Origin; //圆心到射线起点的向量
            //if (oc.Magnitude() <= R)//点在圆内
            //{
            //    rec.T = 0;
            //    rec.P = ray.Origin;
            //    rec.Material = Material;
            //    return true;
            //}

            double proj = oc * ray.Direction; //圆心在射线上的投影
            if (proj < 0 && oc.Magnitude() > R) return false; //反向射线

            double dis2 = oc.X * oc.X + oc.Y * oc.Y - proj * proj; //勾股定理求出圆心到直线的距离（未开根
            if (dis2 > R2) return false; //距离大于半径，无交点

            double tangent = R2 - dis2; //求出半弦长（未开根
            tangent = Math.Sqrt(tangent);
            Vector2 inter = ray.Origin + ray.Direction * (proj + (isInside ? tangent : -tangent));
            double t = (inter - ray.Origin).Magnitude();
            if (t > tMax) return false;//确认是不是最近的交点
            rec.P = inter;
            rec.T = t;
            rec.IsInside = isInside;
            rec.Normal = (inter - Center).Normalize();//计算法线
            if (IsInside(ray.Origin))//如果射线起点在圆内
                rec.Normal = -rec.Normal;//翻转法线
            rec.Material = Material;
            rec.Object = this;

            return true;
        }

        public override bool IsInside(Vector2 point) => (Center - point).Magnitude() <= R;
        public override HitRecord[] GetAllCross(Ray ray, double tMin, double tMax)
        {
            bool isInside = IsInside(ray.Origin);

            Vector2 oc = Center - ray.Origin; //圆心到射线起点的向量

            double proj = oc * ray.Direction; //圆心在射线上的投影
            if (proj < 0 && oc.Magnitude() > R) return new HitRecord[0]; //反向射线

            double dis2 = oc.X * oc.X + oc.Y * oc.Y - proj * proj; //勾股定理求出圆心到直线的距离（未开根
            if (dis2 > R2) return new HitRecord[0]; //距离大于半径，无交点

            double tangent = Math.Sqrt(R2 - dis2); //求出半弦长（未开根
            HitRecord inter1 = new HitRecord();
            inter1.P = ray.Origin + ray.Direction * (proj + tangent);
            inter1.Normal = -(inter1.P - Center).Normalize();//计算法线
            //if (isInside)//如果射线起点在圆内
            //    inter1.Normal = -inter1.Normal;//翻转法线
            inter1.IsInside = isInside;
            inter1.T = (inter1.P - ray.Origin).Magnitude();
            inter1.Material = Material;
            inter1.Object = this;
            HitRecord inter2 = new HitRecord();
            inter2.P = ray.Origin + ray.Direction * (proj - tangent);
            inter2.Normal = (inter2.P - Center).Normalize();//计算法线
            if (isInside)//如果射线起点在圆内
                inter2.Normal = -inter2.Normal;//翻转法线
            inter2.IsInside = isInside;
            inter2.T = (inter2.P - ray.Origin).Magnitude();
            inter2.Material = Material;
            inter2.Object = this;
            return isInside ? new[] {inter1} : new[] {inter1, inter2};
        }
    }
}
