using System.Threading.Tasks;

namespace MyDeckApi_Experimental.Interfaces
{
    public interface IUseCase<T,Params>
    {
         Task<T> Invoke(Params param);
    }

    public interface Params{}
}