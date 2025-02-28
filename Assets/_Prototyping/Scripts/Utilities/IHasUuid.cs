using System;

namespace _Prototyping.Utilities
{
	public interface IHasUuid
	{
		public Guid uuid { get; set; }
		
		public void GenerateUuid()
		{
			this.uuid = Guid.NewGuid();
		}
	}
}