using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BusinessPriorityTotals: ValueObject {
        public int TotalBenefit { get; private set; }
        public int TotalPenalty { get; private set; }
        public int TotalValue { get; private set; }
        public int TotalCost { get; private set; }
        public int TotalRisk { get; private set; }

        public BusinessPriorityTotals(int totalBenefit, int totalPenalty, int totalValue, int totalCost, int totalRisk) {
            this.TotalBenefit = totalBenefit;
            this.TotalPenalty = totalPenalty;
            this.TotalValue = totalValue;
            this.TotalCost = totalCost;
            this.TotalRisk = totalRisk;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.TotalBenefit;
            yield return this.TotalPenalty;
            yield return this.TotalValue;
            yield return this.TotalCost;
            yield return this.TotalRisk;
        }
    }
}