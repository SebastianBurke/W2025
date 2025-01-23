using System.ComponentModel.DataAnnotations;

namespace HIAAA.DTO;

public class AddAppDTO
{
    public long AppId { get; set; }
    public string AppCode { get; set; }
    public string AppName { get; set; }
    public string Appdescription { get; set; }
    [Required(ErrorMessage = "App type is required")]
    public int Apptype { get; set; }
    public long CreatedBy { get; set; }
    
    [Required(ErrorMessage = "Assigned App Admin Id is required")]
    public long AssignedAppAdminId { get; set; }
}