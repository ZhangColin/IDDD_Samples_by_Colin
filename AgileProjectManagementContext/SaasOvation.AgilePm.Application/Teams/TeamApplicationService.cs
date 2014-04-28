using System;
using SaasOvation.AgilePm.Domain.Teams.Model;
using SaasOvation.AgilePm.Domain.Teams.Repository;
using SaasOvation.AgilePm.Domain.Tenants;

namespace SaasOvation.AgilePm.Application.Teams {
    public class TeamApplicationService {
        private readonly ITeamMemberRepository _teamMemberRepository;
        private readonly IProductOwnerRepository _productOwnerRepository;

        public TeamApplicationService(ITeamMemberRepository teamMemberRepository,
            IProductOwnerRepository productOwnerRepository) {
            this._teamMemberRepository = teamMemberRepository;
            this._productOwnerRepository = productOwnerRepository;
        }

        public void EnableProductOwner(EnableProductOwnerCommand command) {
            TenantId tenantId = new TenantId(command.TenantId);
            ApplicationServiceLifeCycle.Begin();
            try {
                ProductOwner productOwner = this._productOwnerRepository.Get(tenantId, command.UserName);
                if (productOwner != null) {
                    productOwner.Enable(command.OccurredOn);
                }
                else {
                    productOwner = new ProductOwner(tenantId, command.UserName, command.FirstName, command.LastName,
                        command.EmailAdddress, command.OccurredOn);

                    this._productOwnerRepository.Save(productOwner);
                }
                ApplicationServiceLifeCycle.Success();
            }
            catch (Exception ex) {
                ApplicationServiceLifeCycle.Fail(ex);
            }
        }

        public void EnableTeamMember(EnableTeamMemberCommand command) {
            TenantId tenantId = new TenantId(command.TenantId);
            ApplicationServiceLifeCycle.Begin();
            try {
                TeamMember teamMember = this._teamMemberRepository.Get(tenantId, command.UserName);
                if (teamMember != null) {
                    teamMember.Enable(command.OccurredOn);
                }
                else {
                    teamMember = new TeamMember(tenantId, command.UserName, command.FirstName, command.LastName,
                        command.EmailAdddress, command.OccurredOn);
                    this._teamMemberRepository.Save(teamMember);
                }
                ApplicationServiceLifeCycle.Success();
            }
            catch (Exception ex) {
                ApplicationServiceLifeCycle.Fail(ex);
            }
        }

        public void ChangeTeamMemberEmailAddress(ChangeTeamMemberEmailAddressCommand command) {
            TenantId tenantId = new TenantId(command.TenantId);
            ApplicationServiceLifeCycle.Begin();
            try {
                ProductOwner productOwner = this._productOwnerRepository.Get(tenantId, command.UserName);
                if (productOwner != null) {
                    productOwner.ChangeEmailAddress(command.EmailAddress, command.OccurredOn);
                    this._productOwnerRepository.Save(productOwner);
                }

                TeamMember teamMember = this._teamMemberRepository.Get(tenantId, command.UserName);
                if (teamMember != null) {
                    teamMember.ChangeEmailAddress(command.EmailAddress, command.OccurredOn);
                    this._teamMemberRepository.Save(teamMember);
                }
                ApplicationServiceLifeCycle.Success();
            }
            catch (Exception ex) {
                ApplicationServiceLifeCycle.Fail(ex);
            }
        }

        public void ChangeTeamMemberName(ChangeTeamMemberNameCommand command) {
            TenantId tenantId = new TenantId(command.TenantId);
            ApplicationServiceLifeCycle.Begin();
            try {
                ProductOwner productOwner = this._productOwnerRepository.Get(tenantId, command.UserName);
                if (productOwner != null) {
                    productOwner.ChangeName(command.FirstName, command.LastName, command.OccurredOn);
                    this._productOwnerRepository.Save(productOwner);
                }

                TeamMember teamMember = this._teamMemberRepository.Get(tenantId, command.UserName);
                if (teamMember != null) {
                    teamMember.ChangeName(command.FirstName, command.LastName, command.OccurredOn);
                    this._teamMemberRepository.Save(teamMember);
                }
                ApplicationServiceLifeCycle.Success();
            }
            catch (Exception ex) {
                ApplicationServiceLifeCycle.Fail(ex);
            }
        }

        public void DisableProductOwner(DisableProductOwnerCommand command) {
            TenantId tenantId = new TenantId(command.TenantId);
            ApplicationServiceLifeCycle.Begin();
            try {
                ProductOwner productOwner = this._productOwnerRepository.Get(tenantId, command.UserName);
                if(productOwner!=null) {
                    productOwner.Disable(command.OccurredOn);
                    this._productOwnerRepository.Save(productOwner);
                }
                ApplicationServiceLifeCycle.Success();
            }
            catch(Exception ex) {
                ApplicationServiceLifeCycle.Fail(ex);
            }
        }

        public void DisableTeamMember(DisableTeamMemberCommand command) {
            TenantId tenantId = new TenantId(command.TenantId);
            ApplicationServiceLifeCycle.Begin();
            try {
                TeamMember teamMember = this._teamMemberRepository.Get(tenantId, command.UserName);
                if(teamMember!=null) {
                    teamMember.Disable(command.OccurredOn);
                    this._teamMemberRepository.Save(teamMember);
                }
            }
            catch(Exception ex) {
               ApplicationServiceLifeCycle.Fail(ex); 
            }
        }
    }
}