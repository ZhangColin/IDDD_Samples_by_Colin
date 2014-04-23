using System;
using System.Web.Mvc;
using SaasOvation.IdentityAccess.Application;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.Web.Controllers
{
    public class TenantsController : Controller
    {
        private readonly IdentityApplicationService _identityApplicationService;

        public TenantsController(IdentityApplicationService identityApplicationService) {
            this._identityApplicationService = identityApplicationService;
        }

        public ActionResult Index(string tenantId)
        {
            Tenant tenant = _identityApplicationService.GetTenant(tenantId);
            if (tenant == null) {
                throw new ApplicationException("Not Found");
            }
            return this.Json(
                new {tenant.TenantId.Id, tenant.Name, tenant.Description, tenant.Active, tenant.ConcurrencyVersion}, 
                JsonRequestBehavior.AllowGet);

        }
	}
}