using Microsoft.Build.Framework;

namespace HIAAAServices.DTO;

public class AddAppDto
{
    public long AppId { get; set; }
    [Required]
    public long AssignedAppAdminId { get; set; }
    public string AppCode { get; set; }
    
    public string AppName { get; set; }
    public string Appdescription { get; set; }
    public int Apptype { get; set; }
    public long CreatedBy { get; set; }
}