using BugStore.Domain.Constants;

namespace BugStore.Domain.Dtos
{
    public abstract class PagedRequestDto
    {
        public int? Page { private get; set; }

        public int? PageSize { private get; set; }

        /// <summary>
        /// Ao usar minimal apis e tentar deixar um campo opcional com valor default não funciona da mesma maneira a uma controller.<br></br>
        /// Então é necessário deixar o campo nullable e atribuir valor em runtime, mesmo atribuindo valor default para a prop o campo não vem preenchido.<br></br>
        /// Por esse motivo esse metodo foi criado para deixar o valor default como padrão.
        /// </summary>
        /// <returns>Page e PageSize</returns>
        public (int Page, int PageSize) GetPageInfo() => (Page ?? PageConstants.DEFAULT_PAGE, PageSize ?? PageConstants.DEFAULT_PAGE_SIZE);
    }
}
