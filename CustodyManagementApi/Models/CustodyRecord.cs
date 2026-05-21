namespace CustodyManagementApi.Models;

public class CustodyRecord
{
    public Guid Id { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public string CaseNumber { get; set; } = string.Empty;
    public DateTime ArrestedAtUtc { get; set; }
    public string Facility { get; set; } = string.Empty;
    public string Status { get; set; } = "InCustody";
    public DateTime CreatedAtUtc { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
