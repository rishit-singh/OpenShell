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
  
			try
			{
				binaryFormatter.Serialize(stream, instance);
			}
			catch (SerializationException e)
			{
				Error.Log($"Serialization failed with errors: {e.Message}");
				
				stream.Close();

				return false;
			}
			
			stream.Close();
			

			return true;
		}

		public static T DeserializeBinary<T>(string byteFilePath)
		{
			FileStream stream = null; 

			BinaryFormatter binaryFormatter = new BinaryFormatter();

			T? DeserializedBinary;
			
			try
			{
				stream = File.Open(byteFilePath, FileMode.Open);
			}
			catch (FileNotFoundException)
			{
				Error.Log($"{byteFilePath} was not found.");

				return default(T);
			}

			try
			{
				DeserializedBinary = (T)binaryFormatter.Deserialize(stream);
			}
			catch (SerializationException e)
			{
				Error.Log($"Serialization failed with errors: {e.Message}");
				
				stream.Close();

				return default(T);
			}

			stream.Close(); 

			return DeserializedBinary;
		}
	}
}
