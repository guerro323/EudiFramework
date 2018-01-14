using System;

namespace EudiFramework
{
    /// <summary>
    /// Indicate a struct that use a id
    /// </summary>
    public interface IStructWithId
    {
        //int worldId { get; }
    }
    
    /// <summary>
    /// Indicate a module that is not shareable and is only used for a component
    /// </summary>
    public interface IModule
    {
        
    }

    /// <summary>
    /// Indicate a module that is shareable in the entity (only one instance of this module will be present)
    /// </summary>
    public interface IShareableModule : IModule, IStructWithId
    {
        
    }
}