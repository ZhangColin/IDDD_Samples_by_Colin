using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SaasOvation.IdentityAccess.Application;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.WebApi.Controllers
{
    public class TenantsController : ApiController
    {
        private readonly IdentityApplicationService _identityApplicationService;

        public TenantsController(IdentityApplicationService identityApplicationService) {
            this._identityApplicationService = identityApplicationService;
        }

        public dynamic Get(string tenantId) {
            Tenant tenant = _identityApplicationService.GetTenant(tenantId);
            if(tenant==null) {
                throw new ApplicationException("Not Found");
            }
            return new {tenant.TenantId.Id, tenant.Name, tenant.Description, tenant.Active, tenant.ConcurrencyVersion};
        }
    }
}
