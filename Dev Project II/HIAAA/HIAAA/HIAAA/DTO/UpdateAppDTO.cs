namespace HIAAA.DTO;

public class UpdateAppDTO
{
    public long AppId { get; set; }
    public string AppCode { get; set; }
    public string AppName { get; set; }
    public string Appdescription { get; set; }
    public int Apptype { get; set; }
    public long CreatedBy { get; set; }
}