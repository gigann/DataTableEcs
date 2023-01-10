namespace DataTableEcs
{
    /// <summary>
    /// An entity is just a container for an ID.
    /// They are intended for quick discardation by the GC.
    /// </summary>
    public class Entity
    {
        private EntityManager _manager;
        public int ID { get; set; }

        /// <summary>
        /// Creates an entity managed by a particular entity manager.
        /// </summary>
        /// <param name="manager"></param>
        public Entity(EntityManager manager)
        {
            _manager = manager;
            _manager.RegisterEntity(this);
        }

        internal Entity(int id)
        {
            ID = id;
        }

        public void Destroy()
        {
            _manager.DestroyEntity(ID);
        }

        /// <summary>
        /// Returns a particular component-instance belonging to an entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>() where T : Component
        {
            return _manager.GetComponent<T>(this);
        }

        public void Add<T>(T component) where T : Component
        {
            _manager.AddComponent<T>(this, component);
        }

        public void Remove<T>() where T : Component
        {
            _manager.RemoveComponent<T>(this);
        }
    }
}