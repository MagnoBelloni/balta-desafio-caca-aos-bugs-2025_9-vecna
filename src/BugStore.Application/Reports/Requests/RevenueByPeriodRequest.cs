using BugStore.Application.Reports.Responses;
using MediatR;

namespace BugStore.Application.Reports.Requests
{
    public class RevenueByPeriodRequest : IRequest<RevenueByPeriodResponse>
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public bool IsValid()
        {
            if (Year < 0)
                return false;

            if (Month is < 1 or > 12)
                return false;

            return true;
        }
    }
}
