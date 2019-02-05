using UnityEngine;
 
 namespace CastleDBImporter
 {
    [CreateAssetMenu(fileName = "CastleDBConfig.asset", menuName = "CastleDB/Config Asset", order = 5000)]
    public class CastleDBConfig : ScriptableObject
    {
        public string GUIDColumnName = "id";
        public string GeneratedTypesLocation = "Database";
        public string GeneratedTypesNamespace = "Database";
    }
 }