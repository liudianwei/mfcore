using MediatR;

namespace MF.MediatR
{
    public class PageCommand<T> : IRequest<T>
    {
        /// <summary>
        /// 条件
        /// </summary>
        public string Condition { get; set; } = "";

        /// <summary>
        /// 排序
        /// </summary>
        public virtual string Order { get; set; } = "";

        /// <summary>
        /// 页码
        /// </summary>
        public int PageNum { get; set; } = 1;

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}