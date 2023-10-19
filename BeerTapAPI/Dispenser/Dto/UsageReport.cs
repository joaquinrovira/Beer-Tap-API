namespace BeerTapAPI.Dtos;

public record DispenserUsageReportResponse(
    decimal Amount,
    IEnumerable<DispenserUsageReportResponseItem> Usages
)
{ }

public record DispenserUsageReportResponseItem(
    DateTime OpenedAt,
    DateTime? ClosedAt,
    decimal FlowVolume,
    decimal TotalSpent
)
{ }