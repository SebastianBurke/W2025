using Microsoft.Build.Framework;

namespace HIAAAServices.DTO;

public class UpdateAppDto
{
    public long AppId { get; set; }
    
    public string AppCode { get; set; }
    [Required]
    public string AppName { get; set; }
    [Required]
    public string Appdescription { get; set; }
    public int Apptype { get; set; }
    public long CreatedBy { get; set; }
}