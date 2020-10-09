using System.Net.Http;

namespace LofiWebPlayer.Biz.Manager.ServiceLocators
{
    internal interface IHttpMessageHandlerFactory
    {
        HttpMessageHandler CreateHttpMessageHandler();
    }
}
