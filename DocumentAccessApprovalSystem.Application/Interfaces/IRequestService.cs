using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAccessApprovalSystem.Application.Models;
using DocumentAccessApprovalSystem.Domain.Entities;

namespace DocumentAccessApprovalSystem.Application.Interfaces
{
    public interface IRequestService
    {
        /// <summary>
        /// Creates a new access request based on the provided data transfer object (DTO).
        /// </summary>
        /// <param name="accessRequestDto">The DTO containing access request details.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateAccessRequest(CreateAccessRequestDto accessRequestDto);

        /// <summary>
        /// Retrieves all access requests made by the specified user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation, containing a collection of access requests.</returns>
        Task<IEnumerable<AccessRequest>> GetRequestsByUser(int userId);

      
    }
}
