using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BusinessPriorityRatings: ValueObject {
        public int Benefit { get; private set; }
        public int Penalty { get; private set; }
        public int Cost { get; private set; }
        public int Risk { get; private set; }

        public BusinessPriorityRatings(int benefit, int penalty, int cost, int risk) {
            AssertionConcern.Range(benefit, 1, 9, "Relative benefit must be between 1 and 9.");
            AssertionConcern.Range(penalty, 1, 9, "Relative penalty must be between 1 and 9.");
            AssertionConcern.Range(cost, 1, 9, "Relative cost must be between 1 and 9.");
            AssertionConcern.Range(risk, 1, 9, "Relative risk must be between 1 and 9.");

            this.Benefit = benefit;
            this.Penalty = penalty;
            this.Cost = cost;
            this.Risk = risk;
        }

        public BusinessPriorityRatings WithAdjustedBenefit(int benefit) {
            return new BusinessPriorityRatings(benefit, this.Penalty, this.Cost, this.Risk);
        }

        public BusinessPriorityRatings WithAdjustedPenalty(int penanlty) {
            return new BusinessPriorityRatings(this.Benefit, penanlty, this.Cost, this.Risk);
        }

        public BusinessPriorityRatings WithAdjustedCost(int cost) {
            return new BusinessPriorityRatings(this.Benefit, this.Penalty, cost, this.Risk);
        }

        public BusinessPriorityRatings WithAdjustedRisk(int risk) {
            return new BusinessPriorityRatings(this.Benefit, this.Penalty, this.Cost, risk);
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.Benefit;
            yield return this.Penalty;
            yield return this.Cost;
            yield return this.Risk;
        }
    }
}