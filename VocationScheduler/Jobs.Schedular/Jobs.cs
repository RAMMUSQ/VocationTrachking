using VocationScheduler;

public class Jobs
{
    private readonly IAnnualLeaveService _annualLeaveService;

    public Jobs(IAnnualLeaveService annualLeaveService)
    {
        _annualLeaveService = annualLeaveService;
    }

    public void UpdateAnnualLeave()
    {
        _annualLeaveService.UpdateAnnualLeave().GetAwaiter().GetResult();
    }
}