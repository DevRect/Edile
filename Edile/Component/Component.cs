namespace Edile
{
	/// <summary>
	/// The raw data for one aspect of the object.
	/// </summary>
	public abstract class Component
	{
		public Component(int entity_id)
		{
			this.EntityID = entity_id;
		}

		public readonly int EntityID;
	}
}
