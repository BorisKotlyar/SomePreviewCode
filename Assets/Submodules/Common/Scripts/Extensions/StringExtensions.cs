using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Linq;

public static class StringExtensions
{
	public static void LogEditor(this string value)
	{
		#if UNITY_EDITOR
		Debug.Log(value);
		#endif
	}

	public static void LogEditor(this string value, Color color)
	{
		#if UNITY_EDITOR
		Debug.LogFormat("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(color), value);
		#endif
	}

	public static string Color(this string value, Color color)
	{
		return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(color), value);
	}

	public static string EncryptToBase64(this string value)
	{
		return Convert.ToBase64String(Encoding.Default.GetBytes(value));
	}

	public static string DecryptFromBase64ToString(this string value)
	{
		return Encoding.Default.GetString(Convert.FromBase64String(value.Length % 4 == 0 ? value : value + "=="));
	}

    // TODO:[BORIS] add compress and decompress func
	///// <summary>
	///// Compresses the string.
	///// </summary>
	///// <param name="text">The text.</param>
	///// <returns></returns>
	//public static string CompressString(this string text)
	//{
	//	var buffer = Encoding.UTF8.GetBytes(text);
	//	var memoryStream = new MemoryStream();
	//	using (var gZipStream = new Ionic.Zlib.GZipStream(memoryStream, Ionic.Zlib.CompressionMode.Compress, true))
	//	{
	//		gZipStream.Write(buffer, 0, buffer.Length);
	//	}

	//	memoryStream.Position = 0;

	//	var compressedData = new byte[memoryStream.Length];
	//	memoryStream.Read(compressedData, 0, compressedData.Length);

	//	var gZipBuffer = new byte[compressedData.Length + 4];
	//	Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length);
	//	Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4);
	//	return Convert.ToBase64String(gZipBuffer);
	//}

	///// <summary>
	///// Decompresses the string.
	///// </summary>
	///// <param name="compressedText">The compressed text.</param>
	///// <returns></returns>
	//public static string DecompressString(this string compressedText)
	//{
	//	var gZipBuffer = Convert.FromBase64String(compressedText);
	//	using (var memoryStream = new MemoryStream())
	//	{
	//		var dataLength = BitConverter.ToInt32(gZipBuffer, 0);
	//		memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4);

	//		var buffer = new byte[dataLength];

	//		memoryStream.Position = 0;
	//		using (var gZipStream = new Ionic.Zlib.GZipStream(memoryStream, Ionic.Zlib.CompressionMode.Decompress))
	//		{
	//			gZipStream.Read(buffer, 0, buffer.Length);
	//		}

	//		return Encoding.UTF8.GetString(buffer);
	//	}
	//}

	/// <summary>
	/// арабик фиксер отражает текст. для правильных переносов нужна галка "справа налево" в TMPro,
	/// которая тоже отражает текст функция переотражает текст, чтобы он в итоге и выглядел правильно,
	/// и переносился по нужным словам
	/// </summary>
	/// <returns>The text.</returns>
	/// <param name="source">Source.</param>
	public static string ArabicReverseText(this string source)
	{
		var result = source.ToList();
		result.Reverse();
		return new string(result.ToArray());
	}
}
