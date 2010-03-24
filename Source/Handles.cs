
namespace Kata
{
    interface Handles<T> where T : Message
    {
        void Handle(T message);
    }
}
