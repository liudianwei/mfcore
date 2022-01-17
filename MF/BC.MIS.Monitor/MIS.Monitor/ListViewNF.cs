using System.Windows.Forms;
/// <summary>
/// 
/// </summary>
public class ListViewNF : ListView
{
    /// <summary>
    /// 
    /// </summary>
    public ListViewNF()
    {
        // ¿ªÆôË«»º³å
        this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        this.SetStyle(ControlStyles.EnableNotifyMessage, true);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="m"></param>
    protected override void OnNotifyMessage(Message m)
    {
        //Filter out the WM_ERASEBKGND message
        if (m.Msg != 0x14)
        {
            base.OnNotifyMessage(m);
        }
    }
}