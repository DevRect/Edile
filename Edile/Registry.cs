using System.Linq;
using System.Collections.Generic;

namespace Edile
{
	/// <summary>
	/// Registry of entities and components.
	/// Provides tools for creating, processing, and deleting entities and components.
	/// </summary>
	public class Registry
	{
		public Registry()
		{
			this.component_manager = new ComponentManager();
			this.null_entities = new HashSet<int>();
		}

		/// <summary>
		/// Creates entity. The entity is a general-purpose object that has a unique id.
		/// </summary>
		/// <returns>Unique id of created entity.</returns>
		public int CreateEntity()
		{
			if(this.null_entities.Count != 0)
			{
				int id = this.null_entities.ElementAt(0);
				this.null_entities.Remove(id);

				return id;
			}

			return this.last_entity_id++;
		}

		/// <summary>
		/// Removes entity by unique id.
		/// </summary>
		/// <param name="id"></param>
		public void RemoveEntity(int id)
		{
			this.component_manager.RemoveComponentsByOwner(id);
			this.null_entities.Add(id);
		}

		/// <summary>
		/// Creates component T using the provided arguments and attaching it to the entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parameters"></param>
		/// <returns>The created instance of component T.</returns>
		public T AttachComponent<T>(params object[] parameters) where T : Component
		{
			return this.component_manager.CreateComponent<T>(parameters);
		}

		/// <summary>
		/// Destroy component T instance of the entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="OwnerID"></param>
		/// <returns>Success of removing an entity component.</returns>
		public bool DetachComponent<T>(int owner) where T : Component
		{
			return this.component_manager.RemoveComponent<T>(owner);
		}

		/// <summary>
		/// Destroys each component of the entity.
		/// </summary>
		/// <param name="OwnerID"></param>
		public void RemoveComponentByOwner(int owner)
		{
			this.component_manager.RemoveComponentsByOwner(owner);
		}

		/// <summary>
		/// Destroys each component of type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public void RemoveComponentsByType<T>() where T : Component
		{
			this.component_manager.RemoveComponentsByType<T>();
		}

		/// <summary>
		/// Checks the entity for the presence of the T component
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="OwnerID"></param>
		/// <returns>Еhe presence of the component T in the entity</returns>
		public bool HasComponent<T>(int owner) where T : Component
		{
			return this.component_manager.HasComponent<T>(owner);
		}

		/// <summary>
		/// Get an instance of an entity component T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="OwnerID"></param>
		/// <returns>Component T instance.</returns>
		public T GetComponent<T>(int owner) where T : Component
		{
			return this.component_manager.GetComponent<T>(owner);
		}

		/// <summary>
		/// Get an component group. Component group store components of same type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>Group of components T.</returns>
		public List<T> GetComponentGroup<T>() where T : Component
		{
			return this.component_manager.GetComponentGroup<T>();
		}

		private int last_entity_id;
		private HashSet<int> null_entities;
		private ComponentManager component_manager;
	}
}
