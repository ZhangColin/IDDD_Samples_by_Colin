using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BusinessPrionrity: ValueObject {
        public BusinessPriorityRatings Ratings { get; private set; }

        public BusinessPrionrity(BusinessPriorityRatings ratings) {
            AssertionConcern.NotNull(ratings, "The ratings must be provided.");
            this.Ratings = ratings;
        }

        public float CostPercentage(BusinessPriorityTotals totals) {
            return (float)100 * this.Ratings.Cost / totals.TotalCost;
        }

        public float Priority(BusinessPriorityTotals totals) {
            float costAndRisk = this.CostPercentage(totals) + RiskPercentage(totals);
            return ValuePercentage(totals) / costAndRisk;
        }

        public float RiskPercentage(BusinessPriorityTotals totals) {
            return (float)100 * this.Ratings.Risk / totals.TotalRisk;
        }

        public float ValuePercentage(BusinessPriorityTotals totals) {
            return (float)100 * this.TotalValue / totals.TotalValue;
        }

        public float TotalValue {
            get { return this.Ratings.Benefit + this.Ratings.Penalty; }
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.Ratings;
        }
    }
}