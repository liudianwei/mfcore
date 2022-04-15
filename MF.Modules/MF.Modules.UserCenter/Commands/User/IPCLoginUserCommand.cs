using MediatR;

using MF.FluentValidation;

namespace UserCenter.Commands
{
    public class IPCLoginUserCommand : IRequest<PubResponse>
    {
        public string ShiftCode { get; set; }
        public string ShiftId { get; set; }
        public string LineCode { get; set; }
        public string OpName { get; set; }
        public string WorkStationId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string LoginType { get; set; }
        public string ShiftName { get; set; }
        public string OpDesc { get; set; }
    }
}