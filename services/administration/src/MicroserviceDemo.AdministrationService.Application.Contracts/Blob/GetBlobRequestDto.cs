using System.ComponentModel.DataAnnotations;

namespace MicroserviceDemo.AdministrationService.Blob;

public class GetBlobRequestDto
{
    [Required]
    public string Name { get; set; }
}
