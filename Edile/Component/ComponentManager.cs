using System;
using System.Linq;
using System.Collections.Generic;
using Edile.Utils;

namespace Edile
{
	/// <summary>
	/// A class that handles creating, processing, and deleting components.
	/// </summary>
	public class ComponentManager
	{
		public ComponentManager(int reserve = 64)
		{
			this.reserve = reserve;
			this.component_groups = new List<List<Component>>(this.reserve);
		}

		/// <summary>
		/// Creates component T using the provided arguments and attaching it to the entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="parameters"></param>
		/// <returns>The created instance of component T.</returns>
		public T CreateComponent<T>(params object[] parameters) where T : Component
		{
			// Store ID of component type
			// And ID of component owner
			int TypeID = FamilyTypeID<Component>.Get<T>();
			int EntityID = (int)parameters[0];

			// Create component instance
			T component = (T)Activator.CreateInstance(typeof(T), args:parameters);

			// Fill list with component groups
			while (this.component_groups.Count <= TypeID)
				this.component_groups.Add(new List<Component>(this.reserve));

			// Fill component group with null elements
			while (this.component_groups[TypeID].Count <= EntityID)
			{
				this.component_groups[TypeID].Add(null);
			}

			// Store the instance of component T in component group
			this.component_groups[TypeID][EntityID] = component;

			return component;
		}

		/// <summary>
		/// Checks for presense of component instance.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="OwnerID"></param>
		/// <returns>The presense of component T in the entity.</returns>
		public bool HasComponent<T>(int OwnerID) where T : Component
		{
			// Store ID of component type
			int TypeID = FamilyTypeID<Component>.Get<T>();

			// Return false if component group not created yet
			if (this.component_groups.Count <= TypeID)
				return false;

			// Return false if component not created yet
			if (this.component_groups[TypeID].Count <= OwnerID)
				return false;

			// Return false if component does not exist
			if (this.component_groups[TypeID][OwnerID] == null)
				return false;

			// Component exist, return true
			return true;
		}

		/// <summary>
		/// Destroy component T instance of the entity.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="OwnerID"></param>
		/// <returns>Success of removing an entity component.</returns>
		public bool RemoveComponent<T>(int OwnerID) where T : Component
		{
			// Store ID of component type
			int TypeID = FamilyTypeID<Component>.Get<T>();

			// Return false if entity has no component
			if (this.HasComponent<T>(OwnerID) == false)
				return false;

			// Remove component, return true
			this.component_groups[TypeID][OwnerID] = null;
			return true;
		}

		/// <summary>
		/// Destroys each component of the entity.
		/// </summary>
		/// <param name="OwnerID"></param>
		public void RemoveComponentsByOwner(int OwnerID)
		{
			// Remove component of each type with OwnerID
			for (int TypeID = 0; TypeID < this.component_groups.Count; TypeID += 1)
			{
				// Continue if component not created yet
				if (this.component_groups[TypeID].Count <= OwnerID) continue;

				// Continue if component does not exist
				if (this.component_groups[TypeID][OwnerID] == null) continue;

				// Remove component
				this.component_groups[TypeID][OwnerID] = null;
			}
		}

		/// <summary>
		/// Destroys each component of type T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public bool RemoveComponentsByType<T>() where T : Component
		{
			// Store ID of component type
			int TypeID = FamilyTypeID<Component>.Get<T>();

			// Return false if component group not created yet
			if (this.component_groups.Count <= TypeID)
				return false;

			// Remove each component of type T
			for (int OwnerID = 0; OwnerID < this.component_groups[TypeID].Count; OwnerID += 1)
			{
				this.RemoveComponent<T>(OwnerID);
			}

			return true;
		}

		/// <summary>
		/// Get an instance of an entity component T.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="OwnerID"></param>
		/// <returns>Component T instance.</returns>
		public T GetComponent<T>(int OwnerID) where T : Component
		{
			// Store ID of component type
			int TypeID = FamilyTypeID<Component>.Get<T>();

			// Return null if component group not created yet
			if (this.component_groups.Count <= TypeID)
				return default(T);

			// Return null if component not created yet
			if (this.component_groups[TypeID].Count <= OwnerID)
				return default(T);

			// Component exist or null
			return (T)this.component_groups[TypeID][OwnerID];
		}

		/// <summary>
		/// Get an component group. Component group store components of same type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns>Group of components T.</returns>
		public List<T> GetComponentGroup<T>() where T : Component
		{
			// Store ID of component type
			int TypeID = FamilyTypeID<Component>.Get<T>();

			// If component not created yet, return null
			if (this.component_groups.Count <= TypeID)
				return default(List<T>);

			return this.component_groups[TypeID].Cast<T>().ToList();
		}

		private readonly int reserve;
		private List<List<Component>> component_groups;
	}
}
