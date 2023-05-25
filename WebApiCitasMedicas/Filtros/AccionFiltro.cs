using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApiCitasMedicas.Filtros
{
    public class AccionFiltro : IActionFilter
    {
        private readonly ILogger<AccionFiltro> log;

        public AccionFiltro(ILogger<AccionFiltro> log){
            this.log = log;
        }

        public void OnActionExecuting(ActionExecutingContext context){
            log.LogInformation("Antes de ejecutar la acción");
        }

        public void OnActionExecuted(ActionExecutedContext context){
            log.LogInformation("Despues de ejecutar la acción");
        }
    }
}
