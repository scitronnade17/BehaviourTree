using System.Collections.Generic;
using Zenject;

public interface IDIService
{
    DiContainer Container { get; }
    DiContainer GetContainer();
    T Resolve<T>();
    IEnumerable<T> ResolveAll<T>();
}

public class DIService: IDIService
{
    public DiContainer Container => ProjectContext.Instance.Container;

    public DiContainer GetContainer() => Container;

    public T Resolve<T>()
    {
        return Container.Resolve<T>();
    }
    public IEnumerable<T> ResolveAll<T>()
    {
        return Container.ResolveAll<T>();
    }
}
