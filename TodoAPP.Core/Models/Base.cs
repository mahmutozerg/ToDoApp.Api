namespace TodoAPP.Core.Models;

public class Base
{
    public DateTime? CreatedAt { get; set; }
    public DateTime? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }


}