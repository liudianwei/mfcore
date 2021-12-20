namespace MF.Utils.Excel
{
    public class ImgPosition
    {
        public ImgPosition(string x1, string y1, string x2, string y2, string c1, string r1, string c2, string r2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Col1 = c1;
            Row1 = r1;
            Col2 = c2;
            Row2 = r2;
        }

        public string Col1 { get; set; }
        public string Row1 { get; set; }
        public string X1 { get; set; }
        public string Y1 { get; set; }
        public string Col2 { get; set; }
        public string Row2 { get; set; }
        public string X2 { get; set; }
        public string Y2 { get; set; }
    }
}