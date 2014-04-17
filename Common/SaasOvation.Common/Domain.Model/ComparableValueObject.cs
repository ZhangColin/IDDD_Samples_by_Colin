using System;
using System.Collections.Generic;

namespace SaasOvation.Common.Domain.Model {
    public abstract class ComparableValueObject: ValueObject, IComparable {
        protected abstract IEnumerable<IComparable> GetComparableComponents();

        protected IComparable AsNonGenericComparable<T>(IComparable<T> comparable) {
            return new NonGenericComparable<T>(comparable);
        }

        class NonGenericComparable<T>:IComparable {
            private readonly IComparable<T> _comparable;

            public NonGenericComparable(IComparable<T> comparable) {
                this._comparable = comparable;
            }

            public int CompareTo(object obj) {
                if(object.ReferenceEquals(this._comparable, obj)) {
                    return 0;
                }
                if(object.ReferenceEquals(null, obj)) {
                    throw new ArgumentException();
                }
                return this._comparable.CompareTo((T)obj);
            }
        }

        protected int CompareTo(ComparableValueObject other) {
            using(IEnumerator<IComparable> thisComponents = this.GetComparableComponents().GetEnumerator()) {
                using(IEnumerator<IComparable> otherComponents = other.GetComparableComponents().GetEnumerator()) {
                    while(true) {
                        bool x = thisComponents.MoveNext();
                        bool y = otherComponents.MoveNext();
                        if(x!=y) {
                            throw new InvalidOperationException();
                        }
                        if(x) {
                            int c = thisComponents.Current.CompareTo(otherComponents.Current);
                            if(c!=0) {
                                return c;
                            }
                        }
                        else {
                            break;
                        }
                    }
                    return 0;
                }
            }
        }

        public int CompareTo(object obj) {
            if(object.ReferenceEquals(this, obj)) {
                return 0;
            }
            if(object.ReferenceEquals(null, obj)) {
                return 1;
            }

            if(this.GetType()!=obj.GetType()) {
                throw new InvalidOperationException();
            }

            return CompareTo(obj as ComparableValueObject);
        }
    }

    public abstract class ComparableValueObject<T>: ComparableValueObject, IComparable<T>
        where T: ComparableValueObject<T> {
        public int CompareTo(T other) {
            return base.CompareTo(other);
        }
    }
}