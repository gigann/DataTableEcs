using System.Data;

namespace DataTableEcs
{
    /// <summary>
    /// Manages entities.
    /// </summary>
    public class EntityManager
    {
        private int _nextID = 0;
        private Stack<int> _recycledIDs = new Stack<int>();

        private DataTable _entities = new DataTable();

        public int EntityCapacity { get; private set; } = 0;
        public int EntityCount { get; private set; } = 0;

        public EntityManager(int entityCapacity = int.MaxValue)
        {
            EntityCapacity = entityCapacity;

            DataColumn entityIDColumn = _entities.Columns.Add("ID", typeof(Int32));
            entityIDColumn.AllowDBNull = false;
            entityIDColumn.Unique = true;
            _entities.PrimaryKey = new DataColumn[1] { entityIDColumn };
        }

        public Entity GetEntity(int ID)
        {
            return new Entity(ID);
        }

        private int GetNextID()
        {
            int returnID;

            if (_recycledIDs.Count > 0)
            {
                returnID = _recycledIDs.Pop();
            }
            else if (_nextID < EntityCapacity)
            {
                returnID = _nextID;
                _nextID++;
            }
            else
            {
                throw new Exception("Error: Entity ID Overflow!");
            }

            return returnID;
        }

        public void RegisterEntity(Entity e)
        {
            e.ID = GetNextID();
            var newRow = _entities.NewRow();
            newRow[0] = e.ID;
            _entities.Rows.Add(newRow);
        }

        public void RegisterComponent<T>()
        {
            if (!_entities.Columns.Contains(typeof(T).Name))
            {
                _entities.Columns.Add(typeof(T).Name, typeof(T));
            }
        }

        public void DestroyEntity(int ID)
        {
            DataRow workingRow = _entities.Rows.Find(ID);

            if (workingRow != null)
            {
                _entities.Rows.Remove(workingRow);
                _recycledIDs.Push(ID);
            }
        }

        internal T GetComponent<T>(Entity entity) where T : Component
        {
            var workingRow = _entities.Rows.Find(entity.ID);
            return (T)workingRow[typeof(T).Name];

        }

        internal void AddComponent<T>(Entity entity, T component) where T : Component
        {
            RegisterComponent<T>();
            var workingRow = _entities.Rows.Find(entity.ID);
            workingRow[typeof(T).Name] = component;

        }

        internal void RemoveComponent<T>(Entity entity) where T : Component
        {
            var workingRow = _entities.Rows.Find(entity.ID);
            workingRow[typeof(T).Name] = DBNull.Value;

        }

        public void Clear()
        {
            _entities.Clear();
        }

        public string PrintData()
        {
            string retVal = "";

            foreach (DataColumn col in _entities.Columns)
            {
                retVal += col.ToString().PadRight(40) + "\t";
            }
            retVal += "\n";

            foreach (DataRow row in _entities.Rows)
            {
                foreach (DataColumn col in _entities.Columns)
                {
                    retVal += row[col].ToString().PadRight(40) + "\t";
                }
                retVal += "\n";
            }

            return retVal;
        }

        /// <summary>
        /// Saves the Entity Manager's data to a file.
        /// </summary>
        public void Serialize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the Entity Manager's data based on the saved file.
        /// </summary>
        public void Deserialize()
        {
            throw new NotImplementedException();
        }
    }
}