using System.Collections.Generic;

namespace OpenShell
{
	/// <summary>
	/// Makes classes comparable.
	/// </summary>
	/// <typeparam name="T"> The tpye of class extending the interface. </typeparam>
	public interface IComparable<T>
	{
		bool IsEqual(T obj);	//	Checks if the instance of the class is equal to the provided instance.
	}
}
