using System;
using System.Data;

namespace SaasOvation.Common.Port.Adapter.Persistence {
    public class JoinOn {
        private readonly string _leftKey;
        private readonly string _rightKey;
        private object _currentLeftQualifier;

        public JoinOn(string leftKey, string rightKey) {
            this._leftKey = leftKey;
            this._rightKey = rightKey;
        }

        public bool IsSpecified {
            get { return this._leftKey != null && this._rightKey != null; }
        }

        public bool HasCurrentLeftQualifier(IDataReader dataReader) {
            try {
                object columnValue = dataReader.GetValue(dataReader.GetOrdinal(this._leftKey));
                if(columnValue==null) {
                    return false;
                }
                return columnValue.Equals(this._currentLeftQualifier);
            }
            catch(Exception) {
                return false;
            }
        }

        public bool IsJoinedOn(IDataReader dataReader) {
            object leftColumn = null;
            object rightColumn = null;
            try {
                if(IsSpecified) {
                    leftColumn = dataReader.GetValue(dataReader.GetOrdinal(this._leftKey));
                    rightColumn = dataReader.GetValue(dataReader.GetOrdinal(this._rightKey));
                }
            }
            catch(Exception) {
            }

            return leftColumn != null && rightColumn != null;
        }

        public void SaveCurrentLeftQualifier(string columnName, object columnValue) {
            if(columnName.Equals(this._leftKey)) {
                this._currentLeftQualifier = columnValue;
            }
        }
    }
}