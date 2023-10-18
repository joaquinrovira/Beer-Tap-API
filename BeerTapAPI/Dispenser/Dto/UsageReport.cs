namespace BeerTapAPI.Dtos;

public record DispenserUsageReportResponse(
    float Amount,
    IEnumerable<DispenserUsageReportResponseItem> Usages
)
{ }

public record DispenserUsageReportResponseItem(
    DateTime OpenedAt,
    DateTime? ClosedAt,
    float FlowVolume,
    float TotalSpent
)
{ }