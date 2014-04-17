using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.User {
    public class PostalAddress: ValueObject {
        public string StreetAddress { get; private set; }
        public string City { get; private set; }
        public string StateProvince { get; private set; }
        public string PostalCode { get; private set; }
        public string CountryCode { get; private set; }

        protected PostalAddress() {}

        public PostalAddress(string streetAddress, string city, string stateProvince, string postalCode, string countryCode) {
            this.StreetAddress = streetAddress;
            this.City = city;
            this.StateProvince = stateProvince;
            this.PostalCode = postalCode;
            this.CountryCode = countryCode;
        }

        public PostalAddress(PostalAddress postalAddress)
            : this(postalAddress.StreetAddress, postalAddress.City, postalAddress.StateProvince,
                postalAddress.PostalCode, postalAddress.CountryCode) {}

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.StreetAddress;
            yield return this.City;
            yield return this.StateProvince;
            yield return this.PostalCode;
            yield return this.CountryCode;
        }

        public override string ToString() {
            return "PostalAddress [streetAddress=" + StreetAddress
                    + ", city=" + City + ", stateProvince=" + StateProvince
                    + ", postalCode=" + PostalCode
                    + ", countryCode=" + CountryCode + "]";
        }
    }
}