using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace SaasOvation.Common.Port.Adapter.Persistence {
    public class ResultSetObjectMapper<T> {
        private readonly IDataReader _dataReader;
        private readonly JoinOn _joinOn;
        private readonly string _columnPrefix;

        public ResultSetObjectMapper(IDataReader dataReader, JoinOn joinOn, string columnPrefix = null) {
            this._dataReader = dataReader;
            this._joinOn = joinOn;
            this._columnPrefix = columnPrefix;
        }

        public T MapResultToType() {
            T obj = default(T);
            HashSet<string> associationsToMap = new HashSet<string>();

            FieldInfo[] fields = typeof(T).GetFields();
            foreach(FieldInfo field in fields) {
                string columnName = this.FieldNameToColumnName(field.Name);
                int columnIndex = this._dataReader.GetOrdinal(columnName);
                if(columnIndex>=0) {
                    object columnValue = this.ColumnValueFrom(columnIndex, field.FieldType);
                    this._joinOn.SaveCurrentLeftQualifier(columnName, columnValue);
                    field.SetValue(obj, columnName);
                }
                else {
                    string objectPrefix = ToObjectPrefix(columnName);
                    if(!associationsToMap.Contains(objectPrefix) && HasAssociation(objectPrefix)) {
                        associationsToMap.Add(field.Name);
                    }
                }
            }

            if(associationsToMap.Count>0) {
                MapAssociations(obj, associationsToMap);
            }

            return obj;
        }

        private void MapAssociations(object obj, ISet<string> associationsToMap) {
            Dictionary<string, ICollection<object>> mappedCollections = new Dictionary<string, ICollection<object>>();
            while(this._dataReader.NextResult()) {
                foreach(string fieldName in associationsToMap) {
                    FieldInfo associationField = typeof(T).GetField(fieldName);
                    ICollection<object> collection = default (ICollection<object>);

                    if(typeof(IEnumerable).IsAssignableFrom(associationField.FieldType)) {
                        if(!mappedCollections.TryGetValue(fieldName, out collection)) {
                            collection = this.CreateCollectionFrom(associationField.FieldType);
                            mappedCollections.Add(fieldName, collection);
                            associationField.SetValue(obj, collection);
                        }
                    }

                    string columnName = this.FieldNameToColumnName(fieldName);
                    ResultSetObjectMapper<object> mapper = new ResultSetObjectMapper<object>(_dataReader, _joinOn,
                        this.ToObjectPrefix(columnName));

                    object associationObject = mapper.MapResultToType();

                    if(collection!=null) {
                        collection.Add(associationObject);
                    }
                    else {
                        associationField.SetValue(obj, associationObject);
                    }
                }
            }
        }

        private ICollection<object> CreateCollectionFrom(Type type) {
            Type genericType = type.GetGenericTypeDefinition();
            if(typeof(IList<>).IsAssignableFrom(genericType)) {
                return new List<object>();
            }
            else if(typeof(ISet<>).IsAssignableFrom(genericType)) {
                return new HashSet<object>();
            }
            else {
                return null;
            }
        } 

        private bool HasAssociation(string objectPrefix) {
            int fieldCount = this._dataReader.FieldCount;
            for(int i = 0; i < fieldCount; i++) {
                string columnName = this._dataReader.GetName(i);
                if(columnName.StartsWith(objectPrefix) && this._joinOn.IsJoinedOn(this._dataReader)) {
                    return true;
                }
            }
            return false;
        }

        private string ToObjectPrefix(string columnName) {
            return columnName;
        }

        private object ColumnValueFrom(int columnIndex, Type columnType) {
            switch(Type.GetTypeCode(columnType)) {
                case TypeCode.Int32:
                    return this._dataReader.GetInt32(columnIndex);
                case TypeCode.Int64:
                    return this._dataReader.GetInt64(columnIndex);
                case TypeCode.Boolean:
                    return this._dataReader.GetBoolean(columnIndex);
                case TypeCode.Int16:
                    return this._dataReader.GetInt16(columnIndex);
                case TypeCode.Single:
                    return this._dataReader.GetFloat(columnIndex);
                case TypeCode.Double:
                    return this._dataReader.GetDouble(columnIndex);
                case TypeCode.Byte:
                    return this._dataReader.GetByte(columnIndex);
                case TypeCode.Char:
                    return this._dataReader.GetChar(columnIndex);
                case TypeCode.String:
                    return this._dataReader.GetString(columnIndex);
                case TypeCode.DateTime:
                    return this._dataReader.GetDateTime(columnIndex);
                default:
                    throw new InvalidOperationException("Unsupported type.");
            }
        }

        private string FieldNameToColumnName(string fieldName) {
            StringBuilder sb = new StringBuilder();
            if(this._columnPrefix!=null) {
                return _columnPrefix + fieldName;
            }

            return fieldName;
        }
    }
}