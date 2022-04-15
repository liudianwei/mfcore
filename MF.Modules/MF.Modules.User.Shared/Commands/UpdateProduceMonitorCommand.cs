using MediatR;

using MF.FluentValidation;

namespace ProductCenter.Commands.ProduceMonitor
{
    public class UpdateProduceMonitorCommand : IRequest<PubResponse>
    {
        public UpdateProduceMonitorCommand()
        {
        }

        public string Id { get; set; }
        public string LineCode { get; set; }
        public string LineName { get; set; }

        public string OpName { get; set; }

        public string OpDesc { get; set; }

        public string OrderNum { get; set; }

        public int EngineCode { get; set; }

        public string EngineType { get; set; }

        public string ComponentSn { get; set; }

        public string Operator { get; set; }

        public string PalletCode { get; set; }

        public string ShiftCode { get; set; }

        public string ShiftName { get; set; }

        public string Remark { get; set; }
        public string UserName { get; set; }
    }
}