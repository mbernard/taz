using RestSharp.Deserializers;
using RestSharp.Serializers;

namespace Taz.Core
{
    public interface IJsonSerializer : ISerializer, IDeserializer
    {
    }
}
