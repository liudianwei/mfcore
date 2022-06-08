using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SystemFramework
{
    /// <summary>
    ///
    /// </summary>
    public interface MesFormInterface
    {
        /// <summary>
        ///
        /// </summary>
        Form MesMainfrm { get; }

        /// <summary>
        ///
        /// </summary>
        string MenuItemCode { get; set; }

        /// <summary>
        ///
        /// </summary>
        void ReflashForm();
    }
}