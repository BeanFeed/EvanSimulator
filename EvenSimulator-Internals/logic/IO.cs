using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenSimulator_Internals.logic
{
    public class IO
    {
        public virtual void ClearScreen() {}
        public virtual void DrawSprite() {}
        public virtual void Render() {}
    }
}
