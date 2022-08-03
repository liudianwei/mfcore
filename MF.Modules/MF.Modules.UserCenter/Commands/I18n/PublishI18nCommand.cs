using MediatR;
using MF.FluentValidation;

namespace MDCenter.Commands.I18n
{
    public class PublishI18nCommand : IRequest<PubResponse>
    {
        public PublishI18nCommand()
        {

        }
        /// <summary>
        /// 文件服务器网络地址(gofasturl)
        /// </summary>
        public string NetUrl { get; set; }
    }
}