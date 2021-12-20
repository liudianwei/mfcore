using System.Collections.Generic;

namespace MF.Orm
{
    public class BaseTreeDto<T> : BaseDto
    {
        public virtual int Level { get; set; }
        public virtual List<T> Childrens { get; set; }
    }
}