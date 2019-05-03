using System.Collections.Generic;
using ProfileStore.Client.Models;

namespace ProfileStore.Client.Interfaces
{
    /// <summary>
    /// Used to access the scopes in the Episerver profile store
    /// </summary>
    public interface IScopeRepository
    {
        /// <summary>
        /// Returns all the scopes available in the environment
        /// </summary>
        /// <returns>List of all scope</returns>
        IList<Scope> GetAllScopes();

        /// <summary>
        /// Get a single scope
        /// </summary>
        /// <param name="scopeId">The scopeId to retrieve</param>
        /// <returns>The scope if found, null if not found</returns>
        Scope GetScope(string scopeId);

        /// <summary>
        /// Insert or update a scope
        /// </summary>
        /// <param name="scope">The scope object to insert or update</param>
        /// <returns>True if the scope was inserted or updated, false if an error occured</returns>
        bool UpsertScope(Scope scope);
    }
}