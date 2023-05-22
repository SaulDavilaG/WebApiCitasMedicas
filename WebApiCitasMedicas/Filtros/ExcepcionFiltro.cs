using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCitasMedicas.Filtros
{
    public class ExcepcionFiltro : ExceptionFilterAttribute
    {
        private readonly ILogger<ExcepcionFiltro> log;

        public ExcepcionFiltro(ILogger<ExcepcionFiltro> log)
        {
            this.log = log;
        }

        public override void OnException(ExceptionContext context)
        {
            log.LogError(context.Exception, context.Exception.Message);

            base.OnException(context);
        }
    }
}