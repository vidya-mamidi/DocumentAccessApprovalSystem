using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentAccessApprovalSystem.Domain.Entities
{

   public class User
    {
        public int  Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// Initialized to <see cref="string.Empty"/> to ensure the property is never null,
        /// which helps prevent potential <see cref="NullReferenceException"/> errors
        /// when accessing or manipulating the string.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.User;
   }
}
