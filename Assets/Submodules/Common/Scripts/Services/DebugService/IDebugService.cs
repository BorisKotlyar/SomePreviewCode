using System;
using UnityEngine;
using Zenject;

public interface IDebugService : IInitializable, IDisposable
{
	event Action<string> OnLog;
	
	void Log (string message);
	void Log (string message, Color color);
	void Log (string tag, string message);
	void Log (string tag, string message, Color color);
}
