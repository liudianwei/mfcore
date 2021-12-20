using System;
using System.Collections.Generic;
using System.Text;

namespace MF.Utils
{
    public class IDCreator
    {
        static long m_curid = 1;
        static object mlock = new object();
        public static long getSeqID()
        {
            lock (mlock)
            {
                if (m_curid > long.MaxValue)
                {
                    m_curid = 1;
                }
                m_curid++;
                return m_curid;
            }
        }
    }
}
