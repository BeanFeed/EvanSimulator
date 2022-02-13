using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic
{
    public class InputKey
    {
        public int[] KeyCodes = new int[] {};
        public bool pressed = false;

        public InputKey(int[] keyCodes)
        {
            this.KeyCodes = keyCodes;
        }
    }
}
