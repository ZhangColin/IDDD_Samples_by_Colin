using System;
using System.Runtime.CompilerServices;
using SaasOvation.AgilePm.Domain.Discussions;
using SaasOvation.AgilePm.Domain.Products.Model.Products;
using SaasOvation.AgilePm.Domain.Products.Repository;
using SaasOvation.AgilePm.Domain.Teams.Model;
using SaasOvation.AgilePm.Domain.Teams.Repository;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;
using SaasOvation.Common.Domain.Model.LongRunningProcess;

namespace SaasOvation.AgilePm.Application.Products {
    public class ProductApplicationService {
        private readonly IProductRepository _productRepository;
        private readonly IProductOwnerRepository _productOwnerRepository;
        private readonly ITimeConstrainedProcessTrackerRepository _processTrackerRepository;

        public ProductApplicationService(IProductRepository productRepository,
            IProductOwnerRepository productOwnerRepository,
            ITimeConstrainedProcessTrackerRepository processTrackerRepository) {
            this._productRepository = productRepository;
            this._productOwnerRepository = productOwnerRepository;
            this._processTrackerRepository = processTrackerRepository;
        }

        public void InitiateDiscussion(InitiateDiscussionCommand command) {
            ApplicationServiceLifeCycle.Begin();
            try {
                Product product = this._productRepository.Get(new TenantId(command.TenantId),
                    new ProductId(command.ProductId));
                AssertionConcern.NotNull(product,
                    string.Format("Unknown product of tenant id: {0} and product id: {1}.", command.TenantId,
                        command.ProductId));
                product.InitiateDiscussion(new DiscussionDescriptor(command.DiscussionId));
                this._productRepository.Save(product);

                ProcessId processId = ProcessId.ExistingProcessId(product.DiscussionInitiationId);
                TimeConstrainedProcessTracker tracker = this._processTrackerRepository.Get(command.TenantId, processId);
                tracker.MarkProcessCompleted();
                this._processTrackerRepository.Save(tracker);

                ApplicationServiceLifeCycle.Success();
            }
            catch(Exception ex) {
                ApplicationServiceLifeCycle.Fail(ex);
            }
        }

        public string NewProduct(NewProductCommand command) {
            return NewProductWith(command.TenantId, command.ProductOwnerId, command.Name, command.Description,
                DiscussionAvailability.NotRequested);
        }

        public string NewProductWithDiscussion(NewProductCommand command) {
            return this.NewProductWith(command.TenantId, command.ProductOwnerId, command.Name, command.Description,
                RequestDiscussionIfAvailable());
        }

        public void RequestProductDiscussion(RequestProductDiscussionCommand command) {
            Product product = this._productRepository.Get(new TenantId(command.TenantId),
                new ProductId(command.ProductId));
            AssertionConcern.NotNull(product,
                string.Format("Unknown product of tenant id: {0} and product id: {1}.", command.TenantId,
                    command.ProductId));
            this.RequestProductDiscussionFor(product);
        }

        public void RetryProductDiscussionRequest(RetryProductDiscussionRequestCommand command) {
            ProcessId processId = ProcessId.ExistingProcessId(command.ProcessId);
            TenantId tenantId = new TenantId(command.TenantId);
            Product product = this._productRepository.GetByDiscussionInitiationId(tenantId, processId.Id);
            AssertionConcern.NotNull(product,
                string.Format("Unknown product of tenant id: {0} and discussion initiation id: {1}.", command.TenantId,
                    command.ProcessId));
            this.RequestProductDiscussionFor(product);
        }

        public void StartDiscussionInitiation(StartDiscussionInitiationCommand command) {
            ApplicationServiceLifeCycle.Begin();
            try {
                Product product = this._productRepository.Get(new TenantId(command.TenantId),
                    new ProductId(command.TenantId));
                AssertionConcern.NotNull(product,
                    string.Format("Unknown product of tenant id: {0} and product id: {1}.", command.TenantId,
                        command.ProductId));
                string timedOutEventName = typeof(ProductDiscussionRequestTimedOut).Name;

                TimeConstrainedProcessTracker tracker = new TimeConstrainedProcessTracker(command.TenantId,
                    ProcessId.NewProcessId(), "Create discussion for product: " + product.Name, DateTime.UtcNow,
                    5L * 60L * 1000L, 3, timedOutEventName);
                this._processTrackerRepository.Save(tracker);

                product.StartDiscussionInitiation(tracker.ProcessId.Id);
                this._productRepository.Save(product);

                ApplicationServiceLifeCycle.Success();
            }
            catch (Exception ex) {
                ApplicationServiceLifeCycle.Fail(ex);
            }
        }

        public void TimeOutProductDiscussionRequest(TimeOutProductDiscussionRequestCommand command) {
            ApplicationServiceLifeCycle.Begin();
            try {
                ProcessId processId = ProcessId.ExistingProcessId(command.ProcessId);
                TenantId tenantId = new TenantId(command.TenantId);
                Product product = this._productRepository.GetByDiscussionInitiationId(tenantId, processId.Id);

                SendEmailForTimedOutProcess(product);

                product.FailDiscussionInitiation();
                this._productRepository.Save(product);

                ApplicationServiceLifeCycle.Success();
            }
            catch (Exception ex) {
                ApplicationServiceLifeCycle.Fail(ex);
            }
        }

        private void SendEmailForTimedOutProcess(Product product) {
            //TODO: implement
        }

        private void RequestProductDiscussionFor(Product product) {
            ApplicationServiceLifeCycle.Begin();
            try {
                product.RequestDiscussion(this.RequestDiscussionIfAvailable());
                this._productRepository.Save(product);
                ApplicationServiceLifeCycle.Success();
            }
            catch (Exception ex) {
                ApplicationServiceLifeCycle.Fail(ex);
            }
        }

        private DiscussionAvailability RequestDiscussionIfAvailable() {
            DiscussionAvailability availability = DiscussionAvailability.AddOnNotEnabled;
            bool enabled = true; // TODO: determine add-on enabled
            if(enabled) {
                availability = DiscussionAvailability.Requested;
            }
            return availability;
        }

        public string NewProductWith(string tenantId, string productOwnerId, string name, string description,
            DiscussionAvailability discussionAvailability) {
            TenantId tenant = new TenantId(tenantId);
            ProductId productId = default (ProductId);

            ApplicationServiceLifeCycle.Begin();
            try {
                productId = this._productRepository.GetNextIdentity();
                ProductOwner productOwner = this._productOwnerRepository.Get(tenant, productOwnerId);
                Product product = new Product(tenant, productId, productOwner.ProductOwnerId, name, description,
                    discussionAvailability);
                this._productRepository.Save(product);

                ApplicationServiceLifeCycle.Success();
            }
            catch (Exception ex) {
                ApplicationServiceLifeCycle.Fail(ex);
            }

            //TODO: handle null properly
            return productId.Id;
        }
    }
}