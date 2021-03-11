using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoga.Tools.Factory
{
    public class ToolGroupsChangeEventArgs : EventArgs
    {
        private int toolGroupsNum;
        public int ToolGroupsNum
        {
            get
            {
                return toolGroupsNum;
            }
            set
            {
                toolGroupsNum = value;
            }
        }
    }
}
