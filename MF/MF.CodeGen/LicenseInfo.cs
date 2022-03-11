using System;

namespace CodeGen
{
    [Serializable]
    public class LicenseInfo
    {
        public string Product { get; set; }
        public string MachineId { get; set; }
        public string StartDateTime { get; set; }
        public string EndDateTime { get; set; }
        public string Company { get; set; }
        public string Sig { get; set; }
    }
}