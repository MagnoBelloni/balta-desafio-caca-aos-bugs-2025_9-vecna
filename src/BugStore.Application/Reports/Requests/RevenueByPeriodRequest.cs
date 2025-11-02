using BugStore.Application.Reports.Responses;
using BugStore.Domain.Exceptions;
using MediatR;

namespace BugStore.Application.Reports.Requests
{
    public class RevenueByPeriodRequest : IRequest<RevenueByPeriodResponse>
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public void CheckIsValid()
        {
            if(Year > DateTime.Now.Year)
                throw new CustomAppException("O ano não pode ser maior que o atual");

            if (Month is < 1 or > 12)
                throw new CustomAppException("O mês precisa ser entre 1 e 12");
        }
    }
}
