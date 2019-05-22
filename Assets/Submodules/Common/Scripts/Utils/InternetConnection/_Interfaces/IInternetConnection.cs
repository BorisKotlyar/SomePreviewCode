using System;

public interface IInternetConnection
{
    bool IsInternetExists { get; }

    void CheckConnection(Action onSuccess, Action onFail, LostInternetMessage messageType = LostInternetMessage.none);

    void ShowLostInternetMessage(LostInternetMessage messageType, Action onClose = null);
}
