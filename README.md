 Edile - Entity Component
==========================
Entity Component is a design pattern that provides flexibility in software design.

 Examples 
----------

* Create and remove entities:
    ```
    Edile.Registry registry = new Edile.Registry();
        
    var entity_1 = registry.CreateEntity();
    registry.RemoveEntity(entity_1);
    ```
* Create, handle and remove components:
    ```
    ...
        public class HealthComponent : Edile.Component
        {
            public HealthComponent(int owner, int value) : base(owner)
            {
                this.Health = value;
            }
    
            public int Health { get; set; }
        }
        
        static void Main(string[] args)
        {
            var registry = new Edile.Registry();
            
            var entities = new List<int>();
            for(int i = 0; i < 50; i += 1)
            {
                entities.Add(registry.CreateEntity());
                if(i % 2 == 0) entities.AttachComponent<HealthComponent>()
            }
        }
    ...
    ```