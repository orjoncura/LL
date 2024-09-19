namespace LL.Core.Interfaces.Extensions;

public interface IAgentService
{ 
    Task<string> Run(string input);
}