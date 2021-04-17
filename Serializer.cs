using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using  OpenShell.Errors;

namespace OpenShell.Serialization   
{   
	public class Serializer
	{
		public static bool SerializeToBinary<T>(T instance, string byteFilePath)	//	Serializes the provided object instance to a binary file
		{
			FileStream stream = null; 
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			
			stream = File.Open(byteFilePath, FileMode.OpenOrCreate);
  
			binaryFormatter.Serialize(stream, instance);

			stream.Close();

			return true;
		}

		public static T DeserializeBinary<T>(string byteFilePath)
		{
			FileStream stream = null; 

			BinaryFormatter binaryFormatter = new BinaryFormatter();

			T DeserializedBinary;
			
			try
			{
				stream = File.Open(byteFilePath, FileMode.Open);
			}
			catch (FileNotFoundException)
			{
				Error.Log($"{byteFilePath} was not found.");

				return (T)(new Object());
			}

			DeserializedBinary = (T)binaryFormatter.Deserialize(stream);

			stream.Close(); 

			return DeserializedBinary;
		}
	}
}
