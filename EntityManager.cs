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

        public DataTable Entities { get; set; } = new DataTable();

        public int EntityCapacity { get; private set; } = 0;
        public int EntityCount { get; private set; } = 0;

        public EntityManager(int entityCapacity = int.MaxValue)
        {
            EntityCapacity = entityCapacity;

            DataColumn entityIDColumn = Entities.Columns.Add("ID", typeof(Int32));
            entityIDColumn.AllowDBNull = false;
            entityIDColumn.Unique = true;
            Entities.PrimaryKey = new DataColumn[1] { entityIDColumn };
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
            var newRow = Entities.NewRow();
            newRow[0] = e.ID;
            Entities.Rows.Add(newRow);
        }

        public void RegisterComponent<T>()
        {
            if (!Entities.Columns.Contains(typeof(T).Name))
            {
                Entities.Columns.Add(typeof(T).Name, typeof(T));
            }
        }

        public void DestroyEntity(int ID)
        {
            DataRow workingRow = Entities.Rows.Find(ID);

            if (workingRow != null)
            {
                Entities.Rows.Remove(workingRow);
                _recycledIDs.Push(ID);
            }
        }

        internal T GetComponent<T>(Entity entity) where T : Component
        {
            var workingRow = Entities.Rows.Find(entity.ID);
            return (T)workingRow[typeof(T).Name];

        }

        internal void AddComponent<T>(Entity entity, T component) where T : Component
        {
            RegisterComponent<T>();
            var workingRow = Entities.Rows.Find(entity.ID);
            workingRow[typeof(T).Name] = component;

        }

        internal void RemoveComponent<T>(Entity entity) where T : Component
        {
            var workingRow = Entities.Rows.Find(entity.ID);
            workingRow[typeof(T).Name] = DBNull.Value;

        }

        public void Clear()
        {
            Entities.Clear();
        }

        public string PrintData()
        {
            string retVal = "";

            foreach (DataColumn col in Entities.Columns)
            {
                retVal += col.ToString().PadRight(40) + "\t";
            }
            retVal += "\n";

            foreach (DataRow row in Entities.Rows)
            {
                foreach (DataColumn col in Entities.Columns)
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