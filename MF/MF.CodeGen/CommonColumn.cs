namespace CodeGen
{
    public class CommonColumn
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
        public int Decimal { get; set; }
        public bool IsNull { get; set; }
        public string Comment { get; set; }
    }
}