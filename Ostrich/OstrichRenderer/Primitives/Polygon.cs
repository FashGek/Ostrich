using OstrichRenderer.RenderMath;
using System.Collections.Generic;
using System.Linq;

namespace OstrichRenderer.Primitives
{
    public class Polygon
    {
        public ushort Material;
        private readonly List<LineSeg> _lineSegs;

        /// 点按顺时针顺序传递
        public Polygon(ushort material, params LineSeg[] lineSegs)
        {
            _lineSegs = lineSegs.ToList();
            Material = material;
        }

        /// 点按顺时针顺序传递
        public Polygon(ushort material, params Vector2[] vector2s)
        {
            if (vector2s.Length < 3) return;
            _lineSegs = new List<LineSeg>();
            for (int i = 0; i < vector2s.Length; i++)
            {
                if (i == vector2s.Length - 1)
                {
                    _lineSegs.Add(new LineSeg(vector2s[i], vector2s[0]));
                    break;
                }
                _lineSegs.Add(new LineSeg(vector2s[i], vector2s[i + 1]));
            }
            Material = material;
        }

        public bool Hit(LineSeg ray, ref HitRecord rec)
        {
            bool isHit = false;
            bool isInside = IsInside(ray.P1);
            foreach (LineSeg lineSeg in _lineSegs)
                if (LineSeg.IsIntersect(ray, lineSeg, ref rec)) isHit = true;
            if (!isHit && !isInside) return false;
            rec.Material = Material;
            rec.IsInside = isInside;
            return true;
        }

        public bool IsInside(Vector2 point) => _lineSegs.All(lineSeg => lineSeg.IsInside(point));
    }
}